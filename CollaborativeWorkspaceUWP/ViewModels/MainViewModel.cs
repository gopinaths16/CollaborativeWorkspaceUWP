using CollaborativeWorkspaceUWP.DAL;
using CollaborativeWorkspaceUWP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CollaborativeWorkspaceUWP.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private ObservableCollection<Organization> organizations;
        private bool addOrganizationTriggered;
        private Organization currOrganization;
        private int selectedOrganizationIndex;

        private OrganizationDataHandler organizationDataHandler = null;

        public ObservableCollection<Organization> Organizations
        {
            get { return organizations; }
            set { organizations = value; }
        }

        public bool AddOrganizationTriggered
        {
            get { return addOrganizationTriggered; }
            set { addOrganizationTriggered = value; }
        }

        public Organization CurrOrganization
        {
            get { return currOrganization; }
            set { currOrganization = value; }
        }

        public MainViewModel()
        {
            organizationDataHandler = new OrganizationDataHandler();

            organizations = organizationDataHandler.GetAllOrganizations();
        }

        public void SetAddOrganizationContext()
        {
            AddOrganizationTriggered = true;
            NotifyPropertyChanged(nameof(AddOrganizationTriggered));
        }

        public void SetSelectOrganizationContext()
        {
            AddOrganizationTriggered = false;
            NotifyPropertyChanged(nameof(AddOrganizationTriggered));
        }

        public void AddNewOrganization(string orgName)
        {
            Organization organization = new Organization() { Name = orgName, OwnerId = 0 };
            organization = organizationDataHandler.AddOrganization(organization);
            Organizations.Add(organization);
            NotifyPropertyChanged(nameof(Organizations));
        }

        public void SetCurrOrganization(Organization organization)
        {
            CurrOrganization = organization;
            NotifyPropertyChanged(nameof(CurrOrganization));
        }
    }
}
