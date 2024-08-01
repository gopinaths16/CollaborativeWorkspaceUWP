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
    public interface IAttachmentPersistence : IPersistenceObject
    {
        void SetAddAttachmentContext(Attachment attachment);
        void SetGetAllAttachmentsForTaskContext(long taskId);
        void SetGetAllAttachmentsForCommentContext(long commentId);
        void SetDeleteAttachmentForCommentContext(long commentId);
        void SetDeleteAttachmentContext(long id);
        ObservableCollection<Attachment> GetAllAttachments();
        Attachment GetAttachment();
    }
}
