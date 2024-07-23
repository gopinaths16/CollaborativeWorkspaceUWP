 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace CollaborativeWorkspaceUWP.Models
{
    public class Priority : ICloneable
    {
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

        public object Clone()
        {
            return new Priority(Id, Name, ColorCode);
        }
    }
}
