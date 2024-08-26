using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CollaborativeWorkspaceUWP.CustomControls.UserControls
{
    public sealed partial class BoardControl : UserControl
    {
        BoardViewModel boardViewModel;

        public Group Board
        {
            get { return (Group)GetValue(BoardProperty); }
            set { SetValue(BoardProperty, value); }
        }

        public static readonly DependencyProperty BoardProperty = DependencyProperty.Register("Board", typeof(Group), typeof(BoardControl), new PropertyMetadata(null));

        public BoardControl()
        {
            this.InitializeComponent();

            boardViewModel = new BoardViewModel();
        }

        private void AddTaskDialog_AddTaskButtonClick(object sender, RoutedEventArgs e)
        {
            boardViewModel.IsAddTaskContextTriggered = !boardViewModel.IsAddTaskContextTriggered;
        }

        private void TaskListView_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(Board != null)
            {
                boardViewModel.CurrBoard = Board;
            }
        }
    }
}
