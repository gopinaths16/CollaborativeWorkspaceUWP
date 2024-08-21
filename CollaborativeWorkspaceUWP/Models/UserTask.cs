using CollaborativeWorkspaceUWP.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
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
        private DateTime? dueDate;
        private DateTime modifiedTime;
        private string mTime;
        private User owner;

        public long Id { get { return _id; } set { _id = value; } }
        public string Name { 
            get { return _name; } 
            set { 
                if(value != _name)
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

        public string ModifiedTime
        {
            get
            {
                if(modifiedTime != DateTime.MinValue)
                {
                    return modifiedTime.ToString("F");
                }
                return null;
            }
        }

        public ObservableCollection<UserTask> SubTasks { get; set; }

        public ObservableCollection<Attachment> Attachments { get; set; }

        public ObservableCollection<Comment> Comments { get; set; }

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

        public User Owner
        {
            get { return owner; }
            set
            {
                owner = value;
                NotifyPropertyChanged(nameof(Owner));
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

        public int Order { get; set; }

        public UserTask Self
        {
            get
            {
                return this;
            }
        }

        public DateTime? DueDate
        {
            get { return dueDate; }
            set
            {
                dueDate = value ?? null;
                NotifyPropertyChanged(nameof(DueDate));
            }
        }

        public UserTask()
        {
            SubTasks = new ObservableCollection<UserTask>();
            Attachments = new ObservableCollection<Attachment>();
            Comments = new ObservableCollection<Comment>();
            DueDate = null;
        }

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
            Attachments = new ObservableCollection<Attachment>();
            Comments = new ObservableCollection<Comment>();
            IsUpdated = false;
            DueDate = null;
        }

        public object Clone()
        {
            UserTask task = new UserTask(_id, _name, _description, _status, _priority, _projectId, _ownerId, _assigneeId, _parentTaskId);
            task.SubTasks = new ObservableCollection<UserTask>();
            task.DueDate = DueDate;
            if(PriorityData != null)
            {
                task.PriorityData = (Priority)PriorityData.Clone();
            }
            if(StatusData != null)
            {
                task.StatusData = (Status)StatusData.Clone();
            }

            foreach (var subTask in SubTasks)
            {
                task.SubTasks.Add(subTask);
            }
            foreach (var attachment in Attachments)
            {
                task.Attachments.Add(attachment);
            }
            foreach(var comment in Comments)
            {
                task.Comments.Add(comment); 
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
                if (task.SubTasks.Count > 0)
                {
                    SubTasks = task.SubTasks;
                }
                if (task.Attachments.Count > 0)
                {
                    Attachments = task.Attachments; 
                }
                if(task.Comments.Count > 0) 
                { 
                    Comments = task.Comments; 
                }
                StatusData = task.StatusData != null ? task.StatusData : StatusData;
                PriorityData = task.PriorityData != null ? task.PriorityData : PriorityData;
                if(task.modifiedTime != DateTime.MinValue)
                {
                    modifiedTime = task.modifiedTime;
                    NotifyPropertyChanged(nameof(ModifiedTime));
                }
            }
        }

        public void SetModifiedTime(DateTime date)
        {
            if(date != null)
            {
                modifiedTime = date;
            }
        }

        public void NotifyChangesToUI()
        {
            NotifyPropertyChanged(nameof(Attachments));
            NotifyPropertyChanged(nameof(Comments));
        }
    }
}
