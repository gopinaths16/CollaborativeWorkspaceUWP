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

        public long CommentId {  get; set; }

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
            else if (task == null)
            {
                isLoaded = false;
                CurrTask = null;
                NotifyPropertyChanged(nameof(CurrTask));
            }
        }

        public async Task AddAttachmentToTask(StorageFile file)
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
                await AddAttachmentToLocalFolder(attachment, true);
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
            await AddAttachmentToLocalFolder(attachment, false);
        }

        public async Task AddAttachmentToLocalFolder(Attachment attachment, bool isTempItem)
        {
            StorageFolder folder = null;
            StorageFolder tempFolder = null;
            try
            {
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                try
                {
                    folder = await storageFolder.GetFolderAsync("Attachments");
                    if(isTempItem)
                    {
                        tempFolder = await folder.GetFolderAsync("Temp");
                        folder = tempFolder;
                    }
                }
                catch(Exception ex)
                {

                }
                if (folder == null && !isTempItem)
                {
                    folder = await storageFolder.CreateFolderAsync("Attachments");
                }
                else if(tempFolder == null && isTempItem)
                {
                    tempFolder = await folder.CreateFolderAsync("Temp");
                    folder = tempFolder;
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
                StorageFolder folder = ApplicationData.Current.LocalFolder;
                folder = await folder.GetFolderAsync("Attachments");
                folder = await folder.GetFolderAsync("Temp");
                StorageFile tempFile = await folder.GetFileAsync(attachment.Path);
                if(tempFile != null)
                {
                    await tempFile.DeleteAsync();
                }
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
            if (CurrTask != null && addAttachmentEvent != null && addAttachmentEvent.Task.Id == CurrTask.Id && !IsOnlyForAddition)
            {
                if (addAttachmentEvent.Attachment != null && CurrTask.Attachments.Where(att => att.Id == addAttachmentEvent.Attachment.Id).Count() <= 0 && (addAttachmentEvent.Attachment.CommentId <= 0 || addAttachmentEvent.Attachment.CommentId == CommentId || AdditionAllowedFromUI))
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
                var attachment = CurrTask.Attachments.Where(att => att.Id == delAttachmentEvent.Attachment.Id);
                if(attachment.Count() > 0)
                {
                    CurrTask.Attachments.Remove(attachment.First());
                }
            }
            NotifyPropertyChanged(nameof(CurrTask));
        }

        public async Task OnAttachmentRemoval(RemoveAttachmentEvent remAttachmentEvent)
        {
            if(CurrTask != null && !AdditionAllowedFromUI && IsOnlyForAddition && remAttachmentEvent.Attachment.TaskId == CurrTask.Id)
            {
                CurrTask.Attachments.Remove(remAttachmentEvent.Attachment);
                StorageFolder folder = ApplicationData.Current.LocalFolder;
                folder = await folder.GetFolderAsync("Attachments");
                folder = await folder.GetFolderAsync("Temp");
                StorageFile file = await folder.GetFileAsync(remAttachmentEvent.Attachment.Path);
                if(file != null)
                {
                    await file.DeleteAsync();
                }
                NotifyPropertyChanged(nameof(CurrTask));
            }
        }
    }
}
