using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject;
using CollaborativeWorkspaceUWP.Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject.EntityPersistence;
using CollaborativeWorkspaceUWP.Utilities.Persistence;

namespace CollaborativeWorkspaceUWP.DAL
{
    public class OrganizationDataHandler
    {
        private PersistenceObjectManager persistanceObjectManager;

        public OrganizationDataHandler()
        {
            persistanceObjectManager = new PersistenceObjectManager(PersistenceMode.SQLITE);
        }

        public Organization AddOrganization(Organization organization)
        {
            Organization result = null;
            IOrganizationPersistence persistenceObject = null;
            try
            {
                persistenceObject = persistanceObjectManager.GetOrganizationPersistenceObject();
                persistenceObject.SetAddOrganizationContext(organization);
                PersistenceHandler.Instance.Get(persistenceObject);
                result = persistenceObject.GetOrganization();
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

        public ObservableCollection<Organization> GetAllOrganizations()
        {
            ObservableCollection<Organization> organizations = new ObservableCollection<Organization>();
            IOrganizationPersistence persistenceObject = null;
            try
            {
                persistenceObject = persistanceObjectManager.GetOrganizationPersistenceObject();
                persistenceObject.SetGetAllOrganizationsContext();
                PersistenceHandler.Instance.Get(persistenceObject);
                organizations = persistenceObject.GetAllOrganizations();
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
            return organizations;
        }

    }
}
