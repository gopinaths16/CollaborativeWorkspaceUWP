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
        private ObservableCollection<Teamspace> teamspaces;

        private bool addOrganizationTriggered;
        private Organization currOrganization;

        private OrganizationDataHandler organizationDataHandler;
        private TeamspaceDataHandler teamspaceDataHandler;

        public ObservableCollection<Organization> Organizations
        {
            get { return organizations; }
            set { organizations = value; }
        }

        public ObservableCollection<Teamspace> TeamspacesForCurrOrganization
        {
            get { return teamspaces; }
            set { teamspaces = value; }
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
            teamspaceDataHandler = new TeamspaceDataHandler();

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
            TeamspacesForCurrOrganization = teamspaceDataHandler.GetAllTeamspacesForCurrOrganization(organization.Id);
            NotifyPropertyChanged(nameof(TeamspacesForCurrOrganization));
            NotifyPropertyChanged(nameof(CurrOrganization));
        }

        public void CreateTeamspaceInCurrentOrganization(Teamspace teamspace)
        {
            teamspace = teamspaceDataHandler.AddTeamspace(teamspace);
            TeamspacesForCurrOrganization.Add(teamspace);
            NotifyPropertyChanged(nameof(TeamspacesForCurrOrganization));
        }
    }
}
