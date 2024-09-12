using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Models.Providers.Boards;
using CollaborativeWorkspaceUWP.Utilities.Custom;
using CollaborativeWorkspaceUWP.ViewModels;
using CollaborativeWorkspaceUWP.Views.ViewObjects.Boards;
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
    public sealed partial class BoardView : UserControl
    {
        BoardviewViewModel boardviewViewModel;

        public BoardView()
        {
            this.InitializeComponent();

            boardviewViewModel = new BoardviewViewModel();
        }

        public string GroupName
        {
            get { return (string)GetValue(GroupNameProperty); }
            set { SetValue(GroupNameProperty, value); }
        }

        public DataTemplate BoardTemplate
        {
            get { return (DataTemplate)GetValue(BoardTemplateProperty); }
            set { SetValue(BoardTemplateProperty, value); }
        }

        public BoardProvider BoardProvider
        {
            get { return (BoardProvider)GetValue(BoardProviderProperty); }
            set { SetValue(BoardProviderProperty, value); }
        }

        public ICollection<BoardProvider> DefaultBoardProviders
        {
            get { return (ICollection<BoardProvider>)GetValue(DefaultBoardProvidersProperty); }
            set { SetValue(DefaultBoardProvidersProperty, value); }
        }

        public bool IsDefaultBoardContext
        {
            get { return (bool)GetValue(IsDefaultBoardContextProperty); }
            set { SetValue(IsDefaultBoardContextProperty, value); }
        }

        public static readonly DependencyProperty GroupNameProperty = DependencyProperty.Register("GroupName", typeof(string), typeof(BoardView), new PropertyMetadata(""));

        public static readonly DependencyProperty BoardTemplateProperty = DependencyProperty.Register("BoardTemplate", typeof(DataTemplate), typeof(BoardView), new PropertyMetadata(null));

        public static readonly DependencyProperty BoardProviderProperty = DependencyProperty.Register("BoardProvider", typeof(BoardProvider), typeof(BoardView), new PropertyMetadata(null));

        public static readonly DependencyProperty DefaultBoardProvidersProperty = DependencyProperty.Register("DefaultBoardProviders", typeof(ICollection<BoardProvider>), typeof(BoardView), new PropertyMetadata(new List<BoardProvider>()));

        public static readonly DependencyProperty IsDefaultBoardContextProperty = DependencyProperty.Register("IsDefaultBoardContext", typeof(bool), typeof(BoardView), new PropertyMetadata(false));

        private RoutedEventHandler cancelButtonClickEventHandler;

        private RoutedEventHandler addBoardClickEventHandler;

        public event RoutedEventHandler OnBoardAddition
        {
            add { addBoardClickEventHandler += value; }
            remove { addBoardClickEventHandler -= value; }
        }

        public string BoardName
        {
            get
            {
                return Name.Text;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void BoardListView_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            BoardListView.CanReorderItems = true;
        }

        private void BoardListView_DragItemsCompleted(ListViewBase sender, DragItemsCompletedEventArgs args)
        {
            BoardListView.CanReorderItems = false;
        }

        private void Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Name.Text.Length > 0)
            {
                AddGroupButton.IsEnabled = true;
            }
            else
            {
                AddGroupButton.IsEnabled = false;
            }
        }

        private void CloseDialogButton_Click(object sender, RoutedEventArgs e)
        {
            AddBoardFlyout.Hide();
            Clear();
        }

        private async void AddGroupButton_Click(object sender, RoutedEventArgs e)
        {
           addBoardClickEventHandler?.Invoke(sender, e);
           AddBoardFlyout.Hide();
        }

        public void SetBoardProvider(BoardProvider boardProvider)
        {
            if(boardProvider != null)
            {
                BoardProvider = boardProvider;
                boardviewViewModel.BoardViewId = boardProvider.GetBoardViewId();
            }
        }

        public void LoadBoards()
        {
            boardviewViewModel.Boards.Clear();
            ICollection<IBoard> boards = BoardProvider.GetBoards();
            if (boards != null && boards.Count() > 0)
            {
                NoBoardAvailableMessage.Visibility = Visibility.Collapsed;
                foreach (var board in boards)
                {
                    boardviewViewModel.Boards.Add(board);
                }
            }
            else
            {
                NoBoardAvailableMessage.Visibility = Visibility.Visible;
            }
        }

        public void SetDefaultBoardProviders(ICollection<BoardProvider> boardProviders)
        {
            DefaultBoardProviders.Clear();
            DefaultBoardProviders = boardProviders;
            DefaultProviderComboBox.ItemsSource = boardProviders;
            DefaultProviderComboBox.SelectedIndex = 0;
        }

        public void Clear()
        {
            Name.Text = string.Empty;
        }

        private void DefaultProviderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetBoardProvider(DefaultProviderComboBox.SelectedItem as BoardProvider);
            Task.Run(async () =>
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    LoadBoards();
                });
            });

        }
    }
}
