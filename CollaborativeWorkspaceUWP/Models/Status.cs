using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Models
{
    public class Status : ICloneable
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ColorCode { get; set; }

        public Status() { }

        public Status(long id, string name, string colorCode)
        {
            Name = name;
            Id = id;
            ColorCode = colorCode;
        }

        public object Clone() { return new Status(Id, Name, ColorCode); }
    }
}
