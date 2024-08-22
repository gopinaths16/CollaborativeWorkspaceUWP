using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Models
{
    public class BoardGroup : BaseModel, ICloneable
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long ProjectId { get; set; }

        public BoardGroup() { }

        public Object Clone()
        {
            BoardGroup boardGroup = new BoardGroup() { Id = Id, Name = Name, ProjectId = ProjectId};
            return boardGroup;
        }
    }
}
