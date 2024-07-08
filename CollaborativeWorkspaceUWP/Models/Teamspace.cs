using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Models
{
    public class Teamspace
    {

        private long _id;
        private string _name;
        private long _orgId;
        private long _ownerId;

        public long Id { get { return _id; } set { this._id = value; } }
        public string Name { get { return _name; } set { this._name = value; } }
        public long OrgId { get { return _orgId; } set { this._orgId = value; } }
        public long OwnerId { get { return _ownerId; } set { this._ownerId = value; } }

        public Teamspace() { }

        public Teamspace(long id, string name, long orgId, long owner)
        {
            this._id = id;
            this._name = name;
            this._orgId = orgId;
            this._ownerId = owner;
        }

    }
}
