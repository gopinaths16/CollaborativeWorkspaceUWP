using CollaborativeWorkspaceUWP.Models.Comm;
using CollaborativeWorkspaceUWP.Utilities.Comm;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CollaborativeWorkspaceUWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomeView : Page
    {
        HomeViewModel taskWindowViewModel;

        public HomeView()
        {
            this.InitializeComponent();
            taskWindowViewModel = new HomeViewModel();
            ViewModelEventManager.Instance.Subscribe<TaskWindowOpenEventArgs>(OnTaskWindowOpen);
            ViewModelEventManager.Instance.Subscribe<AddTaskWindowOpenEvent>(OnAddNewTaskPopupOpen);
            this.DataContext = taskWindowViewModel;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            HomeViewFrame.Navigate(typeof(OrganizationView));
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            HomeViewFrame.Navigate(typeof(OrganizationView));
        }

        private void ProjectViewButton_Click(object sender, RoutedEventArgs e)
        {
            HomeViewFrame.Navigate(typeof(ProjectView));
        }

        private void TaskViewButton_Click(object sender, RoutedEventArgs e)
        {
            HomeViewFrame.Navigate(typeof(TaskView));
        }

        private void SprintViewButton_Click(object sender, RoutedEventArgs e)
        {
            HomeViewFrame.Navigate(typeof(SprintView));
        }

        private void ClosePopupClicked(object sender, RoutedEventArgs e)
        {
            if (AddNewTaskPopup.IsOpen) { AddNewTaskPopup.IsOpen = false; }
        }

        private void OnTaskWindowOpen(TaskWindowOpenEventArgs e)
        {
            SplitViewFrame.Navigate(typeof(CurrentTaskView));
        }

        private void OnAddNewTaskPopupOpen(AddTaskWindowOpenEvent e)
        {
            SplitViewFrame.Navigate(typeof(AddTaskView));
        }

        private void CustomIconButtonControl_ButtonClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
