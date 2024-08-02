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

        private AttachmentDataHandler attachmentDataHandler;

        public AttachmentItemViewModel()
        {
            attachmentDataHandler = new AttachmentDataHandler();
        }

        public async Task DeleteAttachment(Attachment attachment)
        {
            attachmentDataHandler.DeleteAttachment(attachment.Id);
            StorageFolder storageFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Attachments");
            StorageFile file = await storageFolder.GetFileAsync(attachment.Path);
            if (file != null)
            {
                await file.DeleteAsync();
            }
        }

    }
}
