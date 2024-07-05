using CollaborativeWorkspaceUWP.DAL;
using CollaborativeWorkspaceUWP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.ViewModels
{
    public class AddSubTaskViewModel : BaseViewModel
    {
        private TaskDataHandler taskDataHandler;

        public ObservableCollection<UserTask> Tasks {  get; set; }
        
        public AddSubTaskViewModel()
        {
            taskDataHandler = new TaskDataHandler();
        }

        public void LoadNonSubTasks(UserTask task)
        {
            Tasks = taskDataHandler.GetNonSubTasks(task.Id, task.ProjectId);
            NotifyPropertyChanged(nameof(Tasks));
        }
    }
}
