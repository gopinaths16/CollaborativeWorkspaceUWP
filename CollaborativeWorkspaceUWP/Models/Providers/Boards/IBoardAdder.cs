using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Models.Providers.Boards
{
    public interface IBoardAdder
    {
        void SetDefaultArgs(ICollection<IDefaultArgs> args);
        void Focus();
        void ClearAllFields();
    }
}
