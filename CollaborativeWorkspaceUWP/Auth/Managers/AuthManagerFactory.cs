using CollaborativeWorkspaceUWP.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Auth.Managers
{
    public class AuthManagerFactory
    {

        public IAuthManager GetAuthManager(AuthManagerMode mode, AuthProviderMode providerMode)
        {
            IAuthManager authManager = null;
            switch(mode)
            {
                case AuthManagerMode.WINDOWS:
                    authManager = new WindowsAuthManager(providerMode);
                    break;

                default:
                    authManager = new WindowsAuthManager(providerMode);
                    break;
            }
            return authManager;
        }

    }
}
