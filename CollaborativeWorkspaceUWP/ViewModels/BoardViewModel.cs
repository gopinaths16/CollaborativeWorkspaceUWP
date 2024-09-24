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
    public class BoardViewModel : BaseViewModel, IDisposable
    {
        private Group currBoard;
        private bool isAddBoardItemContextTriggered;
        private bool isOpen;
        private string boardboardItemsCount;

        public IBoardItemProvider BoardItemProvider;

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
                if(BoardItems.SourceCount > 0)
                {
                    return "(" + BoardItems.SourceCount.ToString() + ")";
                }
                return "";
            }
        }

        public bool IsAddBoardItemContextTriggered
        {
            get { return isAddBoardItemContextTriggered; }
            set
            {
                isAddBoardItemContextTriggered = value;
                NotifyPropertyChanged(nameof(IsAddBoardItemContextTriggered));
            }
        }

        public BoardViewModel()
        {
            IsAddBoardItemContextTriggered = false;
            IsOpen = true;

            taskDataHandler = new TaskDataHandler();

            BoardItems = new IncrementalLoadingCollection<IBoardItem>(new ObservableCollection<IBoardItem>(), 8);

            ViewmodelEventHandler.Instance.Subscribe<MoveBoardItemEvent>(OnMovingBoardItem);
            ViewmodelEventHandler.Instance.Subscribe<AddBoardItemEvent>(OnBoardItemAddition);
            ViewmodelEventHandler.Instance.Subscribe<UpdateBoardItemEvent>(OnBoardItemUpdation);
        }

        public async Task UpdateDraggedTask(ICollection<IBoardItem> boardItems)
        {
            foreach(IBoardItem boardItem in BoardItemProvider.UpdateBoardItems(boardItems))
            {
                BoardItems.Insert(0, boardItem);
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
                await BoardItems.LoadMoreItemsAsync();
                NotifyPropertyChanged(nameof(CurrBoard));
                NotifyPropertyChanged(nameof(BoardItems));
                NotifyPropertyChanged(nameof(BoardItemsCount));
            }
        }

        public async Task OnBoardItemAddition(AddBoardItemEvent e)
        {
            if(e.BoardItem != null && e.BoardItem.GroupId == CurrBoard.Id)
            {
                if(BoardItemProvider.DoesItemBelongToBoard(e.BoardItem))
                {
                    BoardItems.Add(e.BoardItem);
                    NotifyPropertyChanged(nameof(BoardItemsCount));
                    NotifyPropertyChanged(nameof(BoardItems));
                }
            }
        }

        public async Task OnBoardItemUpdation(UpdateBoardItemEvent e)
        {
            if (e.BoardItem != null)
            {
                await BoardItemProvider.UpdateSource(e.BoardItem, BoardItems);
                await BoardItems.LoadMoreItemsAsync();
                NotifyPropertyChanged(nameof(BoardItemsCount));
                NotifyPropertyChanged(nameof(BoardItems));
            }
        }

        public void NotifyUI()
        {
            NotifyPropertyChanged(nameof(BoardItems));
            NotifyPropertyChanged(nameof(BoardItemsCount));
        }

        public void Dispose()
        {
            ViewmodelEventHandler.Instance.Unsubscribe<MoveBoardItemEvent>(OnMovingBoardItem);
            ViewmodelEventHandler.Instance.Unsubscribe<AddBoardItemEvent>(OnBoardItemAddition);
            ViewmodelEventHandler.Instance.Unsubscribe<UpdateBoardItemEvent>(OnBoardItemUpdation);
        }
    }
}
