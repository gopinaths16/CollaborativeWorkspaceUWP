using CollaborativeWorkspaceUWP.Auth.Providers;
using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;

namespace CollaborativeWorkspaceUWP.Auth.Managers
{
    public class WindowsAuthManager : IAuthManager
    {
        private string resourceName = "CWP";
        private User user;

        private IAuthProvider authProvider;

        private AuthProviderFactory authProviderFactory;

        public WindowsAuthManager(AuthProviderMode mode)
        {
            authProviderFactory = new AuthProviderFactory();
            authProvider = authProviderFactory.GetAuthProvider(mode);
        }

        public void Login(User user)
        {
            this.user = authProvider.Login(user);
            if (IsAuthenticated())
            {
                AddCredentials(user);
            }
        }

        public void Signup(User user)
        {
            this.user = authProvider.Signup(user);
            if(IsAuthenticated())
            {
                AddCredentials(user);
            }
        }

        public bool DoesUserExist(User user)
        {
            return authProvider.DoesUserExist(user.Username);
        }

        public void AddCredentials(User user)
        {
            try
            {
                var vault = new PasswordVault();
                vault.Add(new PasswordCredential(resourceName, user.Username, user.Password));
            }
            catch (Exception ex)
            {

            }
        }

        public PasswordCredential GetCredentials()
        {
            PasswordCredential credential = null;
            var vault = new PasswordVault();
            IReadOnlyList<PasswordCredential> credentialList = null;
            try
            {
                credentialList = vault.FindAllByResource(resourceName);
            }
            catch (Exception)
            {
                return null;
            }

            if (credentialList.Count > 0)
            {
                credential = credentialList[0];
            }
            return credential;
        }

        public void RemoveCredentials(User user)
        {
            var vault = new PasswordVault();
            vault.Remove(new PasswordCredential(resourceName, user.Username, user.Password));
        }

        public bool IsAuthenticated()
        {
            if(user != null)
            {
                return true;
            }
            else 
            {
                PasswordCredential credential = GetCredentials();
                if (credential != null)
                {
                    credential.RetrievePassword();
                    User temp = new User() { Username = credential.UserName, Password  = credential.Password };
                    user = authProvider.Login(temp);
                }
            }
            return user != null;
        }

        public User GetAuthenticatedUser()
        {
            return user;
        }
    }
}
