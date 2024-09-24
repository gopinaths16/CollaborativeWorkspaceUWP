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
    public sealed partial class OrganizationListView : UserControl
    {
        OrganizationListViewViewModel organizationListViewViewModel;

        public OrganizationListView()
        {
            this.InitializeComponent();

            organizationListViewViewModel = new OrganizationListViewViewModel();
        }

        private void OpenAddOrgDialog_Click(object sender, RoutedEventArgs e)
        {
            organizationListViewViewModel.IsAddOrgContextTriggered = !organizationListViewViewModel.IsAddOrgContextTriggered;
            Name.Focus(FocusState.Programmatic);
        }

        public void Clear()
        {
            organizationListViewViewModel.IsAddOrgContextTriggered = false;
        }

        private void Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            AddOrganizationButton.IsEnabled = Name.Text != string.Empty;
        }

        private void AddOrganizationButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CloseAddOrgDialog_Click(object sender, RoutedEventArgs e)
        {
            organizationListViewViewModel.IsAddOrgContextTriggered = !organizationListViewViewModel.IsAddOrgContextTriggered;
        }
    }
}
