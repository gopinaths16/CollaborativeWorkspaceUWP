using CollaborativeWorkspaceUWP.Utilities.Custom;
using CollaborativeWorkspaceUWP.Views.ViewObjects.Boards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Models.Providers.Boards
{
    public abstract class BoardProvider
    {
        public string Name { get; set; }
        public long ProjectId { get; set; }
        public long BoardGroupId { get; set; }

        public long GetBoardViewId()
        {
            return BoardGroupId;
        }

        public abstract IBoardItemProvider GetBoardItemProvider();

        public abstract ICollection<IBoard> GetBoards();
    }
}
