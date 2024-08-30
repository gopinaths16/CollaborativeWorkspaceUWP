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

        public DataTemplate BoardItemTemplate
        {
            get { return (DataTemplate)GetValue(BoardItemTemplateProperty); }
            set { SetValue(BoardItemTemplateProperty, value); }
        }

        public static readonly DependencyProperty BoardProperty = DependencyProperty.Register("Board", typeof(Group), typeof(BoardControl), new PropertyMetadata(null));

        public static readonly DependencyProperty BoardItemTemplateProperty = DependencyProperty.Register("BoardItemTemplate", typeof(DataTemplate), typeof(BoardControl), new PropertyMetadata(null));

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
            e.Data.Properties.Add("Task", draggedTask);
            boardViewModel.MovedTask = draggedTask;
            e.Data.RequestedOperation = DataPackageOperation.Move;
        }

        private void TaskListViewByGroup_DragItemsCompleted(ListViewBase sender, DragItemsCompletedEventArgs args)
        {
        }

        private async void TaskListViewByGroup_DragOver(object sender, DragEventArgs e)
        {
            e.DataView.Properties.TryGetValue("Task", out object draggedTask);
            e.AcceptedOperation = draggedTask != null && draggedTask is UserTask && (draggedTask as UserTask).GroupId != boardViewModel.CurrBoard.Id ? DataPackageOperation.Move : DataPackageOperation.None;
        }

        private async void TaskListViewByGroup_Drop(object sender, DragEventArgs e)
        {
            e.DataView.Properties.TryGetValue("Task", out object draggedTask);
            if (draggedTask != null && draggedTask is UserTask)
            {
                await boardViewModel.UpdateDraggedTask(draggedTask as UserTask);
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            boardViewModel.IsOpen = !boardViewModel.IsOpen;
            if(boardViewModel.IsOpen)
            {
                MinimizeButton.Content = "\ue740";
                TaskListView.ColumnDefinitions.Clear();
                TaskListView.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                TaskListView.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                TaskListView.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            }
            else
            {
                MinimizeButton.Content = "\ue73f";
                TaskListView.ColumnDefinitions.Clear();
                TaskListView.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                TaskListView.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                TaskListView.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            }
        }
    }
}
