using CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject.EntityPersistence.Entity;
using CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject.EntityPersistence.Entity.Imp.DBPersistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject.EntityPersistence.Providers.Imp
{
    public class DBPersistenceObjectProvider : IPersistenceObjectProvider
    {
        public DBPersistenceObjectProvider() { }

        public ITaskPersistence GetTaskPersistenceObject()
        {
            return new TaskDBPersistenceObject();
        }

        public IProjectPersistence GetProjectPersistenceObject()
        {
            return new ProjectDBPersistenceObject();
        }
    }
}
