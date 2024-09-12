using CollaborativeWorkspaceUWP.Models.Providers.Boards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Views.ViewObjects.Boards
{
    public interface IBoard
    {
        string Name { get; set; }
        long Id { get; set; }
        void SetBoardItemProvider(IBoardItemProvider boardItemProvider);
    }
}
