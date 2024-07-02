using CollaborativeWorkspaceUWP.Models.Enums;
using CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject.EntityPersistence;
using CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject.EntityPersistence.Entity;
using CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject.EntityPersistence.Providers;
using CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject.EntityPersistence.Providers.Imp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject
{
    public class PersistanceObjectManager
    {
        private IPersistenceObjectProvider _provider;

        public PersistanceObjectManager(PersistenceMode mode)
        {
            switch (mode)
            {
                case PersistenceMode.SQLITE:
                default:
                    _provider = new DBPersistenceObjectProvider();
                    break;
            }
        }

        public ITaskPersistence GetTaskPersistenceObject()
        {
            return _provider.GetTaskPersistenceObject();
        }

        public IProjectPersistence GetProjectPersistenceObject()
        {
            return _provider.GetProjectPersistenceObject();
        }

    }
}
