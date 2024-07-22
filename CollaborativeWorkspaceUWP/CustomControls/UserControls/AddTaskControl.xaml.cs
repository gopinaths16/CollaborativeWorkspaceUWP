using CollaborativeWorkspaceUWP.Models;
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
    public sealed partial class AddTaskControl : UserControl
    {
        private RoutedEventHandler cancelButtonClickEventHandler;
        private RoutedEventHandler addTaskButtonClickEventHandler;

        public object PriorityComboBoxSource
        {
            get { return (object)GetValue(PriorityComboBoxSourceProperty); }
            set { SetValue(PriorityComboBoxSourceProperty, value); }
        }

        public object StatusComboBoxSource
        {
            get { return (object)GetValue(StatusComboBoxSourceProperty); }
            set { SetValue(StatusComboBoxSourceProperty, value); }
        }

        public event RoutedEventHandler CancelButtonClick
        {
            add { cancelButtonClickEventHandler += value; }
            remove { cancelButtonClickEventHandler -= value; }
        }

        public event RoutedEventHandler AddTaskButtonClick
        {
            add { addTaskButtonClickEventHandler += value; }
            remove { addTaskButtonClickEventHandler -= value; }
        }

        public string TaskName
        {
            get
            {
                return Name.Text;
            }
        }

        public string TaskDescription
        {
            get
            {
                return Description.Text;
            }
        }

        public Status TaskStatus
        {
            get
            {
                return (Status)Status.SelectedItem;
            }
        }

        public Priority TaskPriority
        {
            get
            {
                return (Priority)Priority.SelectedItem;
            }
        }

        public static readonly DependencyProperty PriorityComboBoxSourceProperty = DependencyProperty.Register("PriorityComboBoxSource", typeof(object), typeof(AddTaskControl), new PropertyMetadata(0));

        public static readonly DependencyProperty StatusComboBoxSourceProperty = DependencyProperty.Register("StatusComboBoxSource", typeof(object), typeof(AddTaskControl), new PropertyMetadata(0));

        public AddTaskControl()
        {
            this.InitializeComponent();
        }

        private void AddTaskFromDialogButton_ButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void AddTaskDialogTaskName_TextChanged(object sender, TextChangedEventArgs e)
        {
            AddTaskFromDialogButton.IsEnabled = Name.Text.Length > 0 ? true : false;
        }

        private void CloseTaskDialogButton_Click(object sender, RoutedEventArgs e)
        {

        }

        public void ClearAllFields()
        {
            Name.Text = string.Empty;
            Description.Text = string.Empty;
            Priority.SelectedIndex = 0;
            Status.SelectedIndex = 0;
        }
    }
}
