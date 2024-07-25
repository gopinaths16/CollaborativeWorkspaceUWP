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
    public class AddAttachmentViewModel : BaseViewModel
    {
        private ObservableCollection<Attachment> attachments;
        private AttachmentDataHandler attachmentDataHandler;

        public ObservableCollection<Attachment> Attachments
        {
            get { return attachments; }
            set { attachments = value; }
        }

        public UserTask CurrTask { get; set; }

        public AddAttachmentViewModel()
        {
            Attachments = new ObservableCollection<Attachment>();
            attachmentDataHandler = new AttachmentDataHandler();
        }

        public void SetCurrTask(UserTask task)
        {
            if (task != null)
            {
                CurrTask = task;
            }
        }

        public void AddAttachmentToList(StorageFile file)
        {
            Attachment attachment = new Attachment();
            attachment.Name = file.Name;
            attachment.Path = GetRandomFileName(Path.GetExtension(file.Path));
            attachment.Type = file.ContentType;
            attachment.TaskId = CurrTask.Id;
            attachment.Content = file;
            Attachments.Add(attachment);
            NotifyPropertyChanged(nameof(Attachments));
        }

        public async Task AddAttachmentsToTask()
        {
            await AddAttachmentToLocalFolder();
            attachmentDataHandler.AddAttachmentsToTask(Attachments);
            ViewmodelEventHandler.Instance.Publish(new AddAttachmentEvent() { Task = CurrTask, Attachments = Attachments });
        }

        public async Task AddAttachmentToLocalFolder()
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
                foreach (var attachment in Attachments)
                {
                    StorageFile file = await folder.CreateFileAsync(attachment.Path);
                    var content = await FileIO.ReadBufferAsync(attachment.Content);
                    await FileIO.WriteBufferAsync(file, content);
                }
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
    }
}
