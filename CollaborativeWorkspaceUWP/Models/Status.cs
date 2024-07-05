using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Models
{
    public class Status
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public Status() { }

        public Status(long id, string name)
        {
            Name = name;
            Id = id;
        }
    }
}
