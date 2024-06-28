using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Models
{
    public class Sprint
    {

        private long _id;
        private string _name;
        private long _ownerId;
        private long _projectId;

        public long Id { get { return _id; } set { _id = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public long OwnerId { get { return _ownerId; } set { _ownerId = value; } }
        public long ProjectId { get { return _projectId; } set { _projectId = value; } }

        public Sprint(long id, string name, long ownerId, long projectId)
        {
            this._id = id;
            this._name = name;
            this._ownerId = ownerId;
            this._projectId = projectId;
        }

    }
}
