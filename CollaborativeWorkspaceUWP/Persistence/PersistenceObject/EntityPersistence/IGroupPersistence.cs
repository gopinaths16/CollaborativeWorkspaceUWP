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
    public interface IGroupPersistence : IPersistenceObject
    {
        void SetGetAllBoardGroupsForProjectContext(long projectId);
        void SetGetAllBoardsForBoardGroupContext(long boardId);
        void SetAddBoardGroupForProjectContext(string boardName, long projectId, long boardGroupId, bool isBoardGroup);
        ObservableCollection<Group> GetAllGroups();
        Group GetGroup();
    }
}
