using CollaborativeWorkspaceUWP.Utilities;
using CollaborativeWorkspaceUWP.Utilities.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    public sealed partial class SettingsControl : UserControl
    {
        public SettingsControl()
        {
            this.InitializeComponent();

            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            string requestedTheme = localSettings.Values["RequestedTheme"] as string;

            this.RequestedTheme = requestedTheme != null && requestedTheme != string.Empty ? requestedTheme == "Dark" ? ElementTheme.Dark : ElementTheme.Light : ElementTheme.Default;

            ViewmodelEventHandler.Instance.Subscribe<ThemeChangedEvent>(OnThemeChange);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SettingsNavigationItem.SelectedItem = General;
        }

        public async Task OnThemeChange(ThemeChangedEvent e)
        {
            if (e != null && e.Theme != null && e.Theme != string.Empty)
            {
                this.RequestedTheme = e.Theme == "Dark" ? ElementTheme.Dark : ElementTheme.Light;
            }
        }
    }
}
