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
using static System.Net.WebRequestMethods;
using Windows.UI.Xaml.Media.Animation;
using System.Xml.Linq;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CollaborativeWorkspaceUWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainView : Page
    {

        MainViewModel mainViewModel;

        public MainView()
        {
            this.InitializeComponent();

            mainViewModel = new MainViewModel();
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = false;
            this.DataContext = mainViewModel;
        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if(mainViewModel.Organizations.Count > 0)
            {
                mainViewModel.SetCurrOrganization(mainViewModel.Organizations[0]);
                SelectOrganizationCombobox.SelectedItem = mainViewModel.Organizations[0];
                NavigationBar.SelectedItem = TaskViewNavigationItem;
                HomeViewFrame.Navigate(typeof(TaskView), this.DataContext, new SuppressNavigationTransitionInfo());
            }
            else
            {
                await SelectOrgDialog.ShowAsync();
            }
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

        private void SelectOrganizationCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedOrgIndex = SelectOrganizationCombobox.SelectedIndex;
            mainViewModel.SetCurrOrganization(mainViewModel.Organizations[selectedOrgIndex]);
        }

        private void NavigationBar_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (NavigationBar.SelectedItem == TaskViewNavigationItem)
            {
                HomeViewFrame.Navigate(typeof(TaskView), this.DataContext, new SuppressNavigationTransitionInfo());
            }
            else if(NavigationBar.SelectedItem == SprintViewNavigationItem)
            {
                HomeViewFrame.Navigate(typeof(SprintView), this.DataContext, new SuppressNavigationTransitionInfo());
            }
        }

        private void AccountDetailsNavigationItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            UIElement element = sender as UIElement;
            GeneralTransform transform = element.TransformToVisual(Window.Current.Content);
            Point point = transform.TransformPoint(new Point(0, 0));

            AccountDetailsPopup.HorizontalOffset = point.X + element.RenderSize.Width;
            AccountDetailsPopup.VerticalOffset = point.Y;
            AccountDetailsPopup.IsOpen = true;
        }
    }
}
