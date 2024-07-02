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
        TaskDataHandler taskDataHandler;

        public TaskListViewModel()
        {
            taskDataHandler = new TaskDataHandler();
            tasks = new ObservableCollection<UserTask>();
        }

        public ObservableCollection<UserTask> Tasks
        {
            get
            {
                return tasks;
            }
            set
            {
                tasks = value;
            }
        }

        public void GetTasksForProject(double projectId)
        {
            Tasks = taskDataHandler.GetTasksForProject(projectId);
            NotifyPropertyChanged(nameof(Tasks));
        }

        public void AddNewTask(UserTask task)
        {
            Tasks.Add(task);
            taskDataHandler.AddTask(task);
            NotifyPropertyChanged(nameof(Tasks));
        }
    }
}
