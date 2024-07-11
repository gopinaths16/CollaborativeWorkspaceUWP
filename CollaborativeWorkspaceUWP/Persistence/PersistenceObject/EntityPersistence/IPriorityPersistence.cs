using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Persistence.PersistenceObject.EntityPersistence
{
    public interface IPriorityPersistence : IPersistenceObject
    {
        void SetGetPrioritiesContext();
        List<Priority> GetPriorities(); 
    }
}
