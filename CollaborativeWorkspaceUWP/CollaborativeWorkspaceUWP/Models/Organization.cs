using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Models
{
    public class Organization
    {

        private long _id;
        private string _name;
        private long _ownerId;

        public long Id { get { return _id; } set { this._id = value; } }
        public string Name { get { return _name; } set { this._name = value; } }
        public long Owner { get { return _ownerId; } set { this._ownerId = value; } }

        public Organization(long id, string name, long owner)
        {
            this._id = id;
            this._name = name;
            this._ownerId = owner;
        }

    }
}
