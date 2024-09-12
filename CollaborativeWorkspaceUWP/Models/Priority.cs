using CollaborativeWorkspaceUWP.Models.Providers.Boards;
using CollaborativeWorkspaceUWP.Views.ViewObjects.Boards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace CollaborativeWorkspaceUWP.Models
{
    public class Priority : ICloneable, IDefaultArgs
    {
        public IBoardItemProvider BoardItemProvider { get; set; }

        public long Id { get; set; }
        public string Name { get; set; }
        public string ColorCode { get; set; }
        
        public Priority() { }

        public Priority(long id, string name, string colorCode)
        {
            Id = id;
            Name = name;
            ColorCode = colorCode;
        }

        public void SetBoardItemProvider(IBoardItemProvider boardItemProvider)
        {
            BoardItemProvider = boardItemProvider;
        }

        public object Clone()
        {
            return new Priority(Id, Name, ColorCode);
        }
    }
}
