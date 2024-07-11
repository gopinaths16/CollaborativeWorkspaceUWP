using CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject;
using CollaborativeWorkspaceUWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Persistence.PersistenceObject.EntityPersistence
{
    public interface IStatusPersistence : IPersistenceObject
    {
        void SetGetAllStatusContext();
        List<Status> GetAllStatus();
    }
}
