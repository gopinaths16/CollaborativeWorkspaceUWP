using CollaborativeWorkspaceUWP.Views.ViewObjects.Boards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Models.Providers.Boards
{
    public interface IBoardItemProvider
    {
        ICollection<IBoardItem> GetBoardItems(long boardId, long projectId);
    }
}
