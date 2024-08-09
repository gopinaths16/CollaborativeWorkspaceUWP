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

        public AccountDetailsControl()
        {
            this.InitializeComponent();
            accountDetailsViewModel = new AccountDetailsViewModel();
            this.DataContext = accountDetailsViewModel;
        }

        private async void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            await ViewmodelEventHandler.Instance.Publish(new LogoutEvent());
        }
    }
}
