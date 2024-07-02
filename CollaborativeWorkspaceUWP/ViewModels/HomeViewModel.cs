using CollaborativeWorkspaceUWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CollaborativeWorkspaceUWP.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private bool isTaskWindowOpen;

        public bool IsTaskWindowOpen
        {
            get => isTaskWindowOpen;
            set { isTaskWindowOpen = value; }
        }

        public HomeViewModel()
        {
            IsTaskWindowOpen = false;
        }
    }
}
