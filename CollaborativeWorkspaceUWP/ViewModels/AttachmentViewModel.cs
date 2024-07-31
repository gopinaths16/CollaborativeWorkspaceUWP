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

namespace CollaborativeWorkspaceUWP.ViewModels
{
    public class AttachmentViewModel : BaseViewModel
    {
        private UserTask currTask;
        private ObservableCollection<Attachment> attachments;
        private AttachmentDataHandler attachmentDataHandler;

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

        public AttachmentViewModel()
        {
            attachmentDataHandler = new AttachmentDataHandler();

            ViewmodelEventHandler.Instance.Subscribe<AddAttachmentEvent>(OnAttachmentAddition);
            ViewmodelEventHandler.Instance.Subscribe<DeleteAttachmentEvent>(OnAttachmentDeletion);
        }

        public void SetCurrTask(UserTask task)
        {
            if (task != null)
            {
                CurrTask = task;
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
                CurrTask.Attachments.Add(attachment);
                NotifyPropertyChanged(nameof(CurrTask));
            }
        }

        private async Task<Attachment> AddAttachment(Attachment attachment)
        {
            await AddAttachmentToLocalFolder(attachment);
            Attachment temp = attachmentDataHandler.AddAttachmentsToTask(attachment);
            ViewmodelEventHandler.Instance.Publish(new AddAttachmentEvent() { Task = CurrTask, Attachment = temp });
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

        public void OnAttachmentAddition(AddAttachmentEvent addAttachmentEvent)
        {
            if (addAttachmentEvent != null && addAttachmentEvent.Task.Id == CurrTask.Id && AdditionAllowedFromUI)
            {
                if (addAttachmentEvent.Attachment != null && CurrTask.Attachments.Where(att => att.Id == addAttachmentEvent.Attachment.Id).Count() <= 0)
                {
                    CurrTask.Attachments.Add(addAttachmentEvent.Attachment);
                }
            }
            NotifyPropertyChanged(nameof(CurrTask));
        }

        public void OnAttachmentDeletion(DeleteAttachmentEvent delAttachmentEvent)
        {
            if(!AdditionAllowedFromUI)
            {
                CurrTask.Attachments.Remove(delAttachmentEvent.Attachment);
                NotifyPropertyChanged(nameof(CurrTask));
            }
        }
    }
}
