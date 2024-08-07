using CollaborativeWorkspaceUWP.Auth.Providers;
using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Auth.Managers
{
    public class WindowsAuthManager : IAuthManager
    {
        private IAuthProvider authProvider;

        private AuthProviderFactory authProviderFactory;

        public WindowsAuthManager(AuthProviderMode mode)
        {
            authProviderFactory = new AuthProviderFactory();
            authProvider = authProviderFactory.GetAuthProvider(mode);
        }

        public User Login(User user)
        {
            return authProvider.Login(user);
        }

        public User Signup(User user)
        {
            return authProvider.Signup(user);
        }

        public bool DoesUserExist(string username)
        {
            return authProvider.DoesUserExist(username);
        }
    }
}
