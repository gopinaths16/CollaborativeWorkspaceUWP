using CollaborativeWorkspaceUWP.DAL;
using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Utilities;
using CollaborativeWorkspaceUWP.Utilities.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.ViewModels
{
    public class AddBoardViewModel : BaseViewModel
    {
        BoardDataHandler boardDataHandler;

        public long ProjectId { get; set; }

        public AddBoardViewModel()
        {
            boardDataHandler = new BoardDataHandler();
        }

        public async Task AddBoardGroup(string name)
        {
            BoardGroup group = boardDataHandler.AddBoardGroupForProject(name, ProjectId);
            await ViewmodelEventHandler.Instance.Publish(new AddBoardGroupEvent() { BoardGroup = group});
        }
    }
}
