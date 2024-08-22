using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Persistence.PersistenceObject.EntityPersistence;
using CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Persistence.PersistenceObject.DBPersistence
{
    public class BoardDBPersistenceObject : DBPersistenceObject, IBoardPersistence
    {
        public void SetGetAllBoardsForProjectContext(long projectId)
        {

        }

        public ObservableCollection<Board> GetAllBoards()
        {
            ObservableCollection<Board> boards = new ObservableCollection<Board>();
            return boards;
        }
    }
}
