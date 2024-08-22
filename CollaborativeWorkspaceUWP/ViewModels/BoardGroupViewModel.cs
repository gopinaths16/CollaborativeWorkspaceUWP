using CollaborativeWorkspaceUWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.ViewModels
{
    public class BoardGroupViewModel : BaseViewModel
    {

        private BoardGroup boardGroup;

        public BoardGroup BoardGroup
        {
            get { return boardGroup; }
            set
            {
                boardGroup = value;
                NotifyPropertyChanged(nameof(BoardGroup));
            }
        }

    }
}
