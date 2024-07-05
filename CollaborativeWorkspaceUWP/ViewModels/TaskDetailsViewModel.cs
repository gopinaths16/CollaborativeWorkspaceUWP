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
    public class TaskDetailsViewModel : BaseViewModel
    {
        UserTask currTask;
        Status status;
        Priority priority;

        List<Priority> priorityList;
        List<Status> statusList;

        PriorityDataHandler priorityDataHandler;
        StatusDataHandler statusDataHandler;
        TaskDataHandler taskDataHandler;

        public UserTask CurrTask
        {
            get { return currTask; }
            set
            {
                currTask = value;
                currTask.StatusData = GetTaskStatus();
                currTask.PriorityData = GetTaskPriority();
                currTask.SubTasks = GetAllSubTasks();
                NotifyPropertyChanged(nameof(CurrTask));
            }
        }

        public TaskDetailsViewModel()
        {
            priorityDataHandler = new PriorityDataHandler();
            statusDataHandler = new StatusDataHandler();
            taskDataHandler = new TaskDataHandler();

            priorityList = priorityDataHandler.GetPriorityData();
            statusList = statusDataHandler.GetStatusData();
        }

        public Priority GetTaskPriority()
        {
            return priorityList.Where(priority => priority.Id == CurrTask.Priority).ToList()[0];
        }

        public Status GetTaskStatus()
        {
            return statusList.Where(status => status.Id == CurrTask.Status).ToList()[0];
        }

        public ObservableCollection<UserTask> GetAllSubTasks()
        {
            return taskDataHandler.GetAllSubTasks(currTask.Id);
        }
    }
}
