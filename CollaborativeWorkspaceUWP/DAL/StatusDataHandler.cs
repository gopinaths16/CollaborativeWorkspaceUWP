using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Models.Enums;
using CollaborativeWorkspaceUWP.Persistence.PersistenceObject.EntityPersistence;
using CollaborativeWorkspaceUWP.Utilities.Persistence;
using CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.DAL
{
    public class StatusDataHandler
    {
        PersistenceObjectManager persistenceObjectManager;

        public StatusDataHandler()
        {
            persistenceObjectManager = new PersistenceObjectManager(PersistenceMode.SQLITE);
        }

        public List<Status> GetStatusData()
        {
            List<Status> result = new List<Status>();
            IStatusPersistence persistenceObject = null;
            try
            {
                persistenceObject = persistenceObjectManager.GetStatusPersistenceObject();
                persistenceObject.SetGetAllStatusContext();
                PersistenceHandler.Instance.Get(persistenceObject);
                result = persistenceObject.GetAllStatus();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if(persistenceObject != null)
                {
                    persistenceObject.Dispose();
                }
            }
            return result;
        }
    }
}
