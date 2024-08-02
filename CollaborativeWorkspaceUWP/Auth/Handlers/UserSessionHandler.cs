using CollaborativeWorkspaceUWP.Auth.Managers;
using CollaborativeWorkspaceUWP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Auth.Handlers
{
    public class UserSessionHandler
    {
        private static readonly Lazy<UserSessionHandler> _instance = new Lazy<UserSessionHandler>(() => new UserSessionHandler());
        private IAuthManager authManager;

        public static UserSessionHandler Instance => _instance.Value;

        private UserSessionHandler() { }
    }
}
