using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
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
    public sealed partial class BoardViewControl : UserControl
    {
        BoardGroupViewModel boardGroupViewModel;

        public BoardViewControl()
        {
            this.InitializeComponent();

            boardGroupViewModel = new BoardGroupViewModel();
        }

        public async Task SetBoardGroup(Group boardGroup)
        {
            BoardListView.ItemsSource = null;
            boardGroupViewModel.BoardGroup = null;
            await Task.Delay(50);
            boardGroupViewModel.IsLoading = true;
            boardGroupViewModel.BoardGroup = boardGroup;
            BoardListView.ItemsSource = boardGroupViewModel.BoardGroup.Boards;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
