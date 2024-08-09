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
        UserDataHandler userDataHandler;

        public Comment Comment { get; set; }

        public CommentItemViewModel()
        {
            attachmentDataHandler = new AttachmentDataHandler();
            userDataHandler = new UserDataHandler();
        }

        public Comment SetComment(Comment comment)
        {
            Comment = comment;
            if(comment != null)
            {
                comment.Attachments = attachmentDataHandler.GetAllAttachmentsForComment(comment.Id);
                if(comment.Owner == null)
                {
                    comment.Owner = userDataHandler.GetUser(comment.OwnerId);
                }
            }
            NotifyPropertyChanged(nameof(Comment));
            return Comment;
        }
    }
}
