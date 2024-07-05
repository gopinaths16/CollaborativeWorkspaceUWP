 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace CollaborativeWorkspaceUWP.Models
{
    public class Priority
    {
        public long Id { get; set; }
        public string Name { get; set; }
        
        public Priority() { }

        public Priority(long id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
