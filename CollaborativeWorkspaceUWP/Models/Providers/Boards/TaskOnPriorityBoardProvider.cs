using CollaborativeWorkspaceUWP.DAL;
using CollaborativeWorkspaceUWP.Views.ViewObjects.Boards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Models.Providers.Boards
{
    public class TaskOnPriorityBoardProvider : BoardProvider
    {
        private PriorityDataHandler priorityDataHandler;

        public TaskOnPriorityBoardProvider()
        {
            priorityDataHandler = new PriorityDataHandler();
        }

        public override ICollection<IBoard> GetBoards()
        {
            ICollection<IBoard> boards = new List<IBoard>();
            foreach(Priority priority in priorityDataHandler.GetPriorityBoards())
            {
                boards.Add(new Group() { Id = priority.Id, Name = priority.Name, ProjectId = ProjectId, ColorCode = priority.ColorCode, IsBoardGroup = false, BoardItemProvider = new TaskOnPriorityBoardItemProvider() });
            }
            return boards;
        } 
    }
}
