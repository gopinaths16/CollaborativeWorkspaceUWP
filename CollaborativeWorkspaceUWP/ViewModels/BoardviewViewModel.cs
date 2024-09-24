using CollaborativeWorkspaceUWP.Models.Providers.Boards;
using CollaborativeWorkspaceUWP.Utilities;
using CollaborativeWorkspaceUWP.Utilities.Custom;
using CollaborativeWorkspaceUWP.Utilities.Events;
using CollaborativeWorkspaceUWP.Views.ViewObjects.Boards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.ViewModels
{
    public class BoardviewViewModel : BaseViewModel
    {
        public long ProjectId { get; set; }
        public long BoardViewId { get; set; }

        public BoardProvider BoardProvider { get; set; }

        public IncrementalLoadingCollection<IBoard> Boards { get; set; }

        public BoardviewViewModel()
        {
            Boards = new IncrementalLoadingCollection<IBoard>(new System.Collections.ObjectModel.ObservableCollection<IBoard>(), 4);

            ViewmodelEventHandler.Instance.Subscribe<AddBoardEvent>(OnBoardAddition);
        }

        public async Task OnBoardAddition(AddBoardEvent e)
        {
            if (e.Board != null && e.Id == BoardViewId)
            {
                IBoardItemProvider boardItemProvider = BoardProvider.GetBoardItemProvider();
                boardItemProvider.BoardId = e.Board.Id;
                e.Board.BoardItemProvider = boardItemProvider;
                Boards.Add(e.Board);
                NotifyPropertyChanged(nameof(Boards));
            }
        }
    }
}
