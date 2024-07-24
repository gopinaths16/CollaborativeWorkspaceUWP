using CollaborativeWorkspaceUWP.Models.Enums;
using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.DAL
{
    internal class AttachmentDataHandler
    {
        private PersistenceObjectManager persistanceObjectManager;

        public AttachmentDataHandler()
        {
            persistanceObjectManager = new PersistenceObjectManager(PersistenceMode.SQLITE);
        }

        public ObservableCollection<Attachment> AddAttachmentsToTask(ObservableCollection<Attachment> attachments)
        {
            ObservableCollection<Attachment> result = null;
            return result;
        }
    }
}
