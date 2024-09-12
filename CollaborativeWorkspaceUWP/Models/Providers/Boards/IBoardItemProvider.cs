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
        long BoardId { get; set; }
        bool IsDefaultProvider { get; }
        ICollection<IDefaultArgs> DefaultArgs { get; }

        ICollection<IBoardItem> GetBoardItems(long boardId, long projectId);
        bool DoesItemBelongToBoard(IBoardItem item);
        ICollection<IDefaultArgs> GetDefaultArgs();
    }
}
