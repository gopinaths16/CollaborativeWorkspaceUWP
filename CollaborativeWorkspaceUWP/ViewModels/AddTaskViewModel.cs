using CollaborativeWorkspaceUWP.DAL;
using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Utilities;
using CollaborativeWorkspaceUWP.Utilities.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.ViewModels
{
    public class AddTaskViewModel : BaseViewModel
    {
        private List<Priority> priorityData;
        private List<Status> statusData;

        private Priority priority;
        private Status status;
        
        private long projectId;

        TaskDataHandler taskDataHandler;
        PriorityDataHandler priorityDataHandler;
        StatusDataHandler statusDataHandler;

        private ObservableCollection<UserTask> TasksForCurrProject {  get; set; }

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

        public Priority Priority
        {
            get { return priority; }
            set
            {
                priority = value;
            }
        }

        public Status Status
        {
            get { return status; }
            set
            {
                status = value;
            }
        }

        public long ProjectId
        {
            get { return projectId; }
            set
            {
                projectId = value;
                TasksForCurrProject = taskDataHandler.GetTasksForProject(projectId);
            }
        }

        public AddTaskViewModel()
        {
            priorityDataHandler = new PriorityDataHandler();
            statusDataHandler = new StatusDataHandler();
            taskDataHandler = new TaskDataHandler();
            PriorityData = priorityDataHandler.GetPriorityData();
            StatusData = statusDataHandler.GetStatusData();

            Priority = PriorityData.FirstOrDefault();
            Status = StatusData.FirstOrDefault();

            ViewmodelEventHandler.Instance.Subscribe<AddTaskEvent>(OnTaskAddition);
            ViewmodelEventHandler.Instance.Subscribe<UpdateOrderEvent>(OnTaskOrderUpdation);
        }

        public async Task AddTask(UserTask task)
        {
            task.Order = 1;
            if(TasksForCurrProject.Count() > 0)
            {
                task.Order += TasksForCurrProject.Last().Order;
            }
            UserTask result = taskDataHandler.AddTask(task);
            await ViewmodelEventHandler.Instance.Publish(new AddTaskEvent() { Task = result });
            await ViewmodelEventHandler.Instance.Publish(new AddBoardItemEvent() { BoardItem = result });
        }

        public async Task OnTaskAddition(AddTaskEvent addTaskEvent)
        {
            var task = TasksForCurrProject.Where(item => item.Id == addTaskEvent.Task.Id);
            if(task.Count() <= 0)
            {
                TasksForCurrProject.Add(addTaskEvent.Task);
            }
        }

        public async Task OnTaskOrderUpdation(UpdateOrderEvent updateOrderEvent)
        {
            TasksForCurrProject = taskDataHandler.GetTasksForProject(ProjectId);
        }

        public void Dispose()
        {
            ViewmodelEventHandler.Instance.Unsubscribe<AddTaskEvent>(OnTaskAddition);
            ViewmodelEventHandler.Instance.Unsubscribe<UpdateOrderEvent>(OnTaskOrderUpdation);
        }

        public void NotifyUI()
        {
            NotifyPropertyChanged(nameof(Status));
            NotifyPropertyChanged(nameof(Priority));
        }
    }
}
