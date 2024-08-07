using CollaborativeWorkspaceUWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Auth.Providers
{
    public interface IAuthProvider
    {
        User Login(User user);
        User Signup(User user);
        bool DoesUserExist(string username);
    }
}
