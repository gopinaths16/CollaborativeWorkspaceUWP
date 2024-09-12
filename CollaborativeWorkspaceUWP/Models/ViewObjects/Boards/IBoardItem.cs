using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Views.ViewObjects.Boards
{
    public interface IBoardItem
    {
        long Id { get; set; }
        long GroupId { get; set; }
    }
}
