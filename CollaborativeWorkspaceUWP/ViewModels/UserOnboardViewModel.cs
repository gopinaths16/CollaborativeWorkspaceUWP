using CollaborativeWorkspaceUWP.Auth.Handlers;
using CollaborativeWorkspaceUWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.ViewModels
{
    public class UserOnboardViewModel : BaseViewModel
    {
        private bool isLoginContext;

        public User User { get; set; }

        public bool IsLoginContext
        {
            get { return isLoginContext; }
            set { 
                isLoginContext = value;
                NotifyPropertyChanged(nameof(IsLoginContext));
            }
        }

        public UserOnboardViewModel()
        {
            IsLoginContext = true;
        }

        public bool Login(User user)
        {
            return UserSessionHandler.Instance.Login(user);
        } 

        public bool Signup(User user)
        {
            return UserSessionHandler.Instance.Signup(user);
        }

    }
}
