using CollaborativeWorkspaceUWP.Models.Enums;
using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollaborativeWorkspaceUWP.Persistence.PersistenceObject.EntityPersistence;
using CollaborativeWorkspaceUWP.Utilities.Persistence;

namespace CollaborativeWorkspaceUWP.DAL
{
    internal class AttachmentDataHandler
    {
        private PersistenceObjectManager persistanceObjectManager;

        public AttachmentDataHandler()
        {
            persistanceObjectManager = new PersistenceObjectManager(PersistenceMode.SQLITE);
        }

        public Attachment AddAttachmentsToTask(Attachment attachment)
        {
            Attachment result = null;
            IAttachmentPersistence persistenceObject = null;
            try
            {
                persistenceObject = persistanceObjectManager.GetAttachmentPersistenceObject();
                persistenceObject.SetAddAttachmentContext(attachment);
                PersistenceHandler.Instance.Get(persistenceObject);
                result = persistenceObject.GetAttachment();
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

        public ObservableCollection<Attachment> GetAllAttachmentsForTask(long taskId)
        {
            ObservableCollection<Attachment> result = new ObservableCollection<Attachment> ();
            IAttachmentPersistence persistentObject = null;
            try
            {
                persistentObject = persistanceObjectManager.GetAttachmentPersistenceObject();
                persistentObject.SetGetAllAttachmentsForTaskContext(taskId);
                PersistenceHandler.Instance.Get(persistentObject);
                result = persistentObject.GetAllAttachments();
            }
            catch(Exception ex)
            {

            }
            finally
            {
                if (persistentObject != null)
                {
                    persistentObject.Dispose();
                }
            }
            return result;
        }

        public ObservableCollection<Attachment> GetAllAttachmentsForComment(long commentId)
        {
            ObservableCollection<Attachment> result = new ObservableCollection<Attachment>();
            IAttachmentPersistence persistentObject = null;
            try
            {
                persistentObject = persistanceObjectManager.GetAttachmentPersistenceObject();
                persistentObject.SetGetAllAttachmentsForCommentContext(commentId);
                PersistenceHandler.Instance.Get(persistentObject);
                result = persistentObject.GetAllAttachments();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (persistentObject != null)
                {
                    persistentObject.Dispose();
                }
            }
            return result;
        }
    }
}
