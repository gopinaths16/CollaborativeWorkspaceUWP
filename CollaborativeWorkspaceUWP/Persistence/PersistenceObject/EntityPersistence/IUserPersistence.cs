using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Persistence.PersistenceObject.EntityPersistence
{
    public interface IUserPersistence : IPersistenceObject
    {
        void SetAddUserContext(User user);
        void SetGetUserContext(string username, string password);
        void SetGetUserContext(string username);
        void SetGetUserContext(long userId);
        User GetUser();
    }
}
