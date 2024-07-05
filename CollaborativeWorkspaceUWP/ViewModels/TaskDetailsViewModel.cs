using CollaborativeWorkspaceUWP.DAL;
using CollaborativeWorkspaceUWP.Models;
using System;
using System.Collections.Generic;
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

        public UserTask CurrTask
        {
            get { return currTask; }
            set
            {
                currTask = value;
                currTask.StatusData = GetTaskStatus();
                currTask.PriorityData = GetTaskPriority();
                NotifyPropertyChanged(nameof(CurrTask));
            }
        }

        public TaskDetailsViewModel()
        {
            priorityDataHandler = new PriorityDataHandler();
            statusDataHandler = new StatusDataHandler();

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

    }
}
