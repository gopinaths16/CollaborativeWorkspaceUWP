using CollaborativeWorkspaceUWP.DAL;
using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Utilities;
using CollaborativeWorkspaceUWP.Utilities.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            attachment.Path = file.Path;
            attachment.Type = file.ContentType;
            attachment.TaskId = CurrTask.Id;
            Attachments.Add(attachment);
            NotifyPropertyChanged(nameof(Attachments));
        }

        public void AddAttachmentsToTask()
        {
            attachmentDataHandler.AddAttachmentsToTask(Attachments);
            ViewmodelEventHandler.Instance.Publish(new AddAttachmentEvent() { Task = CurrTask, Attachments = Attachments });
        }
    }
}
