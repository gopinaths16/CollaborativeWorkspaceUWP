using CollaborativeWorkspaceUWP.DAL;
using CollaborativeWorkspaceUWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.ViewModels
{
    public class AddTaskViewModel : BaseViewModel
    {
        private List<Priority> priorityData;
        private List<Status> statusData;

        TaskDataHandler taskDataHandler;
        PriorityDataHandler priorityDataHandler;
        StatusDataHandler statusDataHandler;

        public List<Priority> PriorityData
        {
            get { return priorityData; }
            set { priorityData = value; }
        }

        public List<Status> StatusData
        {
            get { return statusData; }
            set { statusData = value; }
        }

        public AddTaskViewModel()
        {
            priorityDataHandler = new PriorityDataHandler();
            statusDataHandler = new StatusDataHandler();
            taskDataHandler = new TaskDataHandler();
            PriorityData = priorityDataHandler.GetPriorityDataForUI();
            StatusData = statusDataHandler.GetStatusDataForUI();
        }

        public UserTask AddTask(UserTask task)
        {
            return taskDataHandler.AddTask(task);
        }
    }
}
