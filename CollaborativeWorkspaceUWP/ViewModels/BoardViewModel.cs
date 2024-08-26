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
    public class BoardViewModel : BaseViewModel
    {
        private Group currBoard;
        private bool isAddTaskContextTriggered;

        private TaskDataHandler taskDataHandler;

        public Group CurrBoard
        {
            get { return currBoard; }
            set
            {
                currBoard = value;
                if(currBoard != null && currBoard.Tasks.Count <= 0)
                {
                    currBoard.Tasks = taskDataHandler.GetTasksForGroup(currBoard.Id);
                }
                NotifyPropertyChanged(nameof(CurrBoard));
            }
        }

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

            ViewmodelEventHandler.Instance.Subscribe<AddTaskEvent>(OnTaskAddition);
        }

        public async Task OnTaskAddition(AddTaskEvent e)
        {
            if(e.Task != null && currBoard != null)
            {
                if(e.Task.GroupId ==  currBoard.Id)
                {
                    currBoard.Tasks.Add(e.Task);
                    NotifyPropertyChanged(nameof(CurrBoard));
                }
            }
        }
    }
}
