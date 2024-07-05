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

        public ObservableCollection<UserTask> Tasks { get; set; }
        public UserTask CurrTask { get; set; }

        public AddSubTaskViewModel()
        {
            taskDataHandler = new TaskDataHandler();
        }

        public void LoadNonSubTasks()
        {
            Tasks = taskDataHandler.GetNonSubTasks(CurrTask.Id, CurrTask.ProjectId);
            NotifyPropertyChanged(nameof(Tasks));
        }

        public void AddSubTaskForCurrTask(UserTask task)
        {
            Tasks.Remove(task);
            NotifyPropertyChanged(nameof(Tasks));
            taskDataHandler.AddSubTaskForTask(CurrTask.Id, task.Id);
        }
    }
}
