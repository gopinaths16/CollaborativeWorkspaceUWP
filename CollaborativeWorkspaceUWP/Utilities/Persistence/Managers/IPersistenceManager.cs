using CollaborativeWorkspaceUWP.Models.Enums;
using CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Utilities.Persistence.Managers
{
    public interface IPersistenceManager
    {
        void Initialize(PersistenceMode persistenceMode);
        void Add(IPersistenceObject persistenceObject);
        void Get(IPersistenceObject persistenceObject);
        void Update(IPersistenceObject persistenceObject);
        void Delete(IPersistenceObject persistenceObject);

    }
}
