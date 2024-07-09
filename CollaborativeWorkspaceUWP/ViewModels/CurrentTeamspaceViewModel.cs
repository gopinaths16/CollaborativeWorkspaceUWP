using CollaborativeWorkspaceUWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.ViewModels
{
    public class CurrentTeamspaceViewModel : BaseViewModel
    {
        private Teamspace currTeamspace;
        
        public Teamspace CurrTeamspace
        {
            get { return currTeamspace; }
            set { currTeamspace = value; }
        }

        public CurrentTeamspaceViewModel() { }

    }
}
