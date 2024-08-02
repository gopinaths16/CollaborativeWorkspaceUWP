using CollaborativeWorkspaceUWP.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Auth.Providers
{
    public class AuthProviderFactory
    {
        public IAuthProvider GetAuthProvider(AuthProviderMode mode)
        {
            IAuthProvider authProvider = null;
            switch(mode)
            {
                case AuthProviderMode.LOCAL:
                    authProvider = new LocalAuthProvider();
                    break;

                default:
                    authProvider = new LocalAuthProvider();
                    break;
            }
            return authProvider;
        }
    }
}
