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
        bool isAddAttachmentContextTriggered;

        public bool IsLoaeded { get; set; }
        public bool IsSubTaskSelected { get; set; }

        List<Priority> priorityList;
        List<Status> statusList;

        PriorityDataHandler priorityDataHandler;
        StatusDataHandler statusDataHandler;
        TaskDataHandler taskDataHandler;
        AttachmentDataHandler attachmentDataHandler;

        public UserTask CurrTask
        {
            get { return currTask; }
            set
            {
                
                if (value != null)
                {
                    currTask = (UserTask)value.Clone();
                    currTask.Attachments = null;
                    currTask.Attachments = GetAttachmentsForTask(currTask.Id);
                    IsAddSubTaskContextTriggered = false;
                }
                else
                {
                    currTask = null;
                }
                NotifyPropertyChanged(nameof(CurrTask));
            }
        }

        public Dictionary<long, UserTask> Task_SubTask_Mapper  { get; set; }

        public UserTask PrevTask { get; set; }

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

        public bool IsAddAttachmentContextTriggered
        {
            get { return isAddAttachmentContextTriggered; }
            set
            {
                isAddAttachmentContextTriggered = value;
                NotifyPropertyChanged(nameof(IsAddAttachmentContextTriggered));
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
            attachmentDataHandler = new AttachmentDataHandler();

            Task_SubTask_Mapper = new Dictionary<long, UserTask>();

            PriorityData = priorityDataHandler.GetPriorityData();
            StatusData = statusDataHandler.GetStatusData();

            ViewmodelEventHandler.Instance.Subscribe<AddTaskEvent>(OnTaskAddition);
            ViewmodelEventHandler.Instance.Subscribe<UpdateTaskEvent>(OnTaskUpdation);
            ViewmodelEventHandler.Instance.Subscribe<AddAttachmentEvent>(OnAttachmentAddition);
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

        public async Task OnTaskAddition(AddTaskEvent e)
        {
            UserTask task = e.Task;
            if (CurrTask != null && task.ParentTaskId == CurrTask.Id && CurrTask.SubTasks.Where(subTask => subTask.Id == task.Id).ToList().Count <= 0)
            {
                task.StatusData = GetTaskStatus(task.Status);
                task.PriorityData = GetTaskPriority(task.Priority);
                CurrTask.SubTasks.Add(task);
                NotifyPropertyChanged(nameof(CurrTask));
            }

            if (PrevTask != null)
            {
                var temp = PrevTask.SubTasks.Where(item => item.Id == task.ParentTaskId);
                if (temp.Count() > 0)
                {
                    foreach (UserTask t in temp)
                    {
                        t.SubTasks.Add(task);
                    }
                }
            }
        }

        public void SetTaskUpdatedContext()
        {
            if (CurrTask != null && IsLoaeded)
            {
                CurrTask.IsUpdated = true;
            }
        }

        public async Task UpdateTask()
        {
            await UpdateTask(false);
        }

        public async Task UpdateTask(bool forceUpdate)
        {
            if (CurrTask != null && (CurrTask.IsUpdated || forceUpdate) && IsLoaeded)
            {
                UserTask task = taskDataHandler.UpdateTask(CurrTask);
                task.Attachments = CurrTask.Attachments;
                task.SubTasks = CurrTask.SubTasks;
                task.StatusData = GetTaskStatus(task.Status);
                task.PriorityData = GetTaskPriority(task.Priority);
                CurrTask.Update(task);
                NotifyPropertyChanged(nameof(CurrTask));
                await ViewmodelEventHandler.Instance.Publish(new UpdateTaskEvent() { Task = CurrTask });
                CurrTask.IsUpdated = false;
            }
        }

        public async Task OnTaskUpdation(UpdateTaskEvent e)
        {
            if (CurrTask != null && IsLoaeded)
            {
                if (CurrTask.Id == e.Task.Id)
                {
                    CurrTask.Update(e.Task);
                    NotifyPropertyChanged(nameof(CurrTask));
                }
                else
                {
                    foreach (UserTask task in CurrTask.SubTasks)
                    {
                        if (task.Id == e.Task.Id)
                        {
                            task.Update(e.Task);
                            NotifyPropertyChanged(nameof(CurrTask));
                        }
                    }
                }
            }
        }

        public async Task UpdateTaskCompletionStatus(long taskId, bool status)
        {
            if (CurrTask.Id == taskId)
            {
                CurrTask.Status = status ? 2 : 3;
                CurrTask.StatusData = GetTaskStatus(CurrTask.Status);
                CurrTask.PriorityData = GetTaskPriority(CurrTask.Priority);
                taskDataHandler.UpdateTask(CurrTask);
                await ViewmodelEventHandler.Instance.Publish(new UpdateTaskEvent() { Task = (UserTask)CurrTask.Clone() });
                NotifyPropertyChanged(nameof(CurrTask));
            }
        }

        public async Task UpdateSubTaskCompletionStatus(long taskId, bool status)
        {
            UserTask task = CurrTask.SubTasks.Where(subTask => subTask.Id == taskId).FirstOrDefault();
            if (task != null)
            {
                task.Status = status ? 2 : 3;
                task.StatusData = GetTaskStatus(task.Status);
                task.PriorityData = GetTaskPriority(task.Priority);
                taskDataHandler.UpdateTask(task);
                await ViewmodelEventHandler.Instance.Publish(new UpdateTaskEvent() { Task = task });
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

        public async Task DeleteTask()
        {
            taskDataHandler.DeleteTask(CurrTask.Id);
            await ViewmodelEventHandler.Instance.Publish(new DeleteTaskEvent() { TaskId = CurrTask.Id });
            CurrTask = null;
            NotifyPropertyChanged(nameof(CurrTask));
        }

        public async Task DeleteSubTask(long taskId)
        {
            UserTask task = CurrTask.SubTasks.Where(subTask => subTask.Id == taskId).FirstOrDefault();
            if (task != null)
            {
                taskDataHandler.DeleteTask(task.Id);
                CurrTask.SubTasks.Remove(task);
                NotifyPropertyChanged(nameof(CurrTask));
                await ViewmodelEventHandler.Instance.Publish(new DeleteTaskEvent() { TaskId = task.Id });
            }
        }

        public void SetSubTaskToCurrTask(UserTask task)
        {
            UserTask prevTask = (UserTask)CurrTask.Clone();
            Task_SubTask_Mapper.Add(task.Id, prevTask);
            CurrTask = task;
            PrevTask = prevTask;
            NotifyPropertyChanged(nameof(CurrTask));
            NotifyPropertyChanged(nameof(PrevTask));
        }

        public void ReturnToPrevTask()
        {
            UserTask prevTask = Task_SubTask_Mapper.GetValueOrDefault(PrevTask.Id);
            Task_SubTask_Mapper.Remove(CurrTask.Id);
            CurrTask = (UserTask)PrevTask.Clone();
            PrevTask = prevTask != null ? (UserTask)prevTask.Clone() : null;
            NotifyPropertyChanged(nameof(CurrTask));
            NotifyPropertyChanged(nameof(PrevTask));
        }

        public async Task OnAttachmentAddition(AddAttachmentEvent addAttachmentEvent)
        {
            if (addAttachmentEvent != null && addAttachmentEvent.Task.Id == CurrTask.Id)
            {
                if (addAttachmentEvent.Attachment != null && CurrTask.Attachments.Where(att => att.Id == addAttachmentEvent.Attachment.Id).Count() <= 0)
                {
                    CurrTask.Attachments.Add(addAttachmentEvent.Attachment);
                }
            }
            NotifyPropertyChanged(nameof(CurrTask));
        }

        public ObservableCollection<Attachment> GetAttachmentsForTask(long taskId)
        {
            return attachmentDataHandler.GetAllAttachmentsForTask(taskId);
        }

        public void ClearPrevState()
        {
            Task_SubTask_Mapper = new Dictionary<long, UserTask>();
            PrevTask = null;
            NotifyPropertyChanged(nameof(PrevTask));
        }

        public override void Dispose()
        {
            ViewmodelEventHandler.Instance.Unsubscribe<AddTaskEvent>(OnTaskAddition);
            ViewmodelEventHandler.Instance.Unsubscribe<UpdateTaskEvent>(OnTaskUpdation);
            ViewmodelEventHandler.Instance.Unsubscribe<AddAttachmentEvent>(OnAttachmentAddition);
        }
    }
}
