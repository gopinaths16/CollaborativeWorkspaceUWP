using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Models.Enums;
using CollaborativeWorkspaceUWP.Persistence.PersistenceObject.EntityPersistence;
using CollaborativeWorkspaceUWP.Utilities.Persistence;
using CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.DAL
{
    public class CommentDataHandler
    {

        private PersistenceObjectManager persistanceObjectManager;

        public CommentDataHandler()
        {
            persistanceObjectManager = new PersistenceObjectManager(PersistenceMode.SQLITE);
        }

        public Comment AddComment(Comment comment)
        {
            Comment result = null;
            ICommentPersistence persistenceObject = null;
            try
            {
                persistenceObject = persistanceObjectManager.GetCommentPersistenceObject();
                persistenceObject.SetAddCommentContext(comment);
                PersistenceHandler.Instance.Get(persistenceObject);
                result = persistenceObject.GetComment();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (persistenceObject != null)
                {
                    persistenceObject.Dispose();
                }
            }
            return result;
        }

        public ObservableCollection<Comment> GetAllCommentsForCurrentTask(long taskId)
        {
            ObservableCollection<Comment> result = new ObservableCollection<Comment>();
            ICommentPersistence persistenceObject = null;
            try
            {
                persistenceObject = persistanceObjectManager.GetCommentPersistenceObject();
                persistenceObject.SetGetAllCommentsForCurrentTaskContext(taskId);
                PersistenceHandler.Instance.Get(persistenceObject);
                result = persistenceObject.GetComments();
            }
            catch(Exception ex)
            {

            }
            finally
            {
                if (persistenceObject != null)
                {
                    persistenceObject.Dispose();
                }
            }
            return result;
        }

        public void DeleteComment(long commentId)
        {
            ICommentPersistence persistenceObject = null;
            try
            {
                persistenceObject = persistanceObjectManager.GetCommentPersistenceObject();
                persistenceObject.SetDeleteCommentContext(commentId);
                PersistenceHandler.Instance.Get(persistenceObject);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (persistenceObject != null)
                {
                    persistenceObject.Dispose();
                }
            }
        }
    }
}
