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

        public TaskListViewModel()
        {
            taskDataHandler = new TaskDataHandler();
            priorityDataHandler = new PriorityDataHandler();
            statusDataHandler = new StatusDataHandler();

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
                var temp = Tasks.Where(item => item.Id == task.ParentTaskId).ToList();
                if (temp.Count > 0 && temp[0] != null)
                {
                    temp[0].SubTasks.Add(task);
                }
            }
            ViewmodelEventHandler.Instance.Subscribe<AddTaskEvent>(OnTaskAddtion);
            ViewmodelEventHandler.Instance.Subscribe<UpdateTaskEvent>(OnTaskUpdation);
            ViewmodelEventHandler.Instance.Subscribe<DeleteTaskEvent>(OnTaskDeletion);
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

        public void UpdateTaskCompletionStatus(long taskId, bool status)
        {
            foreach (var item in Tasks.Where(task => task.Id == taskId).ToList())
            {
                item.Status = status ? 2 : 3;
                item.StatusData = GetTaskStatus(item.Status);
                item.PriorityData = GetTaskPriority(item.Priority);
                taskDataHandler.UpdateTask(item);
                
                ViewmodelEventHandler.Instance.Publish(new UpdateTaskEvent() { Task = item });
            }
            NotifyPropertyChanged(nameof(Tasks));
        }

        public void OnTaskAddtion(AddTaskEvent e) 
        {
            AddTaskToList((UserTask)e.Task.Clone());
        }

        public void OnTaskUpdation(UpdateTaskEvent e)
        {
            if(Tasks != null && e != null && e.Task != null)
            {
                Tasks.Where(task => e.Task.Id == task.Id).First().Update(e.Task);
                NotifyPropertyChanged(nameof(Tasks));
            }
        }

        public void OnTaskDeletion(DeleteTaskEvent e)
        {
            if (Tasks != null && e != null)
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

        public override void Dispose()
        {
            ViewmodelEventHandler.Instance.Unsubscribe<AddTaskEvent>(OnTaskAddtion);
            ViewmodelEventHandler.Instance.Unsubscribe<UpdateTaskEvent>(OnTaskUpdation);
            ViewmodelEventHandler.Instance.Unsubscribe<DeleteTaskEvent>(OnTaskDeletion);
        }
    }
}
