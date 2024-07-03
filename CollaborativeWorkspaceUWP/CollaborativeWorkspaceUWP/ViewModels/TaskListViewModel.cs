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
            get { return tasks; }
            set { tasks = value; }
        }
        public Project CurrentProject
        {
            get; set;
        }

        public void GetTasksForProject(Project project)
        {
            Tasks = taskDataHandler.GetTasksForProject(project.Id);
            CurrentProject = project;
            NotifyPropertyChanged(nameof(CurrentProject));
            NotifyPropertyChanged(nameof(Tasks));
        }

        public void AddTaskToList(UserTask task)
        {
            Tasks.Add(task);
            NotifyPropertyChanged(nameof(Tasks));
        }
    }
}
