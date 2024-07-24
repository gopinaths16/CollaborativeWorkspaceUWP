using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Models
{
    public class Attachment : BaseModel
    {
        private long id;
        private string name;
        private string path;
        private string type;
        private long taskId;

        public long Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public long TaskId
        {
            get { return taskId; }
            set { taskId = value; }
        }
    }
}
