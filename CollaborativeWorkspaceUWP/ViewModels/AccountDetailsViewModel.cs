using CollaborativeWorkspaceUWP.Auth.Handlers;
using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Utilities.Events;
using CollaborativeWorkspaceUWP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls.Primitives;

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

        public async Task Logout()
        {
            await ViewmodelEventHandler.Instance.Publish(new LogoutEvent());
        }

        public async Task ChangeTheme(string theme)
        {
            await ViewmodelEventHandler.Instance.Publish(new ThemeChangedEvent() { Theme = theme});
        }
    }
}
