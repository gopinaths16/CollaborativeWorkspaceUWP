using CollaborativeWorkspaceUWP.DAL;
using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Utilities;
using CollaborativeWorkspaceUWP.Utilities.Custom;
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
        IncrementalLoadingCollection<UserTask> tasks;
        IncrementalLoadingCollection<UserTask> tasksForGroup;
        bool isAddTaskContextTriggered;
        List<Priority> priorityList;
        List<Status> statusList;
        UserTask currTask;
        Project currProject;
        bool isSingleWindowLayoutTriggered;
        Group currGroup;

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
            IsAddTaskContextTriggered = false;

            ViewmodelEventHandler.Instance.Subscribe<AddTaskEvent>(OnTaskAddtion);
            ViewmodelEventHandler.Instance.Subscribe<UpdateTaskEvent>(OnTaskUpdation);
            ViewmodelEventHandler.Instance.Subscribe<DeleteTaskEvent>(OnTaskDeletion);
            ViewmodelEventHandler.Instance.Subscribe<AddAttachmentEvent>(OnAttachmentAddition);
            ViewmodelEventHandler.Instance.Subscribe<AddCommentEvent>(OnCommentAddition);
        }

        public IncrementalLoadingCollection<UserTask> Tasks
        {
            get { return tasks; }
            set { tasks = value; }
        }

        public IncrementalLoadingCollection<UserTask> TasksForGroup
        {
            get { return tasksForGroup; }
            set { tasksForGroup = value; }
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

        public Group CurrGroup
        {
            get { return currGroup; }
            set
            {
                currGroup = value;
                NotifyPropertyChanged(nameof(CurrGroup));
            }
        }

        public UserTask CurrTask
        {
            get { return currTask; }
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
            CurrGroup = null;
            Tasks = new IncrementalLoadingCollection<UserTask>(taskDataHandler.GetTasksForProject(project.Id), 9);
            foreach (UserTask task in Tasks.source)
            {
                task.Attachments = attachmentDataHandler.GetAllAttachmentsForTask(task.Id);
                task.Comments = commentDataHandler.GetAllCommentsForCurrentTask(task.Id);
                var temp = Tasks.Where(item => item.Id == task.ParentTaskId).ToList();
                if (temp.Count > 0 && temp[0] != null)
                {
                    temp[0].SubTasks.Add(task);
                }
            }
            NotifyPropertyChanged(nameof(CurrentProject));
            NotifyPropertyChanged(nameof(CurrGroup));
            NotifyPropertyChanged(nameof(Tasks));
        }

        public void GetTasksForGroup(Group group)
        {
            CurrGroup = group;
            Tasks = new IncrementalLoadingCollection<UserTask>(taskDataHandler.GetTasksForGroup(group.Id), 9);
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
            NotifyPropertyChanged(nameof(CurrGroup));
            NotifyPropertyChanged(nameof(Tasks));
        }

        public void SetTasksForGroup(Group group)
        {
            CurrGroup = group;
            NotifyPropertyChanged(nameof(Group));
            TasksForGroup = new IncrementalLoadingCollection<UserTask>(new ObservableCollection<UserTask>(), 8);
            foreach(UserTask userTask in Tasks.Where((item) => item.GroupId == group.Id))
            {
                TasksForGroup.Add(userTask);
            }
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

        public async Task ReOrderTasks(UserTask taskToBeReOrdered)
        {
            if (taskToBeReOrdered != null)
            {
                int startIndex = taskToBeReOrdered.Order - 1;
                int endIndex = Tasks.IndexOf(taskToBeReOrdered);
                taskToBeReOrdered.Order = endIndex + 1;
                if(startIndex <= endIndex)
                {
                    taskDataHandler.UpdateOrderForTasks(startIndex + 1, endIndex + 1, -1, taskToBeReOrdered, currProject.Id);
                }
                else
                {
                    taskDataHandler.UpdateOrderForTasks(endIndex + 1, startIndex + 1, 1, taskToBeReOrdered, currProject.Id);
                }
                int order = 0;
                foreach (var task in Tasks)
                {
                    order++;
                    task.Order = order;
                }
                await ViewmodelEventHandler.Instance.Publish(new UpdateOrderEvent());
            }        
        }

        public override void Dispose()
        {
            ViewmodelEventHandler.Instance.Unsubscribe<AddTaskEvent>(OnTaskAddtion);
            ViewmodelEventHandler.Instance.Unsubscribe<UpdateTaskEvent>(OnTaskUpdation);
            ViewmodelEventHandler.Instance.Unsubscribe<DeleteTaskEvent>(OnTaskDeletion);
        }
    }
}
