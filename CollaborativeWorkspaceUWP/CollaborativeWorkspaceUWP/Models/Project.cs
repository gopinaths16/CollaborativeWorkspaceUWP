using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Models
{
    public class Project
    {

        private long _id;
        private string _name;
        private int _status;
        private int _priority;
        private long _teamspaceId;
        private long _ownerId;

        public long Id { get { return _id; } set { _id = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public int Status { get { return _status; } set { _status = value; } }
        public int Priority { get { return _priority; } set { _priority = value; } }
        public long TeamsapceId {  get { return _teamspaceId; } set { _teamspaceId = value; } }
        public long OwnerId { get { return _ownerId; } set { _ownerId = value; } }

        public Project()
        {

        }
        public Project(long id, string name, int status, int priority, long teamspaceId, long ownerId)
        {
            this._id = id;
            this._name = name;
            this._status = status;
            this._priority = priority;
            this._teamspaceId = teamspaceId;
            this._ownerId = ownerId;
        }

    }
}
