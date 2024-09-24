using CollaborativeWorkspaceUWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.ViewModels
{
    public class OrganizationListViewViewModel : BaseViewModel
    {
        private Organization currOrganization;
        private bool isAddOrgContextTriggered;

        public Organization CurrOrganization
        {
            get { return currOrganization; }
            set
            {
                currOrganization = value;
                NotifyPropertyChanged(nameof(CurrOrganization));
            }
        }

        public bool IsAddOrgContextTriggered
        {
            get { return isAddOrgContextTriggered; }
            set
            {
                isAddOrgContextTriggered = value;
                NotifyPropertyChanged(nameof(IsAddOrgContextTriggered));
            }
        }

        public OrganizationListViewViewModel()
        {
            IsAddOrgContextTriggered = false;
        }
    }
}
