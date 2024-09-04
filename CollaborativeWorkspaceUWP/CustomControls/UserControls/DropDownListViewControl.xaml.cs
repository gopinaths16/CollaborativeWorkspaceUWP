using System;
using System.Collections;
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

        public Style ListViewItemContainerStyle
        {
            get { return (Style)GetValue(ListViewItemContainerStyleProperty); }
            set { SetValue(ListViewItemContainerStyleProperty, value); }
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

        public bool IsSubList
        {
            get { return (bool)GetValue(IsSubListProperty); }
            set { SetValue(IsSubListProperty, value); }
        }

        public string TitleIcon
        {
            get { return (string)GetValue(TitleIconProperty); }
            set { SetValue(TitleIconProperty, value); }
        }

        public Flyout AddButtonFlyout
        {
            get { return (Flyout)GetValue(AddButtonFlyoutProperty); }
            set { SetValue(AddButtonFlyoutProperty, value); }
        }

        public bool HideContentOnInitialization
        {
            get { return (bool)GetValue(HideContentOnInitializationProperty); }
            set { SetValue(HideContentOnInitializationProperty, value); }
        }

        public bool ListViewItemClickEnabled
        {
            get { return (bool)GetValue(ListViewItemClickEnabledProperty); }
            set { SetValue(ListViewItemClickEnabledProperty, value); }
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

        public string MessageOnSourceEmpty
        {
            get { return (string)GetValue(MessageOnSourceEmptyProperty); }
            set { SetValue(MessageOnSourceEmptyProperty, value); }
        }

        public DataTemplateSelector ListViewItemTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(ListViewItemTemplateSelectorProperty); }
            set { SetValue(ListViewItemTemplateSelectorProperty, value); }
        }

        public static readonly DependencyProperty ListViewItemTemplateProperty = DependencyProperty.Register("ListViewItemTemplate", typeof(DataTemplate), typeof(DropDownListViewControl), null);
        
        public static readonly DependencyProperty ListViewItemSourceProperty = DependencyProperty.Register("ListViewItemSource", typeof(Object), typeof(DropDownListViewControl), null);

        public static readonly DependencyProperty DropdownTitleProperty = DependencyProperty.Register("DropdownTitle", typeof(string), typeof(DropDownListViewControl), null);

        public static readonly DependencyProperty AllowAdditionToListViewProperty = DependencyProperty.Register("AllowAdditionToListView", typeof(bool), typeof(DropDownListViewControl), null);

        public static readonly DependencyProperty MessageOnSourceEmptyProperty = DependencyProperty.Register("MessageOnSourceEmpty", typeof(string), typeof(DropDownListViewControl), null);

        public static readonly DependencyProperty IsSubListProperty = DependencyProperty.Register("IsSubList", typeof(bool), typeof(DropDownListViewControl), new PropertyMetadata(false));

        public static readonly DependencyProperty ListViewItemClickEnabledProperty = DependencyProperty.Register("ListViewItemClickEnabled", typeof(bool), typeof(DropDownListViewControl), new PropertyMetadata(true));

        public static readonly DependencyProperty TitleIconProperty = DependencyProperty.Register("TitleIcon", typeof(string), typeof(DropDownListViewControl), new PropertyMetadata(""));

        public static readonly DependencyProperty HideContentOnInitializationProperty = DependencyProperty.Register("HideContentOnInitialization", typeof(bool), typeof(DropDownListViewControl), new PropertyMetadata(false));

        public static readonly DependencyProperty AddButtonFlyoutProperty = DependencyProperty.Register("AddButtonFlyout", typeof(Flyout), typeof(DropDownListViewControl), new PropertyMetadata(null));

        public static readonly DependencyProperty ListViewItemContainerStyleProperty = DependencyProperty.Register("ListViewItemContainerStyle", typeof(Style), typeof(DropDownListViewControl), new PropertyMetadata(null));

        public static readonly DependencyProperty ListViewItemTemplateSelectorProperty = DependencyProperty.Register("ListViewItemTemplateSelector", typeof(DataTemplateSelector), typeof(DropDownListViewControl), new PropertyMetadata(null));

        public DropDownListViewControl()
        {
            this.InitializeComponent();
        }

        private void OpenDropdownButton_Click(object sender, RoutedEventArgs e)
        {
            if (DropdownListViewContent.Visibility == Visibility.Collapsed)
            {
                OpenDropdownButton.Content = "\uE70D";
                OpenDropdownButtonAlt.Content = "\uE70D";
                DropdownListViewContent.Visibility = Visibility.Visible;
            }
            else
            {
                OpenDropdownButton.Content = "\uE76C";
                OpenDropdownButtonAlt.Content = "\uE76C";
                DropdownListViewContent.Visibility = Visibility.Collapsed;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(!IsSubList)
            {
                DropdownListView.Margin = new Thickness(0, 10, 0, 0);
            }
            DropdownListView.ItemTemplate = ListViewItemTemplate;
            if (HideContentOnInitialization)
            {
                OpenDropdownButton.Content = "\uE76C";
                OpenDropdownButtonAlt.Content = "\uE76C";
                DropdownListViewContent.Visibility = Visibility.Collapsed;
            }
            else
            {
                OpenDropdownButton.Content = "\uE70D";
                OpenDropdownButtonAlt.Content = "\uE70D";
                DropdownListViewContent.Visibility = Visibility.Visible;
            }
            DropdownListView.ItemsSource = ListViewItemSource;
            DropdownListView.ItemContainerStyle = ListViewItemContainerStyle;
            ICollection collection = ListViewItemSource as ICollection;
            if (collection != null && collection.Count <= 0)
            {
                NoSourceAvailableMessage.Visibility = Visibility.Visible;
            }
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddButtonFlyout != null)
            {
                var button = sender as FrameworkElement;
                AddButtonFlyout.ShowAt(button, new FlyoutShowOptions
                {
                    Placement = FlyoutPlacementMode.BottomEdgeAlignedLeft,
                    ShowMode = FlyoutShowMode.Standard
                });
            }
            _itemAddEventHandler?.Invoke(sender, e);
            ICollection collection = ListViewItemSource as ICollection;
            if(collection.Count > 0)
            {
                NoSourceAvailableMessage.Visibility = Visibility.Collapsed;
            }
        }

        public void SetListViewItemTemplate(DataTemplate template)
        {
            DropdownListView.ItemTemplate = template;
        }

        public void SetListViewItemTemplateSelector(DataTemplateSelector selector)
        {
            DropdownListView.ItemTemplateSelector = selector;
        }

        public void SetListViewItemSource(IList source)
        {
            DropdownListView.ItemsSource = source;
        }
    }
}
