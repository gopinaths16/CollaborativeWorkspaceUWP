using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Models
{
    public class UserTask : BaseModel, ICloneable
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
        private Status statusData;
        private Priority priorityData;
        private bool isCompleted;

        public long Id { get { return _id; } set { _id = value; } }
        public string Name { 
            get { return _name; } 
            set { 
                if(!value.Equals(_name))
                {
                    _name = value;
                    NotifyPropertyChanged(nameof(Name));
                }
            } 
        }
        public string Description { get { return _description; } set { _description = value; } } 
        public long Status { 
            get { return _status; } 
            set { 
                _status = value;
                NotifyPropertyChanged(nameof(Status));
            } 
        }
        public long Priority { 
            get { return _priority; } 
            set { 
                _priority = value;
                NotifyPropertyChanged(nameof(Priority));
            } 
        }
        public long ProjectId { 
            get { return _projectId; } 
            set { _projectId = value; } 
        }
        public long OwnerId { get { return _ownerId; } set { _ownerId = value; } }
        public long AssigneeId { get { return _assigneeId; } set { _assigneeId = value; } }
        public long ParentTaskId { get { return _parentTaskId; } set { _parentTaskId = value; } }

        public ObservableCollection<UserTask> SubTasks { get; set; }

        public Status StatusData
        {
            get { return statusData; }
            set
            {
                statusData = value;
                NotifyPropertyChanged(nameof(StatusData));
            }
        }

        public Priority PriorityData
        {
            get { return priorityData; }
            set
            {
                priorityData = value;
                NotifyPropertyChanged(nameof(PriorityData));
            }
        }

        public bool IsCompleted
        {
            get { return isCompleted; }
            set
            {
                isCompleted = value;
                NotifyPropertyChanged(nameof(IsCompleted));
            }
        }

        public bool IsUpdated { 
            get; 
            set; 
        }

        public UserTask() {}

        public UserTask(long id, string name, string description, long status, long priority, long projectId, long ownerId, long assigneeId, long parentTaskId)
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
            IsCompleted = status == 3;
            SubTasks = new ObservableCollection<UserTask>();
            IsUpdated = false;
        }

        public object Clone()
        {
            UserTask task = new UserTask(Id, Name, Description, Status, Priority, ProjectId, OwnerId, AssigneeId, ParentTaskId);
            task.SubTasks = new ObservableCollection<UserTask>();
            //task.PriorityData = (Priority)PriorityData.Clone();
            //task.StatusData = (Status)StatusData.Clone();
            foreach (var subTask in SubTasks)
            {
                task.SubTasks.Add(subTask);
            }
            return task;
        }

        public void Update(UserTask task)
        {
            if (task != null && task.Id == Id)
            {
                Name = task.Name;
                Description = task.Description;
                Status = task.Status;
                Priority = task.Priority;
                ProjectId = task.ProjectId;
                OwnerId = task.OwnerId;
                AssigneeId = task.AssigneeId;
                ParentTaskId = task.ParentTaskId;
                IsCompleted = task.IsCompleted;
                if(task.SubTasks.Count > 0)
                {
                    SubTasks = task.SubTasks;
                }
                StatusData = task.StatusData != null ? task.StatusData : StatusData;
                PriorityData = task.PriorityData != null ? task.PriorityData : PriorityData;
            }
        }
    }
}
