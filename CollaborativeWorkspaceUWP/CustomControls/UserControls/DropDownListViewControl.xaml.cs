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
    public sealed partial class DropDownListViewControl : UserControl
    {
        private ItemClickEventHandler _itemClickEventHandler;
        private RoutedEventHandler _itemAddEventHandler;

        public DataTemplate ListViewItemTemplate
        {
            get { return (DataTemplate)GetValue(ListViewItemTemplateProperty); }
            set { SetValue(ListViewItemTemplateProperty, value); }
        }

        public Object ListViewItemSource
        {
            get { return (Object)GetValue(ListViewItemSourceProperty); }
            set { SetValue(ListViewItemSourceProperty, value); }
        }

        public string DropdownTitle
        {
            get { return (string)GetValue(DropdownTitleProperty); }
            set { SetValue(DropdownTitleProperty, value); }
        }

        public bool AllowAdditionToListView
        {
            get { return (bool)GetValue(AllowAdditionToListViewProperty); }
            set { SetValue(AllowAdditionToListViewProperty, value); }
        }

        public event ItemClickEventHandler ListViewItemClicked
        {
            add { _itemClickEventHandler += value; }
            remove { _itemClickEventHandler -= value; }
        }

        public event RoutedEventHandler OnItemAddClick
        {
            add { _itemAddEventHandler += value; }
            remove { _itemAddEventHandler -= value; }
        }

        public static readonly DependencyProperty ListViewItemTemplateProperty = DependencyProperty.Register("ListViewItemTemplate", typeof(DataTemplate), typeof(DropDownListViewControl), null);
        
        public static readonly DependencyProperty ListViewItemSourceProperty = DependencyProperty.Register("ListViewItemSource", typeof(Object), typeof(DropDownListViewControl), null);

        public static readonly DependencyProperty DropdownTitleProperty = DependencyProperty.Register("DropdownTitle", typeof(string), typeof(DropDownListViewControl), null);

        public static readonly DependencyProperty AllowAdditionToListViewProperty = DependencyProperty.Register("AllowAdditionToListView", typeof(bool), typeof(DropDownListViewControl), null);

        public DropDownListViewControl()
        {
            this.InitializeComponent();
        }

        private void OpenDropdownButton_Click(object sender, RoutedEventArgs e)
        {
            if (DropdownListView.Visibility == Visibility.Collapsed)
            {
                OpenDropdownButton.Content = "\uE70D";
                DropdownListView.Visibility = Visibility.Visible;
            }
            else
            {
                OpenDropdownButton.Content = "\uE76C";
                DropdownListView.Visibility = Visibility.Collapsed;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            OpenDropdownButton.Content = "\uE70D";
            DropdownListView.Visibility = Visibility.Visible;
        }
    }
}
