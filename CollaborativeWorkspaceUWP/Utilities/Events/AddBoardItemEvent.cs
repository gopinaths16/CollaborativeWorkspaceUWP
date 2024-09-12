using CollaborativeWorkspaceUWP.Views.ViewObjects.Boards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Utilities.Events
{
    public class AddBoardItemEvent
    {
        public IBoardItem BoardItem { get; set; }
    }
}
