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
    public class AddGroupViewModel : BaseViewModel
    {
        GroupDataHandler groupDataHandler;

        public long ProjectId { get; set; }
        public long BoardGroupId { get; set; }
        public bool IsBoardGroupContext { get; set; }

        public AddGroupViewModel()
        {
            groupDataHandler = new GroupDataHandler();
        }

        public async Task AddBoardGroup(string name)
        {
            Group group = groupDataHandler.AddBoardGroupForProject(name, ProjectId, BoardGroupId, IsBoardGroupContext);
            await ViewmodelEventHandler.Instance.Publish(new AddGroupEvent() { Group = group});
        }
    }
}
