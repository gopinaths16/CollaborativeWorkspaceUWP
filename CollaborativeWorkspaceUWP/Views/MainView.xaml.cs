using CollaborativeWorkspaceUWP.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;
using CollaborativeWorkspaceUWP.Models;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CollaborativeWorkspaceUWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomeView : Page
    {

        MainViewModel mainViewModel;

        public HomeView()
        {
            this.InitializeComponent();

            mainViewModel = new MainViewModel();

            this.DataContext = mainViewModel;

            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            Window.Current.SetTitleBar(TitleBar);
        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await SelectOrgDialog.ShowAsync();
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

        private void AllOrganizationsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            Organization organization = (Organization)e.ClickedItem;
            mainViewModel.SetCurrOrganization(organization);
            SelectOrganizationCombobox.SelectedItem = organization;
            SelectOrgDialog.Hide();
            HomeViewFrame.Navigate(typeof(TaskView), this.DataContext);
        }

        private void AddNewOrganizationButton_ButtonClick(object sender, RoutedEventArgs e)
        {
            mainViewModel.SetAddOrganizationContext();
        }

        private void GoBackToAllOrganizationsButton_Click(object sender, RoutedEventArgs e)
        {
            mainViewModel.SetSelectOrganizationContext();
        }

        private void AddOrganizationButton_ButtonClick(object sender, RoutedEventArgs e)
        {
            mainViewModel.AddNewOrganization(OrgName.Text);
            mainViewModel.SetSelectOrganizationContext();
        }
    }
}
