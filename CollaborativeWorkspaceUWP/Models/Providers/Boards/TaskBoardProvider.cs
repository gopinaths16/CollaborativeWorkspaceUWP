using CollaborativeWorkspaceUWP.DAL;
using CollaborativeWorkspaceUWP.Utilities.Events;
using CollaborativeWorkspaceUWP.Utilities;
using CollaborativeWorkspaceUWP.Views.ViewObjects.Boards;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollaborativeWorkspaceUWP.ViewModels;

namespace CollaborativeWorkspaceUWP.Models.Providers.Boards
{
    public class TaskBoardProvider : BoardProvider
    {
        private GroupDataHandler groupDataHandler;
        private TaskDataHandler taskDataHandler;

        public TaskBoardProvider()
        {
            groupDataHandler = new GroupDataHandler();
            taskDataHandler = new TaskDataHandler();
        }

        public override ICollection<IBoard> GetBoards()
        {
            ICollection<IBoard> boards = groupDataHandler.GetBoardsForBoardGroup(BoardGroupId);
            foreach (IBoard board in boards)
            {
                TaskBoardItemProvider boardItemProvider = new TaskBoardItemProvider() { BoardId = board.Id };
                board.SetBoardItemProvider(boardItemProvider);
            }
            return boards;
        }
    }
}
