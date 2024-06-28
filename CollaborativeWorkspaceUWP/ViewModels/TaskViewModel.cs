using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CollaborativeWorkspaceUWP.Models;
using Windows.UI.Popups;
using CollaborativeWorkspaceUWP.Utilities;
using CollaborativeWorkspaceUWP.Models.Comm;
using CollaborativeWorkspaceUWP.Utilities.Comm;
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
            ButtonCommand = new RelayCommand<UserTask>(OnButtonClicked);
        }

        private async void OnButtonClicked(UserTask item)
        {
            if (item != null)
            {
                ViewModelEventManager.Instance.Publish(new TaskWindowOpenEventArgs() { CurrentTask = item, IsWindowOpen = true });
            }
        }

        public void CreateNewTask()
        {
            ViewModelEventManager.Instance.Publish(new AddTaskWindowOpenEvent() { IsWindowOpen = true });
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

        public void AddNewTask(string name, string status, string priority, string descripion)
        {
            UserTask task = new UserTask(0, name, descripion, status, priority, 0, 0, 0);
            tasks.Add(task);
            taskDataHandler.AddTask(task);
        }

    }
}
