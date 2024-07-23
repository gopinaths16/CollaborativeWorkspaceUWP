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

        public List<Priority> PriorityData
        {
            get { return priorityList; }
            set { priorityList = value; }
        }

        public List<Status> StatusData
        {
            get { return statusList; }
            set { statusList = value; }
        }

        public TaskDetailsViewModel()
        {
            priorityDataHandler = new PriorityDataHandler();
            statusDataHandler = new StatusDataHandler();
            taskDataHandler = new TaskDataHandler();

            PriorityData = priorityDataHandler.GetPriorityData();
            StatusData = statusDataHandler.GetStatusData();

            ViewmodelEventHandler.Instance.Subscribe<AddTaskEvent>(OnTaskAddition);
            ViewmodelEventHandler.Instance.Subscribe<UpdateTaskEvent>(OnTaskUpdation);
        }

        public Priority GetTaskPriority()
        {
            return PriorityData.Where(priority => priority.Id == CurrTask.Priority).ToList()[0];
        }

        public Status GetTaskStatus()
        {
            return StatusData.Where(status => status.Id == CurrTask.Status).ToList()[0];
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
            if(CurrTask != null && task.ParentTaskId == CurrTask.Id && CurrTask.SubTasks.Where(subTask => subTask.Id == task.Id).ToList().Count <= 0)
            {
                CurrTask.SubTasks.Add(task);
                NotifyPropertyChanged(nameof(CurrTask));
            }
        }

        public void SetTaskUpdatedContext()
        {
            if(CurrTask != null)
            {
                CurrTask.IsUpdated = true;
            }
        }

        public void UpdateTask()
        {
            UpdateTask(false);
        }

        public void UpdateTask(bool forceUpdate)
        {
            if (CurrTask != null && (CurrTask.IsUpdated || forceUpdate))
            {
                UserTask task = taskDataHandler.UpdateTask(CurrTask);
                CurrTask.Update(task);
                NotifyPropertyChanged(nameof(CurrTask));
                ViewmodelEventHandler.Instance.Publish(new UpdateTaskEvent() { Task = CurrTask });
                CurrTask.IsUpdated = false;
            }
        }

        public void OnTaskUpdation(UpdateTaskEvent e)
        {
            if(CurrTask != null)
            {
                if(CurrTask.Id == e.Task.Id)
                {
                    CurrTask.Update(e.Task);
                    NotifyPropertyChanged(nameof(CurrTask));
                } 
                else
                {
                    foreach (UserTask task in CurrTask.SubTasks)
                    {
                        if(task.Id == e.Task.Id)
                        {
                            task.Update(e.Task);
                            NotifyPropertyChanged(nameof(CurrTask));
                        }
                    }
                }
            }
        }

        public void UpdateTaskCompletionStatus(long taskId, bool status)
        {
            if(CurrTask.Id == taskId)
            {
                CurrTask.Status = status ? 2 : 3;
                CurrTask.StatusData = GetTaskStatus(CurrTask.Status);
                CurrTask.PriorityData = GetTaskPriority(CurrTask.Priority);
                taskDataHandler.UpdateTask(CurrTask);
                ViewmodelEventHandler.Instance.Publish(new UpdateTaskEvent() { Task = (UserTask)CurrTask.Clone() });
                NotifyPropertyChanged(nameof(CurrTask));
            }
        }

        public void UpdateSubTaskCompletionStatus(long taskId, bool status)
        {
            UserTask task = CurrTask.SubTasks.Where(subTask => subTask.Id == taskId).FirstOrDefault();
            if(task != null)
            {
                task.Status = status ? 2 : 3;
                task.StatusData = GetTaskStatus(task.Status);
                task.PriorityData = GetTaskPriority(task.Priority);
                taskDataHandler.UpdateTask(task);
                ViewmodelEventHandler.Instance.Publish(new UpdateTaskEvent() { Task = task });
            }
        }

        public Status GetTaskStatus(long statusId)
        {
            return statusList.Where(status => status.Id == statusId).ToList()[0];
        }

        public Priority GetTaskPriority(long priorityId)
        {
            return priorityList.Where(priority => priority.Id == priorityId).ToList()[0];
        }

        public void DeleteTask()
        {
            taskDataHandler.DeleteTask(CurrTask.Id);
            ViewmodelEventHandler.Instance.Publish(new DeleteTaskEvent() { TaskId = CurrTask.Id });
            CurrTask = null;
            NotifyPropertyChanged(nameof(CurrTask));
        }

        public void DeleteSubTask(long taskId)
        {
            UserTask task = CurrTask.SubTasks.Where(subTask => subTask.Id == taskId).FirstOrDefault();
            if (task != null)
            {
                taskDataHandler.DeleteTask(task.Id);
                CurrTask.SubTasks.Remove(task);
                NotifyPropertyChanged(nameof(CurrTask));
                ViewmodelEventHandler.Instance.Publish(new DeleteTaskEvent() { TaskId = task.Id });
            }
        }

        public override void Dispose()
        {
            ViewmodelEventHandler.Instance.Unsubscribe<AddTaskEvent>(OnTaskAddition);
            ViewmodelEventHandler.Instance.Unsubscribe<UpdateTaskEvent>(OnTaskUpdation);
        }
    }
}
