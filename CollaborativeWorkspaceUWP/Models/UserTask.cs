using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private long _status;
        private long _priority;
        private long _projectId;
        private long _ownerId;
        private long _assigneeId;
        private long _parentTaskId;

        public long Id { get { return _id; } set { _id = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public string Description { get { return _description; } set { _description = value; } } 
        public long Status { get { return _status; } set { _status = value; } }
        public long Priority { get { return _priority; } set { _priority = value; } }
        public long ProjectId { get { return _projectId; } set { _projectId = value; } }
        public long OwnerId { get { return _ownerId; } set { _ownerId = value; } }
        public long AssigneeId { get { return _assigneeId; } set { _assigneeId = value; } }
        public long ParentTaskId { get { return _parentTaskId; } set { _parentTaskId = value; } }

        public ObservableCollection<UserTask> SubTasks { get; set; }

        public Status StatusData{ get; set; }

        public Priority PriorityData { get; set; }

        public bool IsCompleted { get; set; }

        public UserTask() {}

        public UserTask(long id, string name, string description, int status, int priority, long projectId, long ownerId, long assigneeId, long parentTaskId)
        {
            Id = id;
            Name = name;
            Description = description;
            Status = status;
            Priority = priority;
            ProjectId = projectId;
            OwnerId = ownerId;
            AssigneeId = assigneeId;
            ParentTaskId = parentTaskId;
            IsCompleted = status == 2;
            SubTasks = new ObservableCollection<UserTask>();
        }

    }
}
