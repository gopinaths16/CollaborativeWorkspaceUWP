using CollaborativeWorkspaceUWP.DAL;
using CollaborativeWorkspaceUWP.Views.ViewObjects.Boards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Models.Providers.Boards
{
    public class TaskOnStatusBoardItemProvider : IBoardItemProvider
    {
        TaskDataHandler taskDataHandler;

        public long BoardId
        {
            get; set;
        }

        public bool IsDefaultProvider
        {
            get; private set;
        }

        public ICollection<IDefaultArgs> DefaultArgs
        {
            get; private set;
        }

        public TaskOnStatusBoardItemProvider()
        {
            taskDataHandler = new TaskDataHandler();

            IsDefaultProvider = true;
            DefaultArgs = new List<IDefaultArgs>();
        }

        public ICollection<IBoardItem> GetBoardItems(long boardId, long projectId)
        {
            return taskDataHandler.GetAllTasksForStatusBoard(boardId, projectId);
        }

        public bool DoesItemBelongToBoard(IBoardItem item)
        {
            if (item != null && (item as UserTask).Status == BoardId)
            {
                return true;
            }
            return false;
        }

        public ICollection<IDefaultArgs> GetDefaultArgs()
        {
            return DefaultArgs;
        }

        public ICollection<IBoardItem> UpdateBoardItems(ICollection<IBoardItem> boardItems)
        {
            ICollection<IBoardItem> result = new List<IBoardItem>();
            foreach (UserTask item in boardItems)
            {
                item.Status = BoardId;
                taskDataHandler.UpdateTask(item);
                item.IsCompleted = item.Status == 3;
                result.Add(item);
            }
            return boardItems;
        }
    }
}
