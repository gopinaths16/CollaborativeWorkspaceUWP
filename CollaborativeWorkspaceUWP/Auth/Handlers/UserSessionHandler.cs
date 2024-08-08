using CollaborativeWorkspaceUWP.Auth.Managers;
using CollaborativeWorkspaceUWP.Utilities;
using CollaborativeWorkspaceUWP.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollaborativeWorkspaceUWP.Models;

namespace CollaborativeWorkspaceUWP.Auth.Handlers
{
    public class UserSessionHandler
    {
        private static readonly Lazy<UserSessionHandler> _instance = new Lazy<UserSessionHandler>(() => new UserSessionHandler());
        private IAuthManager authManager;

        private AuthManagerFactory authManagerFactory;

        public static UserSessionHandler Instance => _instance.Value;

        private UserSessionHandler()
        {
            authManagerFactory = new AuthManagerFactory();
            authManager = authManagerFactory.GetAuthManager(AuthManagerMode.WINDOWS, AuthProviderMode.LOCAL);
        }

        public bool IsAuthenticated
        {
            get { return authManager.IsAuthenticated(); }
        }

        public User CurrUser
        {
            get { return authManager.GetAuthenticatedUser(); }
        }

        public bool Login(User args)
        {
            authManager.Login(args);
            return IsAuthenticated;
        }

        public bool Signup(User args)
        {
            if(authManager.DoesUserExist(args))
            {
                return false;
            }
            authManager.Signup(args);
            return IsAuthenticated;
        }
    }
}
