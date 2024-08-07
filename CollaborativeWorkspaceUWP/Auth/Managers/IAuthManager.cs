using CollaborativeWorkspaceUWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Documents;

namespace CollaborativeWorkspaceUWP.Auth.Managers
{
    public interface IAuthManager
    {
        User Login(User user);
        User Signup(User user);
        bool DoesUserExist(string username);
    }
}
