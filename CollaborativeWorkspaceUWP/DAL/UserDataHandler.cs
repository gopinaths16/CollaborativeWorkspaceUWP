using CollaborativeWorkspaceUWP.Models.Enums;
using CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.DAL
{
    public class UserDataHandler
    {
        private PersistenceObjectManager persistanceObjectManager;

        public UserDataHandler()
        {
            persistanceObjectManager = new PersistenceObjectManager(PersistenceMode.SQLITE);
        }
    }
}
