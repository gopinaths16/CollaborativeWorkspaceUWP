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
using System.Numerics;
using Windows.UI.Xaml.Media.Animation;
using static System.Net.Mime.MediaTypeNames;
using Windows.UI.Core;
using System.Threading.Tasks;
using CollaborativeWorkspaceUWP.Views.ViewObjects.Boards;
using CollaborativeWorkspaceUWP.Utilities.Custom;
using System.Collections.ObjectModel;
using Microsoft.Toolkit.Uwp;
using System.Collections;
using CollaborativeWorkspaceUWP.Models.Providers.Boards;
using CollaborativeWorkspaceUWP.Utilities;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CollaborativeWorkspaceUWP.CustomControls.UserControls
{
    public sealed partial class Board : UserControl
    {
        BoardViewModel boardViewModel;

        public Group BoardSource
        {
            get { return (Group)GetValue(BoardSourcedProperty); }
            set { SetValue(BoardSourcedProperty, value); }
        }
        
        public DataTemplate BoardItemTemplate
        {
            get { return (DataTemplate)GetValue(BoardItemTemplateProperty); }
            set { SetValue(BoardItemTemplateProperty, value); }
        }

        public IBoardItemProvider BoardItemProvider
        {
            get { return (IBoardItemProvider)GetValue(BoardItemProviderProperty); }
            set { SetValue(BoardItemProviderProperty, value); }
        }

        public IBoardAdder AddBoardItemControl
        {
            get { return (IBoardAdder)GetValue(AddBoardItemControlProperty); }
            set { SetValue(AddBoardItemControlProperty, value); }
        }

        public static readonly DependencyProperty BoardSourcedProperty = DependencyProperty.Register("BoardSource", typeof(Group), typeof(Board), new PropertyMetadata(null));

        public static readonly DependencyProperty BoardItemTemplateProperty = DependencyProperty.Register("BoardItemTemplate", typeof(DataTemplate), typeof(Board), new PropertyMetadata(null));

        public static readonly DependencyProperty BoardItemProviderProperty = DependencyProperty.Register("BoardItemProvider", typeof(IBoardItemProvider), typeof(Board), new PropertyMetadata(null));

        public static readonly DependencyProperty AddBoardItemControlProperty = DependencyProperty.Register("AddBoardItemControl", typeof(IBoardAdder), typeof(Board), new PropertyMetadata(null));

        public Board()
        {
            this.InitializeComponent();

            boardViewModel = new BoardViewModel();
        }

        private void TaskListView_ItemClick(object sender, ItemClickEventArgs e)
        {
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (BoardSource != null)
            {
                boardViewModel.CurrBoard = BoardSource;
                this.DataContext = boardViewModel.CurrBoard;
                boardViewModel.BoardItemProvider = BoardItemProvider;
                if(boardViewModel.CurrBoard.ColorCode != null && boardViewModel.CurrBoard.ColorCode != string.Empty)
                {
                    Separator.Stroke = Util.CreateBrushFromHex(boardViewModel.CurrBoard.ColorCode);
                }
                if(BoardItemProvider.GetDefaultArgs().Count() > 0)
                {
                    AddBoardItemControl.SetDefaultArgs(BoardItemProvider.GetDefaultArgs());
                }
                Task.Run(async () =>
                {
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        LoadBoardItems();
                    });
                });
            }
        }

        private void TaskListViewByGroup_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            ICollection<IBoardItem> draggedTasks = new List<IBoardItem>();
            foreach (IBoardItem task in e.Items)
            {
                draggedTasks.Add(task);
            }
            e.Data.Properties.Add("Item", draggedTasks);
            boardViewModel.MovedTask = draggedTasks;
            e.Data.RequestedOperation = DataPackageOperation.Move;
        }

        private void TaskListViewByGroup_DragItemsCompleted(ListViewBase sender, DragItemsCompletedEventArgs args)
        {
        }

        private async void TaskListViewByGroup_DragOver(object sender, DragEventArgs e)
        {
            e.DataView.Properties.TryGetValue("Item", out object draggedTasks);
            e.AcceptedOperation = draggedTasks != null && draggedTasks is ICollection<IBoardItem> && (draggedTasks as ICollection<IBoardItem>).FirstOrDefault() != null && !BoardItemProvider.DoesItemBelongToBoard((draggedTasks as ICollection<IBoardItem>).FirstOrDefault()) ? DataPackageOperation.Move : DataPackageOperation.None;
        }

        private async void TaskListViewByGroup_Drop(object sender, DragEventArgs e)
        {
            e.DataView.Properties.TryGetValue("Item", out object draggedItems);
            if (draggedItems != null && draggedItems is ICollection<IBoardItem>)
            {
                await boardViewModel.UpdateDraggedTask(draggedItems as ICollection<IBoardItem>);
            }
            boardViewModel.NotifyUI();
        }

        public void LoadBoardItems()
        {
            if(BoardItemProvider != null)
            {
                ICollection<IBoardItem> boardItems = BoardItemProvider.GetBoardItems(BoardSource.Id, BoardSource.ProjectId);
                if (boardItems != null && boardItems.Count > 0)
                {
                    foreach (IBoardItem item in boardItems)
                    {
                        boardViewModel.BoardItems.Add(item);
                    }
                    boardViewModel.NotifyUI();
                }
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            boardViewModel.IsOpen = !boardViewModel.IsOpen;
            if (boardViewModel.IsOpen)
            {
                MinimizeButton.Content = "\ue740";
                TaskListView.ColumnDefinitions.Clear();
                TaskListView.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                TaskListView.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
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
                TaskListView.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            }
        }

        private void AddBoardItemButton_Click(object sender, RoutedEventArgs e)
        {
            boardViewModel.IsAddBoardItemContextTriggered = !boardViewModel.IsAddBoardItemContextTriggered;
            if(boardViewModel.IsAddBoardItemContextTriggered)
            {
                AddBoardItemControl.Focus();
            }
            if (!boardViewModel.IsAddBoardItemContextTriggered)
            {
                AddBoardItemControl.ClearAllFields();
            }
        }
    }
}
