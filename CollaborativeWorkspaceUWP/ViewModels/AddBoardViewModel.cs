using CollaborativeWorkspaceUWP.DAL;
using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Utilities;
using CollaborativeWorkspaceUWP.Utilities.Custom;
using CollaborativeWorkspaceUWP.Utilities.Events;
using CollaborativeWorkspaceUWP.Views.ViewObjects.Boards;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.ViewModels
{
    public class AddBoardViewModel : BaseViewModel
    {
        private GroupDataHandler groupDataHandler;
        private TaskDataHandler taskDataHandler;

        public long ProjectId { get; set; }
        public long BoardGroupId { get; set; }
        public bool IsBoardGroupContext { get; set; }

        public AddBoardViewModel()
        {
            groupDataHandler = new GroupDataHandler();
            taskDataHandler = new TaskDataHandler();
        }

        public async Task AddBoardGroup(string name)
        {
            Group group = groupDataHandler.AddBoardGroupForProject(name, ProjectId, BoardGroupId, IsBoardGroupContext);
            await ViewmodelEventHandler.Instance.Publish(new AddGroupEvent() { Group = group });
            await ViewmodelEventHandler.Instance.Publish(new AddBoardEvent() { Board = group, ProjectId = ProjectId, Id = group.BoardGroupId });
        }
    }
}
