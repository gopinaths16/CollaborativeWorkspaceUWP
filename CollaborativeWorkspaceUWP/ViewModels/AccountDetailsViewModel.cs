using CollaborativeWorkspaceUWP.Auth.Handlers;
using CollaborativeWorkspaceUWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.ViewModels
{
    public class AccountDetailsViewModel : BaseViewModel
    {

        public User CurrUser
        {
            get { return UserSessionHandler.Instance.CurrUser; }
        }

        public AccountDetailsViewModel()
        {

        }
    }
}
