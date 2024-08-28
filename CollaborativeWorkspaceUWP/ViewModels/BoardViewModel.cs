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
    public class BoardViewModel : BaseViewModel
    {
        private Group currBoard;
        private bool isAddTaskContextTriggered;

        public UserTask MovedTask { get; set; }

        private TaskDataHandler taskDataHandler;

        public Group CurrBoard
        {
            get { return currBoard; }
            set
            {
                currBoard = value;
                NotifyPropertyChanged(nameof(CurrBoard));
                if (currBoard != null && currBoard.Tasks.Count <= 0)
                {
                    Tasks = taskDataHandler.GetTasksForGroup(currBoard.Id);
                    NotifyPropertyChanged(nameof(Tasks));
                    NotifyPropertyChanged(nameof(TaskCount));
                }
            }
        }

        public string TaskCount
        {
            get
            {
                if(Tasks.Count > 0)
                {
                    return "(" + Tasks.Count.ToString() + ")";
                }
                return "";
            }
        }

        public ObservableCollection<UserTask> Tasks { get; set; }

        public bool IsAddTaskContextTriggered
        {
            get { return isAddTaskContextTriggered; }
            set
            {
                isAddTaskContextTriggered = value;
                NotifyPropertyChanged(nameof(IsAddTaskContextTriggered));
            }
        }

        public BoardViewModel()
        {
            IsAddTaskContextTriggered = false;

            taskDataHandler = new TaskDataHandler();
            Tasks = new ObservableCollection<UserTask>();

            ViewmodelEventHandler.Instance.Subscribe<AddTaskEvent>(OnTaskAddition);
            ViewmodelEventHandler.Instance.Subscribe<MoveTaskEvent>(OnMovingTask);
        }

        public async Task OnTaskAddition(AddTaskEvent e)
        {
            if(e.Task != null && currBoard != null)
            {
                if(e.Task.GroupId ==  currBoard.Id)
                {
                    Tasks.Add(e.Task);
                    NotifyPropertyChanged(nameof(TaskCount));
                }
            }
        }

        public async Task UpdateDraggedTask(UserTask task)
        {
            if(task.GroupId != CurrBoard.Id)
            {
                task.GroupId = CurrBoard.Id;
                taskDataHandler.UpdateGroupIdForTask(task);
                Tasks.Insert(0, task);
                NotifyPropertyChanged(nameof(TaskCount));
                await ViewmodelEventHandler.Instance.Publish(new MoveTaskEvent() { Task = task });
            }
        }

        public async Task OnMovingTask(MoveTaskEvent e)
        {
            if(e != null && e.Task != null && MovedTask != null && e.Task.Id == MovedTask.Id)
            {
                Tasks.Remove(Tasks.Where(item => item.Id == e.Task.Id).FirstOrDefault());
                MovedTask = null;
                NotifyPropertyChanged(nameof(CurrBoard));
            }
        }
    }
}
