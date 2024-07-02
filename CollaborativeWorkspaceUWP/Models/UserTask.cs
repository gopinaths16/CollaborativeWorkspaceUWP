using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Models
{
    public class UserTask
    {

        private double _id;
        private string _name;
        private string _description;
        private long _status;
        private long _priority;
        private double _projectId;
        private double _ownerId;
        private double _assigneeId;

        public double Id { get { return _id; } set { _id = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public string Description { get { return _description; } set { _description = value; } } 
        public long Status { get { return _status; } set { _status = value; } }
        public long Priority { get { return _priority; } set { _priority = value; } }
        public double ProjectId { get { return _projectId; } set { _projectId = value; } }
        public double OwnerId { get { return _ownerId; } set { _ownerId = value; } }
        public double AssigneeId { get { return _assigneeId; } set { _assigneeId = value; } }

        public UserTask()
        {

        }

        public UserTask(double id, string name, string description, long status, long priority, double projectId, double ownerId, double assigneeId)
        {
            Id = id;
            Name = name;
            Description = description;
            Status = status;
            Priority = priority;
            ProjectId = projectId;
            OwnerId = ownerId;
            AssigneeId = assigneeId;
        }

    }
}
