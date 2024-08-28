using CollaborativeWorkspaceUWP.DAL;
using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Utilities.Events;
using CollaborativeWorkspaceUWP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.ViewModels
{
    public class TaskListItemViewModel : BaseModel
    {
        private UserTask task;
        List<Priority> priorityList;
        List<Status> statusList;

        TaskDataHandler taskDataHandler;
        PriorityDataHandler priorityDataHandler;
        StatusDataHandler statusDataHandler;
        UserDataHandler userDataHandler;
        CommentDataHandler commentDataHandler;
        AttachmentDataHandler attachmentDataHandler;

        public UserTask Task
        {
            get { return task; }
            set
            {
                task = value;
                if(task != null)
                {
                    task.Owner = userDataHandler.GetUser(task.OwnerId);
                    task.StatusData = GetTaskStatus(task.Status);
                    task.PriorityData = GetTaskPriority(task.Priority);
                    task.Comments = commentDataHandler.GetAllCommentsForCurrentTask(task.Id);
                    task.Attachments = attachmentDataHandler.GetAllAttachmentsForTask(task.Id);
                    task.SubTasks = taskDataHandler.GetAllSubTasks(task.Id);
                }
                NotifyPropertyChanged(nameof(Task));
            }
        }

        public TaskListItemViewModel()
        {
            taskDataHandler = new TaskDataHandler();
            priorityDataHandler = new PriorityDataHandler();
            statusDataHandler = new StatusDataHandler();
            userDataHandler = new UserDataHandler();
            commentDataHandler = new CommentDataHandler();
            attachmentDataHandler = new AttachmentDataHandler();

            priorityList = priorityDataHandler.GetPriorityData();
            statusList = statusDataHandler.GetStatusData();

            ViewmodelEventHandler.Instance.Subscribe<UpdateTaskEvent>(OnTaskUpdation);
        }

        public async Task UpdateTaskCompletionStatus(bool status)
        {
            Task.Status = status ? 2 : 3;
            Task.StatusData = GetTaskStatus(Task.Status);
            Task.PriorityData = GetTaskPriority(Task.Priority);
            UserTask temp = taskDataHandler.UpdateTask(Task);
            Task.Update(temp);
            await ViewmodelEventHandler.Instance.Publish(new UpdateTaskEvent() { Task = Task });
            NotifyPropertyChanged(nameof(Task));
        }

        public Priority GetTaskPriority(long priorityId)
        {
            return priorityList.Where(priority => priority.Id == priorityId).FirstOrDefault();
        }

        public Status GetTaskStatus(long statusId)
        {
            return statusList.Where(status => status.Id == statusId).FirstOrDefault();
        }

        public async Task OnTaskUpdation(UpdateTaskEvent taskEvent)
        {
            if (taskEvent != null && taskEvent.Task != null && taskEvent.Task.Id == Task.Id)
            {
                Task.Update(taskEvent.Task);
                NotifyPropertyChanged(nameof(Task));
            }
        }
    }
}
