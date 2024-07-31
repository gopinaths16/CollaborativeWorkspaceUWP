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

namespace CollaborativeWorkspaceUWP.ViewModels
{
    public class CommentViewModel : BaseViewModel
    {
        private CommentDataHandler commentDataHandler;

        public UserTask CurrTask { get; set; }
        public Comment Comm { get; set; }
        public ObservableCollection<Comment> CommentsForCurrTask { get; set; }

        public CommentViewModel()
        {
            Comm = new Comment();
            ViewmodelEventHandler.Instance.Subscribe<AddCommentEvent>(OnCommentAddition);

            commentDataHandler = new CommentDataHandler();
        }

        public void SetCurrTask(UserTask currTask)
        {
            CurrTask = currTask;
            CommentsForCurrTask = CurrTask != null ? commentDataHandler.GetAllCommentsForCurrentTask(CurrTask.Id) : new ObservableCollection<Comment>();
            NotifyPropertyChanged(nameof(CommentsForCurrTask));
        }

        public Comment AddCommentToCurrTask()
        {
            Comm.TaskId = CurrTask.Id;
            Comment comment = commentDataHandler.AddComment(Comm);
            ViewmodelEventHandler.Instance.Publish(new AddCommentEvent() { Comment = Comm });
            Comm = new Comment();
            NotifyPropertyChanged(nameof(Comm));
            return comment;
        }

        public void OnCommentAddition(AddCommentEvent addCommentEvent)
        {
            CommentsForCurrTask.Add(addCommentEvent.Comment);
            NotifyPropertyChanged(nameof(CommentsForCurrTask));
        }
    }
}
