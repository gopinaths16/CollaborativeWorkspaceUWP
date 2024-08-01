using CollaborativeWorkspaceUWP.DAL;
using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Utilities;
using CollaborativeWorkspaceUWP.Utilities.Events;
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
            PriorityData = priorityDataHandler.GetPriorityData();
            StatusData = statusDataHandler.GetStatusData();
        }

        public async Task AddTask(UserTask task)
        {
            UserTask result = taskDataHandler.AddTask(task);
            await ViewmodelEventHandler.Instance.Publish(new AddTaskEvent() { Task = result });
        }
    }
}
