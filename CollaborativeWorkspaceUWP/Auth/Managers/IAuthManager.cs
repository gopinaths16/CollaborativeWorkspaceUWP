using CollaborativeWorkspaceUWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.UI.Xaml.Documents;

namespace CollaborativeWorkspaceUWP.Auth.Managers
{
    public interface IAuthManager
    {
        void Login(User user);
        void Signup(User user);
        void Logout();
        bool DoesUserExist(User user);
        PasswordCredential GetCredentials();
        void AddCredentials(User user);
        void RemoveCredentials();
        bool IsAuthenticated();
        User GetAuthenticatedUser();
    }
}
