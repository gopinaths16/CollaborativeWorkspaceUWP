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
        public String CurrentProjectName
        {
            get; set;
        }

        public void GetTasksForProject(Project project)
        {
            Tasks = taskDataHandler.GetTasksForProject(project.Id);
            CurrentProjectName = project.Name;
            NotifyPropertyChanged(nameof(CurrentProjectName));
            NotifyPropertyChanged(nameof(Tasks));
        }
    }
}
