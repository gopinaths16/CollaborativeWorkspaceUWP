using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Persistence.PersistenceObject.EntityPersistence
{
    public interface IBoardPersistence : IPersistenceObject
    {
        void SetGetAllBoardsForProjectContext(long projectId);
        ObservableCollection<Board> GetAllBoards();
    }
}
