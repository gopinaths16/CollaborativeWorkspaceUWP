using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Persistence.PersistenceObject.DBPersistence;
using CollaborativeWorkspaceUWP.Persistence.PersistenceObject.EntityPersistence;
using CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject.EntityPersistence.DBPersistence;
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

        public IOrganizationPersistence GetOrganizationPersistenceObject()
        {
            return new OrganizationDBPersistenceObject();
        }

        public ITeamspacePersistence GetTeamspacePersistenceObject()
        {
            return new TeamspaceDBPersistenceObject();
        }

        public IPriorityPersistence GetPriorityPersistenceObject()
        {
            return new PriorityDBPersistenceObject();
        }

        public IStatusPersistence GetStatusPersistenceObject()
        {
            return new StatusDBPersistenceObject();
        }

        public IAttachmentPersistence GetAttachmentPersistenceObject()
        {
            return new AttachmentDBPersistenceObject();
        }

        public ICommentPersistence GetCommentPersistenceObject()
        {
            return new CommentDBPersistenceObject();
        }
    }
}
