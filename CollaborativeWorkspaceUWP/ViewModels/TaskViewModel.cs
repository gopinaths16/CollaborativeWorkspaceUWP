using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CollaborativeWorkspaceUWP.Models;
using Windows.UI.Popups;
using CollaborativeWorkspaceUWP.Utilities;
using CollaborativeWorkspaceUWP.DAL;
using System.Collections.ObjectModel;

namespace CollaborativeWorkspaceUWP.ViewModels
{
    public class TaskViewModel : BaseViewModel
    {
        ObservableCollection<UserTask> tasks;
        UserTask currentTask;

        public ICommand ButtonCommand { get; set; } 
        TaskDataHandler taskDataHandler;

        public TaskViewModel()
        {
            taskDataHandler = new TaskDataHandler();
            tasks = taskDataHandler.GetAllTasks();
        }

        public ObservableCollection<UserTask> Tasks
        {
            get
            {
                return tasks;
            }
        }

        public UserTask CurrentTask
        {
            get { return currentTask; }
            set { 
                currentTask = value;
                NotifyPropertyChanged(nameof(CurrentTask));
            }
        }

        public void AddNewTask(string name, long status, long priority, string descripion)
        {
            UserTask task = new UserTask(0, name, descripion, status, priority, 0, 0, 0);
            tasks.Add(task);
            taskDataHandler.AddTask(task);
        }

    }
}
