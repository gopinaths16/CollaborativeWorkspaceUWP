using CollaborativeWorkspaceUWP.Persistence.PersistenceObject.EntityPersistence;
using CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject.EntityPersistence.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject.EntityPersistence.Providers
{
    public interface IPersistenceObjectProvider
    {
        ITaskPersistence GetTaskPersistenceObject();
        IProjectPersistence GetProjectPersistenceObject();
        IOrganizationPersistence GetOrganizationPersistenceObject();
        ITeamspacePersistence GetTeamspacePersistenceObject();
        IPriorityPersistence GetPriorityPersistenceObject();
        IStatusPersistence GetStatusPersistenceObject();
        IAttachmentPersistence GetAttachmentPersistenceObject();
        ICommentPersistence GetCommentPersistenceObject();
        IUserPersistence GetUserPersistenceObject();
        IGroupPersistence GetGroupPersistenceObject();
    }
}
