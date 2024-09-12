using CollaborativeWorkspaceUWP.DAL;
using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Models.Providers.Boards;
using CollaborativeWorkspaceUWP.Utilities;
using CollaborativeWorkspaceUWP.Utilities.Custom;
using CollaborativeWorkspaceUWP.Utilities.Events;
using CollaborativeWorkspaceUWP.Views.ViewObjects.Boards;
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

        public IncrementalLoadingCollection<IBoardItem> BoardItems;

        private BoardProvider boardProvider;

        public ICollection<IBoardItem> MovedTask { get; set; }

        private TaskDataHandler taskDataHandler;

        public Group CurrBoard
        {
            get { return currBoard; }
            set
            {
                currBoard = value;
                NotifyPropertyChanged(nameof(CurrBoard));
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

        public string BoardItemsCount
        {
            get
            {
                if(BoardItems.Count > 0)
                {
                    return "(" + BoardItems.Count.ToString() + ")";
                }
                return "";
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
            IsOpen = true;

            taskDataHandler = new TaskDataHandler();

            BoardItems = new IncrementalLoadingCollection<IBoardItem>(new ObservableCollection<IBoardItem>(), 8);

            ViewmodelEventHandler.Instance.Subscribe<MoveBoardItemEvent>(OnMovingBoardItem);
        }

        public async Task OnTaskAddition(AddTaskEvent e)
        {
            if(e.Task != null && currBoard != null)
            {
                if(e.Task.GroupId ==  currBoard.Id)
                {
                    BoardItems.Add(e.Task);
                    NotifyPropertyChanged(nameof(BoardItemsCount));
                }
            }
        }

        public async Task UpdateDraggedTask(ICollection<IBoardItem> boardItems)
        {
            foreach(IBoardItem boardItem in boardItems)
            {
                if (boardItem.GroupId != CurrBoard.Id)
                {
                    BoardItems.Insert(0, boardItem);
                }
            }
            await ViewmodelEventHandler.Instance.Publish(new MoveBoardItemEvent() { BoardItems = boardItems });
        }

        public async Task OnMovingBoardItem(MoveBoardItemEvent e)
        {
            if(e != null && e.BoardItems != null && MovedTask != null && e.BoardItems.Count > 0)
            {
                foreach (var task in e.BoardItems)
                {
                    BoardItems.Remove(task);
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
