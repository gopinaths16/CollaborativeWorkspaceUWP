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

        public TaskBoardItemProvider()
        {
            taskDataHandler = new TaskDataHandler();
        }

        public ICollection<IBoardItem> GetBoardItems(long boardId, long projectId)
        {
            return taskDataHandler.GetTasksForBoard(boardId);
        }
    }
}
