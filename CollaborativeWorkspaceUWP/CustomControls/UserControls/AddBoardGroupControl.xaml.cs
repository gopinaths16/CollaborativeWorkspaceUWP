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
    public sealed partial class AddBoardGroupControl : UserControl
    {
        AddBoardViewModel addBoardViewModel;

        public long ProjectId
        {
            get { return (long)GetValue(ProjectIdProperty); }
            set { SetValue(ProjectIdProperty, value); }
        }

        public static readonly DependencyProperty ProjectIdProperty = DependencyProperty.Register("ProjectId", typeof(long), typeof(AddBoardGroupControl), new PropertyMetadata(-1));

        private RoutedEventHandler cancelButtonClickEventHandler;

        public event RoutedEventHandler CancelButtonClick
        {
            add { cancelButtonClickEventHandler += value; }
            remove { cancelButtonClickEventHandler -= value; }
        }

        public AddBoardGroupControl()
        {
            this.InitializeComponent();

            addBoardViewModel = new AddBoardViewModel();
        }

        private void Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(Name.Text.Length > 0)
            {
                AddBoardButton.IsEnabled = true;
            }
            else
            {
                AddBoardButton.IsEnabled = false;
            }
        }

        private async void AddBoardButton_Click(object sender, RoutedEventArgs e)
        {
            await addBoardViewModel.AddBoardGroup(Name.Text);
            Name.Text = string.Empty;
        }

        private void CloseDialogButton_Click(object sender, RoutedEventArgs e)
        {
            cancelButtonClickEventHandler?.Invoke(sender, e);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (ProjectId != -1)
            {
                addBoardViewModel.ProjectId = ProjectId;
            }
        }
    }
}
