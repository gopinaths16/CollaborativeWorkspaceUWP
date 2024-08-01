using CollaborativeWorkspaceUWP.DAL;
using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Utilities;
using CollaborativeWorkspaceUWP.Utilities.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace CollaborativeWorkspaceUWP.ViewModels
{
    public class AttachmentViewModel : BaseViewModel
    {
        private UserTask currTask;
        private ObservableCollection<Attachment> attachments;
        private AttachmentDataHandler attachmentDataHandler;

        private bool isLoaded;

        public ObservableCollection<Attachment> Attachments
        {
            get { return attachments; }
            set { attachments = value; }
        }

        public UserTask CurrTask
        {
            get { return currTask; }
            set
            {
                currTask = value;
                NotifyPropertyChanged(nameof(CurrTask));
            }
        }

        public bool AdditionAllowedFromUI;
        public bool IsOnlyForAddition;

        public AttachmentViewModel()
        {
            attachmentDataHandler = new AttachmentDataHandler();

            ViewmodelEventHandler.Instance.Subscribe<AddAttachmentEvent>(OnAttachmentAddition);
            ViewmodelEventHandler.Instance.Subscribe<DeleteAttachmentEvent>(OnAttachmentDeletion);
            ViewmodelEventHandler.Instance.Subscribe<RemoveAttachmentEvent>(OnAttachmentRemoval);
        }

        public void SetCurrTask(UserTask task)
        {
            if (task != null && !isLoaded)
            {
                isLoaded = true;
                CurrTask = (UserTask)task.Clone();
                NotifyPropertyChanged(nameof(CurrTask));
            }
        }

        public async void AddAttachmentToTask(StorageFile file)
        {
            Attachment attachment = new Attachment();
            attachment.Name = file.Name;
            attachment.Path = GetRandomFileName(Path.GetExtension(file.Path));
            attachment.Type = file.ContentType;
            attachment.TaskId = CurrTask.Id;
            attachment.CommentId = -1;
            attachment.Content = file;
            if(AdditionAllowedFromUI)
            {
                await AddAttachment(attachment);
            }
            else
            {
                attachment.IsOnlyForAddition = true;
                attachment.OriginalPath = file.Path;
                CurrTask.Attachments.Add(attachment);
                NotifyPropertyChanged(nameof(CurrTask));
            }
        }

        private async Task<Attachment> AddAttachment(Attachment attachment)
        {
            await AddAttachmentToLocalFolder(attachment);
            Attachment temp = attachmentDataHandler.AddAttachmentsToTask(attachment);
            await ViewmodelEventHandler.Instance.Publish(new AddAttachmentEvent() { Task = CurrTask, Attachment = temp });
            return temp;
        }

        public async Task AddAttachmentToLocalFolder(Attachment attachment)
        {
            StorageFolder folder = null;
            try
            {
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                try
                {
                    folder = await storageFolder.GetFolderAsync("Attachments");
                }
                catch(Exception ex)
                {

                }
                if (folder == null)
                {
                    folder = await storageFolder.CreateFolderAsync("Attachments");
                }
                StorageFile file = await folder.CreateFileAsync(attachment.Path);
                var content = await FileIO.ReadBufferAsync(attachment.Content);
                await FileIO.WriteBufferAsync(file, content);
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }

        public static string GetRandomFileName(string extension)
        {
            if (!extension.StartsWith("."))
            {
                extension = "." + extension;
            }

            string fileName = Path.GetRandomFileName();

            fileName = Path.ChangeExtension(fileName, extension);

            return fileName;
        }

        public void DeleteAttachmentFromList(Attachment attachment)
        {
            CurrTask.Attachments.Remove(attachment);
            NotifyPropertyChanged(nameof(CurrTask));
        }

        public void ClearAttachmentList()
        {
            CurrTask.Attachments.Clear();
            NotifyPropertyChanged(nameof(CurrTask));
        }

        public async Task<ObservableCollection<Attachment>> AddAttachmentForComment(long commentId)
        {
            ObservableCollection<Attachment> attachments = new ObservableCollection<Attachment>();
            foreach (var attachment in CurrTask.Attachments)
            {
                attachment.CommentId = commentId;
                Attachment temp = await AddAttachment(attachment);
                attachments.Add(temp);
            }
            ClearAttachmentList();
            return attachments;
        }

        public void SetAttachments(ObservableCollection<Attachment> attachments)
        {
            if(CurrTask != null)
            {
                CurrTask.Attachments = attachments;
                NotifyPropertyChanged(nameof(CurrTask));
            }
        }

        public async Task OnAttachmentAddition(AddAttachmentEvent addAttachmentEvent)
        {
            if (CurrTask != null && addAttachmentEvent != null && addAttachmentEvent.Task.Id == CurrTask.Id && AdditionAllowedFromUI)
            {
                if (addAttachmentEvent.Attachment != null && CurrTask.Attachments.Where(att => att.Id == addAttachmentEvent.Attachment.Id).Count() <= 0)
                {
                    CurrTask.Attachments.Add(addAttachmentEvent.Attachment);
                }
            }
            NotifyPropertyChanged(nameof(CurrTask));
        }

        public async Task OnAttachmentDeletion(DeleteAttachmentEvent delAttachmentEvent)
        {
            if(CurrTask != null && CurrTask.Id == delAttachmentEvent.Attachment.TaskId)
            {
                attachmentDataHandler.DeleteAttachment(delAttachmentEvent.Attachment.Id);
                StorageFolder storageFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Attachments");
                StorageFile file = await storageFolder.GetFileAsync(delAttachmentEvent.Attachment.Path);
                await file.DeleteAsync();
                CurrTask.Attachments.Remove(delAttachmentEvent.Attachment);
                NotifyPropertyChanged(nameof(CurrTask));
            }
        }

        public async Task OnAttachmentRemoval(RemoveAttachmentEvent remAttachmentEvent)
        {
            if(CurrTask != null && !AdditionAllowedFromUI && IsOnlyForAddition && remAttachmentEvent.Attachment.TaskId == CurrTask.Id)
            {
                CurrTask.Attachments.Remove(remAttachmentEvent.Attachment);
                NotifyPropertyChanged(nameof(CurrTask));
            }
        }
    }
}
