using CollaborativeWorkspaceUWP.Models.Enums;
using CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.DAL
{
    public class BoardDataHandler
    {
        private PersistenceObjectManager persistanceObjectManager;

        public BoardDataHandler()
        {
            persistanceObjectManager = new PersistenceObjectManager(PersistenceMode.SQLITE);
        }
    }
}
