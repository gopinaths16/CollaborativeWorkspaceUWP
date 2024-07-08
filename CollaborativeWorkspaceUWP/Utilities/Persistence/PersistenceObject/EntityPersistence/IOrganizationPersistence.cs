using CollaborativeWorkspaceUWP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject.EntityPersistence
{
    public interface IOrganizationPersistence : IPersistenceObject
    {
        void SetAddOrganizationContext(Organization organization);
        void SetGetAllOrganizationsContext();
        Organization GetOrganization();
        ObservableCollection<Organization> GetAllOrganizations();
    }
}
