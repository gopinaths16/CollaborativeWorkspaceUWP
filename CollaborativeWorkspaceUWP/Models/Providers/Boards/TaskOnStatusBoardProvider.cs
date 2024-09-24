using CollaborativeWorkspaceUWP.DAL;
using CollaborativeWorkspaceUWP.Views.ViewObjects.Boards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Models.Providers.Boards
{
    public class TaskOnStatusBoardProvider : BoardProvider
    {
        private StatusDataHandler statusDataHandler;

        public TaskOnStatusBoardProvider()
        {
            statusDataHandler = new StatusDataHandler();
        }

        public override ICollection<IBoard> GetBoards()
        {
            ICollection<IBoard> boards = new List<IBoard>();
            foreach (Status status in statusDataHandler.GetStatusData())
            {
                TaskOnStatusBoardItemProvider boardProvider = new TaskOnStatusBoardItemProvider() { BoardId = status.Id };
                boardProvider.DefaultArgs.Add(status);
                boards.Add(new Group() { Id = status.Id, Name = status.Name, ProjectId = ProjectId, ColorCode = status.ColorCode, IsBoardGroup = false, BoardItemProvider = boardProvider });
            }
            return boards;
        }

        public override IBoardItemProvider GetBoardItemProvider()
        {
            return new TaskOnStatusBoardItemProvider();
        }
    }
}
