﻿using CollaborativeWorkspaceUWP.DAL;
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

        public Group BoardGroup
        {
            get { return boardGroup; }
            set
            {
                boardGroup = value;
                if(boardGroup.Boards.Count <= 0)
                {
                    boardGroup.Boards = groupDataHandler.GetBoardsForBoardGroup(boardGroup.Id);
                }
                NotifyPropertyChanged(nameof(BoardGroup));
            }
        }

        public BoardGroupViewModel()
        {
            groupDataHandler = new GroupDataHandler();

            ViewmodelEventHandler.Instance.Subscribe<AddGroupEvent>(OnBoardAddition);
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
