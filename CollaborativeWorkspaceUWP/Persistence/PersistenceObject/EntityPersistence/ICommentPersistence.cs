using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Persistence.PersistenceObject.EntityPersistence
{
    public interface ICommentPersistence : IPersistenceObject
    {
        void SetAddCommentContext(Comment comment);
        void SetGetAllCommentsForCurrentTaskContext(long taskId);
        void SetDeleteCommentContext(long id);
        Comment GetComment();
        ObservableCollection<Comment> GetComments();
    }
}
