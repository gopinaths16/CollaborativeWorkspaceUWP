using CollaborativeWorkspaceUWP.DAL;
using CollaborativeWorkspaceUWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.ViewModels
{
    public class CommentItemViewModel : BaseViewModel
    {
        AttachmentDataHandler attachmentDataHandler;

        public Comment Comment { get; set; }

        public CommentItemViewModel()
        {
            attachmentDataHandler = new AttachmentDataHandler();
        }

        public void SetComment(Comment comment)
        {
            Comment = comment;
            if(comment != null)
            {
                comment.Attachments = attachmentDataHandler.GetAllAttachmentsForComment(comment.Id);
            }
            NotifyPropertyChanged(nameof(Comment));
        }
    }
}
