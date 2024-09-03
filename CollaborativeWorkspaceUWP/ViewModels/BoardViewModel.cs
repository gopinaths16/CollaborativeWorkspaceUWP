using CollaborativeWorkspaceUWP.DAL;
using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Utilities;
using CollaborativeWorkspaceUWP.Utilities.Custom;
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
        private bool isOpen;

        public List<UserTask> MovedTask { get; set; }

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
                    Tasks = new IncrementalLoadingCollection<UserTask>(taskDataHandler.GetTasksForGroup(currBoard.Id), 8);
                    NotifyPropertyChanged(nameof(Tasks));
                    NotifyPropertyChanged(nameof(TaskCount));
                }
            }
        }

        public bool IsOpen
        {
            get { return isOpen; }
            set
            {
                isOpen = value;
                NotifyPropertyChanged(nameof(IsOpen));
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

        public IncrementalLoadingCollection<UserTask> Tasks { get; set; }

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
            IsOpen = true;
            Tasks = new IncrementalLoadingCollection<UserTask>(new ObservableCollection<UserTask>(), 8);

            taskDataHandler = new TaskDataHandler();

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

        public async Task UpdateDraggedTask(ICollection<UserTask> tasks)
        {
            foreach(var taskItem in tasks)
            {
                if (taskItem.GroupId != CurrBoard.Id)
                {
                    taskItem.GroupId = CurrBoard.Id;
                    taskDataHandler.UpdateGroupIdForTask(taskItem);
                    Tasks.Insert(0, taskItem);
                    NotifyPropertyChanged(nameof(TaskCount));
                }
            }
            await ViewmodelEventHandler.Instance.Publish(new MoveTaskEvent() { Tasks = tasks });
        }

        public async Task OnMovingTask(MoveTaskEvent e)
        {
            if(e != null && e.Tasks != null && MovedTask != null && e.Tasks.Count > 0)
            {
                foreach (var task in e.Tasks)
                {
                    Tasks.Remove(Tasks.Where(item => item.Id == task.Id).FirstOrDefault());
                }
                MovedTask = null;
                NotifyPropertyChanged(nameof(CurrBoard));
            }
        }

        public void NotifyUI(object property)
        {
            NotifyPropertyChanged(nameof(property));
        }
    }
}
