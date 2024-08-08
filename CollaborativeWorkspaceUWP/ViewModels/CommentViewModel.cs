using CollaborativeWorkspaceUWP.Auth.Handlers;
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
using System.Xml.Linq;

namespace CollaborativeWorkspaceUWP.ViewModels
{
    public class CommentViewModel : BaseViewModel
    {
        private CommentDataHandler commentDataHandler;

        public UserTask CurrTask { get; set; }
        public Comment Comm { get; set; }
        public ObservableCollection<Comment> CommentsForCurrTask { 
            get; 
            set; 
        }

        public CommentViewModel()
        {
            Comm = new Comment();
            ViewmodelEventHandler.Instance.Subscribe<AddCommentEvent>(OnCommentAddition);

            commentDataHandler = new CommentDataHandler();
        }

        public void SetCurrTask(UserTask currTask)
        {
            CurrTask = currTask != null ? (UserTask)currTask.Clone() : null;
            CommentsForCurrTask = CurrTask != null ? commentDataHandler.GetAllCommentsForCurrentTask(CurrTask.Id) : new ObservableCollection<Comment>();
            NotifyPropertyChanged(nameof(CommentsForCurrTask));
        }

        public Comment AddCommentToCurrTask()
        {
            Comm.TaskId = CurrTask.Id;
            Comm.OwnerId = UserSessionHandler.Instance.CurrUser.Id;
            Comment comment = commentDataHandler.AddComment(Comm);
            comment.Attachments = Comm.Attachments;
            Comm = new Comment();
            NotifyPropertyChanged(nameof(Comm));
            return comment;
        }

        public async Task NotifyCommentAddition(Comment comment)
        {
            await ViewmodelEventHandler.Instance.Publish(new AddCommentEvent() { Comment = comment });
        }

        public void AddAttachmentsToComment(ObservableCollection<Attachment> attachments)
        {
            Comm.Attachments = attachments;
        }

        public async Task OnCommentAddition(AddCommentEvent addCommentEvent)
        {
            CommentsForCurrTask.Add(addCommentEvent.Comment);
            NotifyPropertyChanged(nameof(CommentsForCurrTask));
        }
    }
}
