using CollaborativeWorkspaceUWP.Models.Comm;
using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Utilities.Comm;
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
            ViewModelEventManager.Instance.Subscribe<TaskWindowOpenEventArgs>(OnTaskWindowOpen);
            ViewModelEventManager.Instance.Subscribe<AddTaskWindowOpenEvent>(OnAddNewTaskPopupOpen);
        }

        private void OnTaskWindowOpen(TaskWindowOpenEventArgs e)
        {
            IsTaskWindowOpen = e.IsWindowOpen;
            NotifyPropertyChanged(nameof(IsTaskWindowOpen));
        }

        private void OnAddNewTaskPopupOpen(AddTaskWindowOpenEvent e)
        {
            IsTaskWindowOpen = e.IsWindowOpen;
            NotifyPropertyChanged(nameof(IsTaskWindowOpen));
        }
    }
}
