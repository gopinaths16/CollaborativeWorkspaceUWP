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
    public class TaskDetailsViewModel : BaseViewModel
    {
        UserTask currTask;
        bool isAddSubTaskContextTriggered;

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
                if (currTask != null)
                {
                    SubTasks = currTask.SubTasks;
                    currTask.StatusData = GetTaskStatus();
                    currTask.PriorityData = GetTaskPriority();
                    IsAddSubTaskContextTriggered = false;
                }
                NotifyPropertyChanged(nameof(CurrTask));
            }
        }

        public ObservableCollection<UserTask> SubTasks
        {
            get; set;
        }

        public bool IsAddSubTaskContextTriggered
        {
            get { return isAddSubTaskContextTriggered; }
            set
            {
                isAddSubTaskContextTriggered = value;
                NotifyPropertyChanged(nameof(IsAddSubTaskContextTriggered));
            }
        }

        public TaskDetailsViewModel()
        {
            priorityDataHandler = new PriorityDataHandler();
            statusDataHandler = new StatusDataHandler();
            taskDataHandler = new TaskDataHandler();

            priorityList = priorityDataHandler.GetPriorityData();
            statusList = statusDataHandler.GetStatusData();

            ViewmodelEventHandler.Instance.Subscribe<AddTaskEvent>(OnTaskAddition);
        }

        public Priority GetTaskPriority()
        {
            return priorityList.Where(priority => priority.Id == CurrTask.Priority).ToList()[0];
        }

        public Status GetTaskStatus()
        {
            return statusList.Where(status => status.Id == CurrTask.Status).ToList()[0];
        }

        public void AddSubTaskToCurrTask(UserTask task)
        {
            SubTasks.Add(task);
            NotifyPropertyChanged(nameof(SubTasks));
        }

        public void CurrTaskPropertyChanged()
        {
            NotifyPropertyChanged(nameof(CurrTask));
            NotifyPropertyChanged(nameof(SubTasks));
        }

        public void OnTaskAddition(AddTaskEvent e)
        {
            UserTask task = e.Task;
            if(CurrTask != null && task.ParentTaskId == CurrTask.Id)
            {
                CurrTask.SubTasks.Add(task);
                NotifyPropertyChanged(nameof(CurrTask));
            }
        }
    }
}
