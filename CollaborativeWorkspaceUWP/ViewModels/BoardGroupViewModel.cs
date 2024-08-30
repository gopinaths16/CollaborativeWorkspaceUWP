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
    public class BoardGroupViewModel : BaseViewModel
    {
        private GroupDataHandler groupDataHandler;

        private Group boardGroup;
        private bool isLoading;

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
                if(boardGroup != null && (boardGroup.Boards == null || boardGroup.Boards.Count <= 0))
                {
                    boardGroup.Boards = groupDataHandler.GetBoardsForBoardGroup(boardGroup.Id);
                }
                NotifyPropertyChanged(nameof(BoardGroup));
                BoardGroup.NotifyChangesToEntity();
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

        public async Task OnBoardAddition(AddGroupEvent e)
        {
            if(BoardGroup != null)
            {
                if(e.Group != null && e.Group.BoardGroupId == BoardGroup.Id)
                {
                    BoardGroup.Boards.Add(e.Group);
                    NotifyPropertyChanged(nameof(BoardGroup));
                }
            }
        }
    }
}
