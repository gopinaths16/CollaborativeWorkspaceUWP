using CollaborativeWorkspaceUWP.DAL;
using CollaborativeWorkspaceUWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace CollaborativeWorkspaceUWP.ViewModels
{
    public class AttachmentItemViewModel : BaseViewModel
    {
        public Attachment Attachment { get; set; }

        private AttachmentDataHandler attachmentDataHandler;

        public AttachmentItemViewModel()
        {
            attachmentDataHandler = new AttachmentDataHandler();
        }

        public void SetAttachment(Attachment attachment)
        {
            Attachment = attachment;
            NotifyPropertyChanged(nameof(Attachment));
        }

        public async Task DeleteAttachment()
        {
            attachmentDataHandler.DeleteAttachment(Attachment.Id);
            StorageFolder storageFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Attachments");
            StorageFile file = await storageFolder.GetFileAsync(Attachment.Path);
            if (file != null)
            {
                await file.DeleteAsync();
            }
        }

    }
}
