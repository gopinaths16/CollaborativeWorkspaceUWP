using CollaborativeWorkspaceUWP.Auth.Managers;
using CollaborativeWorkspaceUWP.DAL;
using CollaborativeWorkspaceUWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Auth.Providers
{
    public class LocalAuthProvider : IAuthProvider
    {
        private UserDataHandler userDataHandler;

        public LocalAuthProvider()
        {
            userDataHandler = new UserDataHandler();
        }

        public User Login(User user)
        {
            User result = null;
            result = userDataHandler.GetUser(user.Username, user.Password);
            return result;
        }

        public User Signup(User user)
        {
            User result = null;
            result = userDataHandler.AddUser(user);
            return result;
        }

        public bool DoesUserExist(string username)
        {
            User user = null;
            user = userDataHandler.GetUser(username, null);
            return user != null;
        }
    }
}
