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
    public class BoardGroupViewModel : BaseViewModel
    {
        private GroupDataHandler groupDataHandler;

        private Group boardGroup;
        private bool isLoading;
        private IncrementalLoadingCollection<Group> boards;

        public bool IsLoading
        {
            get { return isLoading; }
            set
            {
                isLoading = value;
                NotifyPropertyChanged(nameof(IsLoading));
            }
        }

        public long ProjectId { get; set; }
        public long BoardGroupId { get; set; }
        public bool IsBoardGroupContext { get; set; }


        public Group BoardGroup
        {
            get { return boardGroup; }
            set
            {
                boardGroup = value;
                NotifyPropertyChanged(nameof(BoardGroup));
            }
        }

        public IncrementalLoadingCollection<Group> Boards
        {
            get { return boards; }
            set
            {
                boards = value;
                NotifyPropertyChanged(nameof(Boards));
            }
        }

        public BoardGroupViewModel()
        {
            groupDataHandler = new GroupDataHandler();
            IsLoading = false;
            ViewmodelEventHandler.Instance.Subscribe<AddGroupEvent>(OnBoardAddition);
        }

        public async Task AddBoardGroup(string name)
        {
            Group group = groupDataHandler.AddBoardGroupForProject(name, ProjectId, BoardGroupId, IsBoardGroupContext);
            await ViewmodelEventHandler.Instance.Publish(new AddGroupEvent() { Group = group });
        }

        public void GetBoards()
        {
            if (BoardGroup != null)
            {
                Boards = new IncrementalLoadingCollection<Group>(groupDataHandler.GetBoardsForBoardGroup(boardGroup.Id), 4);
            }
        }

        public async Task OnBoardAddition(AddGroupEvent e)
        {
            if(BoardGroup != null)
            {
                if(e.Group != null && e.Group.BoardGroupId == BoardGroup.Id)
                {
                    Boards.Add(e.Group);
                    NotifyPropertyChanged(nameof(BoardGroup));
                }
            }
        }
    }
}
