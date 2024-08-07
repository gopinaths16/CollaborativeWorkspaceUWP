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
        private static User user;
        private IAuthManager authManager;

        private AuthManagerFactory authManagerFactory;

        public static UserSessionHandler Instance => _instance.Value;

        private UserSessionHandler()
        {
            authManagerFactory = new AuthManagerFactory();
            authManager = authManagerFactory.GetAuthManager(AuthManagerMode.WINDOWS, AuthProviderMode.LOCAL);
        }

        public bool IsAuthenticated => user != null;

        public bool Login(User args)
        {
            User temp = authManager.Login(args);
            if(temp != null)
            {
                user = temp;
                return true;
            }
            return false;
        }

        public bool Signup(User args)
        {
            if (!authManager.DoesUserExist(args.Username))
            {
                User temp = authManager.Signup(args);
                if (temp != null)
                {
                    user = temp;
                    return true;
                }
            }
            return false;
        }
    }
}
