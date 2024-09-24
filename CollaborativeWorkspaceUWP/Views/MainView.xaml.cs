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
using CollaborativeWorkspaceUWP.Utilities;
using CollaborativeWorkspaceUWP.Utilities.Events;
using System.Threading.Tasks;
using CollaborativeWorkspaceUWP.Auth.Handlers;
using CollaborativeWorkspaceUWP.CustomControls.UserControls;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml.Hosting;
using Windows.Storage;

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

            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            string requestedTheme = localSettings.Values["RequestedTheme"] as string;

            var titleBar = ApplicationView.GetForCurrentView().TitleBar;

            this.RequestedTheme = requestedTheme != null && requestedTheme != string.Empty ? requestedTheme == "Dark" ? ElementTheme.Dark : ElementTheme.Light : ElementTheme.Default;

            titleBar.BackgroundColor = requestedTheme == "Dark" ? (Color)(Application.Current.Resources["TitleBarDarkBackgroundColor"]) : (Color)(Application.Current.Resources["SystemAccentColor"]);
            titleBar.ButtonBackgroundColor = requestedTheme == "Dark" ? (Color)(Application.Current.Resources["TitleBarDarkBackgroundColor"]) : (Color)(Application.Current.Resources["SystemAccentColor"]);
            titleBar.InactiveBackgroundColor = requestedTheme == "Dark" ? (Color)(Application.Current.Resources["TitleBarDarkBackgroundColor"]) : (Color)(Application.Current.Resources["SystemAccentColor"]);
            titleBar.ButtonInactiveBackgroundColor = requestedTheme == "Dark" ? (Color)(Application.Current.Resources["TitleBarDarkBackgroundColor"]) : (Color)(Application.Current.Resources["SystemAccentColor"]);

            mainViewModel = new MainViewModel();
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = false;
            this.DataContext = mainViewModel;
            ViewmodelEventHandler.Instance.Subscribe<LogoutEvent>(OnLogoutTriggered);
        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if(mainViewModel.Organizations.Count > 0)
            {
                mainViewModel.SetCurrOrganization(mainViewModel.Organizations[0]);
                Organization.Content = mainViewModel.Organizations[0].Name;
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
            //int selectedOrgIndex = SelectOrganizationCombobox.SelectedIndex;
            //mainViewModel.SetCurrOrganization(mainViewModel.Organizations[selectedOrgIndex]);
        }

        private void AccountDetailsNavigationItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            PersonPicture personPicture = sender as PersonPicture;
            if (personPicture.ContextFlyout != null)
            {
                var element = sender as FrameworkElement;
                personPicture.ContextFlyout.ShowAt(element, new FlyoutShowOptions
                {
                    Placement = FlyoutPlacementMode.BottomEdgeAlignedLeft,
                    ShowMode = FlyoutShowMode.Standard
                });
            }
        }

        public async Task OnLogoutTriggered(LogoutEvent logoutEvent)
        {
            if(logoutEvent != null && UserSessionHandler.Instance.Logout())
            {
                Frame rootFrame = Window.Current.Content as Frame;
                if (rootFrame.Content != null)
                {
                    rootFrame.Navigate(typeof(UserOnboardView), null);
                }
            }
        }

        private async void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            string requestedTheme = localSettings.Values["RequestedTheme"] as string;

            AppWindow appWindow;
            Grid newWindowLayout = new Grid();
            newWindowLayout.Style = SettingsControlStyle;
            SettingsControl settingsControl = new SettingsControl();
            settingsControl.RequestedTheme = requestedTheme != null && requestedTheme != string.Empty ? requestedTheme == "Dark" ? ElementTheme.Dark : ElementTheme.Light : ElementTheme.Default;
            newWindowLayout.Children.Add(settingsControl);
            appWindow = await AppWindow.TryCreateAsync();
            appWindow.RequestSize(new Size(1100, 600));
            ElementCompositionPreview.SetAppWindowContent(appWindow, newWindowLayout);
            await appWindow.TryShowAsync();
            appWindow.Closed += delegate
            {
                appWindow = null;
            };
        }

        private void NavigationBar_ItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            if (NavigationBar.SelectedItem == TaskViewNavigationItem)
            {
                HomeViewFrame.Navigate(typeof(TaskView), null, new SuppressNavigationTransitionInfo());
            }
        }

        private void AccountDetailsControl_OnThemeChange(object sender, RoutedEventArgs e)
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            AccountFlyout.Hide();
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            if (this.RequestedTheme == ElementTheme.Dark)
            {
                this.RequestedTheme = ElementTheme.Light;
                localSettings.Values["RequestedTheme"] = ElementTheme.Light.ToString();
                titleBar.BackgroundColor = (Color)(Application.Current.Resources["SystemAccentColor"]);
                titleBar.ButtonBackgroundColor = (Color)(Application.Current.Resources["SystemAccentColor"]);
                titleBar.InactiveBackgroundColor = (Color)(Application.Current.Resources["SystemAccentColor"]);
                titleBar.ButtonInactiveBackgroundColor = (Color)(Application.Current.Resources["SystemAccentColor"]);
            }
            else
            {
                this.RequestedTheme = ElementTheme.Dark;
                localSettings.Values["RequestedTheme"] = ElementTheme.Dark.ToString();
                titleBar.BackgroundColor = (Color)(Application.Current.Resources["TitleBarDarkBackgroundColor"]);
                titleBar.ButtonBackgroundColor = (Color)(Application.Current.Resources["TitleBarDarkBackgroundColor"]);
                titleBar.InactiveBackgroundColor = (Color)(Application.Current.Resources["TitleBarDarkBackgroundColor"]);
                titleBar.ButtonInactiveBackgroundColor = (Color)(Application.Current.Resources["TitleBarDarkBackgroundColor"]);
            }
        }
    }
}
