using CollaborativeWorkspaceUWP.Views.ViewObjects.Boards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Utilities.Events
{
    public class AddBoardEvent
    {
        public IBoard Board { get; set; }
        public long Id { get; set; }
        public long ProjectId { get; set; }
    }
}
