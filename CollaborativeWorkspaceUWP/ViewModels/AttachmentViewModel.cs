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
            Attachments = new ObservableCollection<Attachment>();
            attachmentDataHandler = new AttachmentDataHandler();

            ViewmodelEventHandler.Instance.Subscribe<AddAttachmentEvent>(OnAttachmentAddition);
            ViewmodelEventHandler.Instance.Subscribe<DeleteAttachmentEvent>(OnAttachmentDeletion);
        }

        public void SetCurrTask(UserTask task)
        {
            if (task != null)
            {
                CurrTask = task;
            }
        }

        public async void AddAttachmentToTask(StorageFile file)
        {
            Attachment attachment = new Attachment();
            attachment.Name = file.Name;
            attachment.Path = GetRandomFileName(Path.GetExtension(file.Path));
            attachment.Type = file.ContentType;
            attachment.TaskId = CurrTask.Id;
            attachment.Content = file;
            Attachments.Add(attachment);
            if(AdditionAllowedFromUI)
            {
                await AddAttachmentToLocalFolder(attachment);
                attachment = attachmentDataHandler.AddAttachmentsToTask(attachment);
                ViewmodelEventHandler.Instance.Publish(new AddAttachmentEvent() { Task = CurrTask, Attachment = attachment });
            }
            else
            {
                CurrTask.Attachments.Add(attachment);
                NotifyPropertyChanged(nameof(CurrTask));
            }
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
            Attachments.Remove(attachment);
            NotifyPropertyChanged(nameof(Attachments));
        }

        public void ClearAttachmentList()
        {
            Attachments.Clear();
            NotifyPropertyChanged(nameof(Attachments));
        }

        public void OnAttachmentAddition(AddAttachmentEvent addAttachmentEvent)
        {
            if (addAttachmentEvent != null && addAttachmentEvent.Task.Id == CurrTask.Id)
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
