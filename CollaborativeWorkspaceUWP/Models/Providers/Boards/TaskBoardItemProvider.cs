using CollaborativeWorkspaceUWP.DAL;
using CollaborativeWorkspaceUWP.Views.ViewObjects.Boards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Models.Providers.Boards
{
    public class TaskBoardItemProvider : IBoardItemProvider
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

        public TaskBoardItemProvider()
        {
            taskDataHandler = new TaskDataHandler();

            DefaultArgs = new List<IDefaultArgs>();
            IsDefaultProvider = false;
        }

        public ICollection<IBoardItem> GetBoardItems(long boardId, long projectId)
        {
            return taskDataHandler.GetTasksForBoard(boardId);
        }

        public bool DoesItemBelongToBoard(IBoardItem item)
        {
            if (item != null && (item as UserTask).GroupId == BoardId)
            {
                return true;
            }
            return false;
        }

        public ICollection<IDefaultArgs> GetDefaultArgs()
        {
            return DefaultArgs;
        }
    }
}
