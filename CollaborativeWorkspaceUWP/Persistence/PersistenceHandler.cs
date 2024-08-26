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
    public sealed class PersistenceHandler
    {
        IPersistenceManager manager;
        private static PersistenceHandler instance;
        private static object locker = new object();

        private PersistenceHandler() { }

        public static PersistenceHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new PersistenceHandler();
                        }
                    }
                }
                return instance;
            }
        }

        public void Initialize(PersistenceMode mode)
        {
            manager = new DBPersistenceManager();
            manager.Initialize(mode);
        }

        public void Add(IPersistenceObject persistenceObject)
        {
            manager.Add(persistenceObject);
        }

        public void Get(IPersistenceObject persistenceObject)
        {
            manager.Get(persistenceObject);
        }

        public void Update(IPersistenceObject persistenceObject)
        {
            manager.Update(persistenceObject);
        }

        public void Delete(IPersistenceObject persistenceObject)
        {
            manager.Delete(persistenceObject);
        }

        public void PerformTransaction(List<IPersistenceObject> persistenceObjects)
        {
            manager.PerformTransaction(persistenceObjects);
        }
    }
}
