using CollaborativeWorkspaceUWP.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.ViewModels
{
    public class OrganizationListViewModel : BaseViewModel
    {
        private List<Organization> _organizations;
        private Dictionary<long, List<Teamspace>> _orgTeamspaceMap;
        private Dictionary<long, List<Project>> _teamspaceProjectMap;

        public OrganizationListViewModel()
        {
            _organizations = new List<Organization>();
            _orgTeamspaceMap = new Dictionary<long, List<Teamspace>>();
            _teamspaceProjectMap = new Dictionary<long, List<Project>>();
        }

        public void AddOrganization(Organization organization)
        {
            _organizations.Add(organization);
        }

    }
}
