using CollaborativeWorkspaceUWP.DAL;
using CollaborativeWorkspaceUWP.Models;
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
            get; set;
        }

        public bool IsAddTaskContextTriggered
        {
            get { return isAddTaskContextTriggered; }
            set { 
                isAddTaskContextTriggered = value; 
                NotifyPropertyChanged(nameof(IsAddTaskContextTriggered));
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
            NotifyPropertyChanged(nameof(CurrentProject));
            NotifyPropertyChanged(nameof(Tasks));
        }

        public void AddTaskToList(UserTask task)
        {
            task.StatusData = GetTaskStatus(task.Priority);
            task.PriorityData = GetTaskPriority(task.Status);
            Tasks.Add(task);
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
                item.Status = status ? 1 : 2;
                item.StatusData = GetTaskStatus(item.Status);
                item.PriorityData = GetTaskPriority(item.Status);
                taskDataHandler.UpdateTaskStatus(taskId, item.Status);
            }
            NotifyPropertyChanged(nameof(Tasks));
        }
    }
}
