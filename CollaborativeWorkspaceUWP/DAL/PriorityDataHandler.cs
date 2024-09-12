using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Models.Enums;
using CollaborativeWorkspaceUWP.Persistence.PersistenceObject.EntityPersistence;
using CollaborativeWorkspaceUWP.Utilities.Persistence;
using CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject;
using CollaborativeWorkspaceUWP.Views.ViewObjects.Boards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace CollaborativeWorkspaceUWP.DAL
{
    public class PriorityDataHandler
    {
        PersistenceObjectManager _persistenceObjectManager;

        public PriorityDataHandler()
        {
            _persistenceObjectManager = new PersistenceObjectManager(PersistenceMode.SQLITE);
        }

        public List<Priority> GetPriorityData()
        {
            List<Priority> result = new List<Priority>();
            IPriorityPersistence persistenceObject = null;
            try
            {
                persistenceObject = _persistenceObjectManager.GetPriorityPersistenceObject();
                persistenceObject.SetGetPrioritiesContext();
                PersistenceHandler.Instance.Get(persistenceObject);
                result = persistenceObject.GetPriorities();
            }
            catch(Exception ex)
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
