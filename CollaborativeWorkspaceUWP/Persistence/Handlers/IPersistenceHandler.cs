using CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Utilities.Persistence
{
    public interface IPersistenceHandler
    {
        Task Initialize();

        void Add(IPersistenceObject persistenceObject);
        void Get(IPersistenceObject persistenceObject);
        void Update(IPersistenceObject persistenceObject);
        void Delete(IPersistenceObject persistenceObject);

    }
}
