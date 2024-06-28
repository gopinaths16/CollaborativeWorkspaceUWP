using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Models
{
    public class UserTask
    {

        private long _id;
        private string _name;
        private string _description;
        private string _status;
        private string _priority;
        private long _projectId;
        private long _ownerId;
        private long _assigneeId;

        public long Id { get { return _id; } set { _id = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public string Description { get { return _description; } set { _description = value; } } 
        public string Status { get { return _status; } set { _status = value; } }
        public string Priority { get { return _priority; } set { _priority = value; } }
        public long ProjectId { get { return _projectId; } set { _projectId = value; } }
        public long OwnerId { get { return _ownerId; } set { _ownerId = value; } }
        public long AssigneeId { get { return _assigneeId; } set { _assigneeId = value; } }

        public UserTask()
        {

        }

        public UserTask(long id, string name, string description, string status, string priority, long projectId, long ownerId, long assigneeId)
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
