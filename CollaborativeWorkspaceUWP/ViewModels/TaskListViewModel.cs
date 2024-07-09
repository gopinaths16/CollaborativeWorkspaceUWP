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
            tasks = new ObservableCollection<UserTask>();
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

        public void GetTasksForProject(Project project)
        {
            CurrentProject = project;
            Tasks = taskDataHandler.GetTasksForProject(project.Id);
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
    }
}
