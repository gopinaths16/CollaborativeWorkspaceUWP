using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using System.Text.Json;

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
            AddTaskDialog.Focus();
        }

        private void TaskListView_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(Board != null)
            {
                boardViewModel.CurrBoard = Board;
                this.DataContext = boardViewModel.CurrBoard;
            }
        }

        private void TaskListViewByGroup_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            UserTask draggedTask = e.Items.FirstOrDefault() as UserTask;
            boardViewModel.MovedTask = draggedTask;
            string jsonData = JsonSerializer.Serialize(draggedTask);
            e.Data.SetText(jsonData);
            e.Data.RequestedOperation = DataPackageOperation.Move;
        }

        private void TaskListViewByGroup_DragItemsCompleted(ListViewBase sender, DragItemsCompletedEventArgs args)
        {
        }

        private async void TaskListViewByGroup_DragOver(object sender, DragEventArgs e)
        {
            UserTask draggedTask = JsonSerializer.Deserialize<UserTask>(await e.DataView.GetTextAsync());
            e.AcceptedOperation = draggedTask.GroupId != boardViewModel.CurrBoard.Id ? DataPackageOperation.Move : DataPackageOperation.None;
        }

        private async void TaskListViewByGroup_Drop(object sender, DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.Text))
            {
                string draggedItem = await e.DataView.GetTextAsync();
                UserTask draggedTask = JsonSerializer.Deserialize<UserTask>(draggedItem);
                await boardViewModel.UpdateDraggedTask(draggedTask);
            }
        }
    }
}
