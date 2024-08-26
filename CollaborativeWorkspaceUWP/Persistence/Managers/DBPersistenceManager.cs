using CollaborativeWorkspaceUWP.Models.Enums;
using CollaborativeWorkspaceUWP.Utilities.Persistence.Managers;
using CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Utilities.Persistence
{
    public sealed class DBPersistenceManager : IPersistenceManager
    {
        private IPersistenceHandler handler;

        public DBPersistenceManager() {}

        public async void Initialize(PersistenceMode persistenceMode)
        {
            switch(persistenceMode)
            {
                case PersistenceMode.SQLITE:
                default:
                    handler = new SqliteDBPersistenceHandler();
                    break;
            }
            await handler.Initialize();
        }

        public void Add(IPersistenceObject persistenceObject)
        {
            handler.Add(persistenceObject);
        }

        public void Get(IPersistenceObject persistenceObject)
        {
            handler.Get(persistenceObject);
        }

        public void Update(IPersistenceObject persistenceObject)
        {
            handler.Update(persistenceObject);
        }

        public void Delete(IPersistenceObject persistenceObject)
        {
            handler.Delete(persistenceObject);
        }

        public void PerformTransaction(List<IPersistenceObject> persistenceObjects)
        {
            handler.PerformTransaction(persistenceObjects);
        }
    }
}
