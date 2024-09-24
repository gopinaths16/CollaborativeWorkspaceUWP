using CollaborativeWorkspaceUWP.Utilities;
using CollaborativeWorkspaceUWP.Utilities.Events;
using CollaborativeWorkspaceUWP.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class AccountDetailsControl : UserControl
    {
        AccountDetailsViewModel accountDetailsViewModel;

        private RoutedEventHandler themeChangeEventHandler;

        public AccountDetailsControl()
        {
            this.InitializeComponent();
            accountDetailsViewModel = new AccountDetailsViewModel();
            this.DataContext = accountDetailsViewModel;
        }

        public event RoutedEventHandler OnThemeChange
        {
            add { themeChangeEventHandler += value; }
            remove { themeChangeEventHandler -= value; }
        }

        private async void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            await accountDetailsViewModel.Logout();
        }

        private async void ChangeThemeButton_Click(object sender, RoutedEventArgs e)
        {
            themeChangeEventHandler?.Invoke(sender, e);
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            string requestedTheme = localSettings.Values["RequestedTheme"] as string;
            await accountDetailsViewModel.ChangeTheme(requestedTheme);
        }
    }
}
