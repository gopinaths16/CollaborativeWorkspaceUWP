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
    public sealed partial class AddGroupdControl : UserControl
    {
        AddGroupViewModel addGroupViewModel;

        public long ProjectId
        {
            get { return (long)GetValue(ProjectIdProperty); }
            set { SetValue(ProjectIdProperty, value); }
        }
        public long BoardGroupId
        {
            get { return (long)GetValue(BoardGroupIdProperty); }
            set { SetValue(BoardGroupIdProperty, value); }
        }

        public bool IsBoardGroupContext
        {
            get { return (bool)GetValue(IsBoardGroupContextProperty); }
            set { SetValue(IsBoardGroupContextProperty, value); }
        }

        public static readonly DependencyProperty ProjectIdProperty = DependencyProperty.Register("ProjectId", typeof(long), typeof(AddGroupdControl), new PropertyMetadata(-1L));

        public static readonly DependencyProperty BoardGroupIdProperty = DependencyProperty.Register("BoardGroupId", typeof(long), typeof(AddGroupdControl), new PropertyMetadata(-1L));

        public static readonly DependencyProperty IsBoardGroupContextProperty = DependencyProperty.Register("IsBoardGroupContext", typeof(bool), typeof(AddGroupdControl), new PropertyMetadata(false));

        private RoutedEventHandler cancelButtonClickEventHandler;

        public event RoutedEventHandler CancelButtonClick
        {
            add { cancelButtonClickEventHandler += value; }
            remove { cancelButtonClickEventHandler -= value; }
        }

        public AddGroupdControl()
        {
            this.InitializeComponent();

            addGroupViewModel = new AddGroupViewModel();
        }

        private void Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(Name.Text.Length > 0)
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (ProjectId != -1)
            {
                addGroupViewModel.ProjectId = ProjectId;
                addGroupViewModel.BoardGroupId = BoardGroupId;
                addGroupViewModel.IsBoardGroupContext = IsBoardGroupContext;
            }
        }

        private async void AddGroupButton_Click(object sender, RoutedEventArgs e)
        {
            await addGroupViewModel.AddBoardGroup(Name.Text);
            Name.Text = string.Empty;
        }
    }
}
