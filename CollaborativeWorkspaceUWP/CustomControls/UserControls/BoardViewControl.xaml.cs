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
        public BoardViewControl()
        {
            this.InitializeComponent();
        }

        public DataTemplate BoardTemplate
        {
            get { return (DataTemplate)GetValue(BoardTemplateProperty); }
            set { SetValue(BoardTemplateProperty, value); }
        }

        public DataTemplate BoardItemTemplate
        {
            get { return (DataTemplate)GetValue(BoardItemTemplateProperty); }
            set { SetValue(BoardItemTemplateProperty, value); }
        }

        public object BoardSource
        {
            get { return (object)GetValue(BoardSourceProperty); }
            set { SetValue(BoardSourceProperty, value); }
        }

        public string GroupName
        {
            get { return (string)GetValue(GroupNameProperty); }
            set { SetValue(GroupNameProperty, value); }
        }

        public static readonly DependencyProperty BoardTemplateProperty = DependencyProperty.Register("BoardTemplate", typeof(DataTemplate), typeof(BoardViewControl), new PropertyMetadata(null));

        public static readonly DependencyProperty BoardItemTemplateProperty = DependencyProperty.Register("BoardItemTemplate", typeof(DataTemplate), typeof(BoardViewControl), new PropertyMetadata(null));

        public static readonly DependencyProperty BoardSourceProperty = DependencyProperty.Register("BoardSource", typeof(object), typeof(BoardViewControl), new PropertyMetadata(null));

        public static readonly DependencyProperty GroupNameProperty = DependencyProperty.Register("GroupName", typeof(string), typeof(BoardViewControl), new PropertyMetadata(""));

        private RoutedEventHandler cancelButtonClickEventHandler;

        private RoutedEventHandler addBoardClickEventHandler;

        public event RoutedEventHandler OnBoardAddition
        {
            add { addBoardClickEventHandler += value; }
            remove { addBoardClickEventHandler -= value; }
        }

        public event RoutedEventHandler CancelButtonClick
        {
            add { cancelButtonClickEventHandler += value; }
            remove { cancelButtonClickEventHandler -= value; }
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
            cancelButtonClickEventHandler?.Invoke(sender, e);
        }

        private async void AddGroupButton_Click(object sender, RoutedEventArgs e)
        {
           addBoardClickEventHandler?.Invoke(sender, e);
        }

        public void Clear()
        {
            Name.Text = string.Empty;
        }
    }
}
