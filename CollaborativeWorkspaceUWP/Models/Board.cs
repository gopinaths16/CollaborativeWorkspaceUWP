using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Models
{
    public class Board : BaseModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long ProjectId { get; set; }

        public Board() { }
    }
}
