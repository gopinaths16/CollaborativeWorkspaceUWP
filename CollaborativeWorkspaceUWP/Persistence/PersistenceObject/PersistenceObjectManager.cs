using CollaborativeWorkspaceUWP.Models.Enums;
using CollaborativeWorkspaceUWP.Persistence.PersistenceObject.EntityPersistence;
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
    public class PersistenceObjectManager
    {
        private IPersistenceObjectProvider _provider;

        public PersistenceObjectManager(PersistenceMode mode)
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

        public IOrganizationPersistence GetOrganizationPersistenceObject()
        {
            return _provider.GetOrganizationPersistenceObject();
        }

        public ITeamspacePersistence GetTeamspacePersistenceObject()
        {
            return _provider.GetTeamspacePersistenceObject();
        }

        public IPriorityPersistence GetPriorityPersistenceObject()
        {
            return _provider.GetPriorityPersistenceObject();
        }

        public IStatusPersistence GetStatusPersistenceObject()
        {
            return _provider.GetStatusPersistenceObject();
        }

        public IAttachmentPersistence GetAttachmentPersistenceObject()
        {
            return _provider.GetAttachmentPersistenceObject();
        }

        public ICommentPersistence GetCommentPersistenceObject()
        {
            return _provider.GetCommentPersistenceObject();
        }

        public IUserPersistence GetUserPersistenceObject()
        {
            return _provider.GetUserPersistenceObject();
        }
    }
}
