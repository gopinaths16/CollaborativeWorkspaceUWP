using CollaborativeWorkspaceUWP.Persistence.PersistenceObject.EntityPersistence;
using CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Persistence.PersistenceObject.DBPersistence
{
    public class AttachmentDBPersistenceObject : DBPersistenceObject, IAttachmentPersistence
    {

        public void SetAddAttachmentContext()
        {
            SQLiteCommand command = new SQLiteCommand();
            Query = command;
        }

    }
}
