using CollaborativeWorkspaceUWP.DAL;
using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Utilities;
using CollaborativeWorkspaceUWP.Utilities.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CollaborativeWorkspaceUWP.ViewModels
{
    public class TaskListViewModel : BaseViewModel
    {
        ObservableCollection<UserTask> tasks;
        bool isAddTaskContextTriggered;
        List<Priority> priorityList;
        List<Status> statusList;
        UserTask currTask;
        Project currProject;
        bool isSingleWindowLayoutTriggered;

        TaskDataHandler taskDataHandler;
        PriorityDataHandler priorityDataHandler;
        StatusDataHandler statusDataHandler;
        CommentDataHandler commentDataHandler;
        AttachmentDataHandler attachmentDataHandler;

        public bool IsLoaded { get; set; }

        public TaskListViewModel()
        {
            taskDataHandler = new TaskDataHandler();
            priorityDataHandler = new PriorityDataHandler();
            statusDataHandler = new StatusDataHandler();
            attachmentDataHandler = new AttachmentDataHandler();
            commentDataHandler = new CommentDataHandler();

            priorityList = priorityDataHandler.GetPriorityData();
            statusList = statusDataHandler.GetStatusData();
            Tasks = new ObservableCollection<UserTask>();
            IsAddTaskContextTriggered = false;
        }

        public ObservableCollection<UserTask> Tasks
        {
            get { return tasks; }
            set { tasks = value; }
        }

        public Project CurrentProject
        {
            get { return currProject; }
            set
            {
                currProject = value;
                NotifyPropertyChanged(nameof(CurrentProject));
            }
        }

        public UserTask CurrTask
        {
            get { return  currTask; }
            set { 
                currTask = value;
                NotifyPropertyChanged(nameof(CurrTask));
            }
        }

        public bool IsAddTaskContextTriggered
        {
            get { return isAddTaskContextTriggered; }
            set { 
                isAddTaskContextTriggered = value; 
                NotifyPropertyChanged(nameof(IsAddTaskContextTriggered));
            }
        }

        public bool IsSingleWindowLayoutTriggered
        {
            get { return isSingleWindowLayoutTriggered; }
            set
            {
                isSingleWindowLayoutTriggered = value;
                NotifyPropertyChanged(nameof(IsSingleWindowLayoutTriggered));
            }
        }

        public void GetTasksForProject(Project project)
        {
            CurrentProject = project;
            Tasks = taskDataHandler.GetTasksForProject(project.Id);
            foreach (UserTask task in Tasks)
            {
                task.Attachments = attachmentDataHandler.GetAllAttachmentsForTask(task.Id);
                task.Comments = commentDataHandler.GetAllCommentsForCurrentTask(task.Id);
                var temp = Tasks.Where(item => item.Id == task.ParentTaskId).ToList();
                if (temp.Count > 0 && temp[0] != null)
                {
                    temp[0].SubTasks.Add(task);
                }
            }
            ViewmodelEventHandler.Instance.Subscribe<AddTaskEvent>(OnTaskAddtion);
            ViewmodelEventHandler.Instance.Subscribe<UpdateTaskEvent>(OnTaskUpdation);
            ViewmodelEventHandler.Instance.Subscribe<DeleteTaskEvent>(OnTaskDeletion);
            ViewmodelEventHandler.Instance.Subscribe<AddAttachmentEvent>(OnAttachmentAddition);
            ViewmodelEventHandler.Instance.Subscribe<AddCommentEvent>(OnCommentAddition);
            NotifyPropertyChanged(nameof(CurrentProject));
            NotifyPropertyChanged(nameof(Tasks));
        }

        public void AddTaskToList(UserTask task)
        {
            task.StatusData = GetTaskStatus(task.Status);
            task.PriorityData = GetTaskPriority(task.Priority);
            Tasks.Add(task);
            var temp = Tasks.Where(pTask => pTask.Id == task.ParentTaskId);
            if (temp != null)
            {
                foreach(var pTask in temp)
                {
                    if(pTask.SubTasks.Where(sTask => sTask.Id == pTask.ParentTaskId) == null)
                    {
                        pTask.SubTasks.Add(task);
                    }
                }
            }
            NotifyPropertyChanged(nameof(Tasks));
        }

        public Priority GetTaskPriority(long priorityId)
        {
            return priorityList.Where(priority => priority.Id == priorityId).ToList()[0];
        }

        public Status GetTaskStatus(long statusId)
        {
            return statusList.Where(status => status.Id == statusId).ToList()[0];
        }

        public UserTask GetTaskForTaskId(long taskId)
        {
            return Tasks.Where(task => task.Id == taskId).ToList()[0];
        }

        public async Task OnTaskAddtion(AddTaskEvent e) 
        {
            AddTaskToList((UserTask)e.Task.Clone());
        }

        public async Task OnTaskUpdation(UpdateTaskEvent e)
        {
            if(Tasks != null && e != null && e.Task != null)
            {
                Tasks.Where(task => e.Task.Id == task.Id).First().Update(e.Task);
                NotifyPropertyChanged(nameof(Tasks));
            }
        }

        public async Task OnTaskDeletion(DeleteTaskEvent e)
        {
            if (Tasks != null && e != null && IsLoaded)
            {
                Tasks.Remove(Tasks.Where(task => e.TaskId == task.Id).First());
                foreach(var task in Tasks)
                {
                    if (task.SubTasks.Count > 0)
                    {
                        var subTask = task.SubTasks.Where(sTask => e.TaskId == sTask.Id);
                        if (subTask.Count() > 0)
                        {
                            task.SubTasks.Remove(subTask.First());
                        }
                    }
                }
                NotifyPropertyChanged(nameof(Tasks));
            }
        }

        public async Task OnAttachmentAddition(AddAttachmentEvent addAttachmentEvent)
        {
            var tasks = Tasks.Where(item => item.Id == addAttachmentEvent.Task.Id);
            if(tasks.Count() > 0)
            {
                UserTask task = tasks.First();
                task.NotifyChangesToUI();
            }
        }

        public async Task OnCommentAddition(AddCommentEvent e)
        {
            var tasks = Tasks.Where(item => item.Id == e.Comment.TaskId);
            if (tasks.Count() > 0)
            {
                UserTask task = tasks.First();
                if(task.Comments.Where(item => item.Id == e.Comment.Id).Count() <= 0)
                {
                    task.Comments.Add(e.Comment);
                }
                task.NotifyChangesToUI();
            }
        }

        public async Task ReOrderTasks()
        {
            int order = 0;
            foreach (var task in Tasks)
            {
                order++;
                task.Order = order;
                taskDataHandler.UpdateOrderForTask(task);
            }
            await ViewmodelEventHandler.Instance.Publish(new UpdateOrderEvent());
        }

        public override void Dispose()
        {
            ViewmodelEventHandler.Instance.Unsubscribe<AddTaskEvent>(OnTaskAddtion);
            ViewmodelEventHandler.Instance.Unsubscribe<UpdateTaskEvent>(OnTaskUpdation);
            ViewmodelEventHandler.Instance.Unsubscribe<DeleteTaskEvent>(OnTaskDeletion);
        }
    }
}
