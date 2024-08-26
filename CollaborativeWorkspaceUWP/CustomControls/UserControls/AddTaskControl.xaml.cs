using CollaborativeWorkspaceUWP.Auth.Handlers;
using CollaborativeWorkspaceUWP.Models;
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
    public sealed partial class AddTaskControl : UserControl
    {
        private AddTaskViewModel addTaskViewModel;

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

        public long CurrProjectId
        {
            get { return (long)GetValue(CurrProjectIdProperty); }
            set { 
                SetValue(CurrProjectIdProperty, value);
                if(addTaskViewModel != null)
                {
                    addTaskViewModel.ProjectId = (long)GetValue(CurrProjectIdProperty);
                }
            }
        }

        public long ParentTaskId
        {
            get { return (long)GetValue(ParentTaskIdProperty); }
            set { SetValue(ParentTaskIdProperty, value); }
        }

        public long GroupId
        {
            get { return (long)GetValue(GroupIdProperty); }
            set { SetValue(GroupIdProperty, value); }
        }

        public static readonly DependencyProperty PriorityComboBoxSourceProperty = DependencyProperty.Register("PriorityComboBoxSource", typeof(object), typeof(AddTaskControl), new PropertyMetadata(null));

        public static readonly DependencyProperty StatusComboBoxSourceProperty = DependencyProperty.Register("StatusComboBoxSource", typeof(object), typeof(AddTaskControl), new PropertyMetadata(null));

        public static readonly DependencyProperty CurrProjectIdProperty = DependencyProperty.Register("CurrProjectId", typeof(long), typeof(AddTaskControl), new PropertyMetadata(-1L));

        public static readonly DependencyProperty ParentTaskIdProperty = DependencyProperty.Register("ParentTaskId", typeof(long), typeof(AddTaskControl), new PropertyMetadata(-1L));

        public static readonly DependencyProperty GroupIdProperty = DependencyProperty.Register("GroupId", typeof(long), typeof(AddTaskControl), new PropertyMetadata(-1L));

        public AddTaskControl()
        {
            this.InitializeComponent();

            addTaskViewModel = new AddTaskViewModel();
        }

        private async void AddTaskFromDialogButton_ButtonClick(object sender, RoutedEventArgs e)
        {
            UserTask task = new UserTask();

            task.Name = Name.Text;
            task.Description = Description.Text;
            task.Status = ((Status)Status.SelectedItem).Id;
            task.Priority = ((Priority)Priority.SelectedItem).Id;
            task.ProjectId = CurrProjectId;
            task.OwnerId = UserSessionHandler.Instance.CurrUser.Id;
            task.AssigneeId = 0;
            task.ParentTaskId = ParentTaskId > 0 ? ParentTaskId : -1;
            task.GroupId = GroupId;
            if(DueDatePicker.Date != null)
            {
                task.DueDate = DueDatePicker.Date.Value.DateTime;
            }
            await addTaskViewModel.AddTask(task);

            addTaskButtonClickEventHandler?.Invoke(sender, e);

            ClearAllFields();
        }

        private void AddTaskDialogTaskName_TextChanged(object sender, TextChangedEventArgs e)
        {
            AddTaskFromDialogButton.IsEnabled = Name.Text.Length > 0 ? true : false;
        }

        private void CloseTaskDialogButton_Click(object sender, RoutedEventArgs e)
        {
            cancelButtonClickEventHandler?.Invoke(sender, e);
            ClearAllFields();
        }

        public void Focus()
        {
            Name.Focus(FocusState.Programmatic);
        }

        public void ClearAllFields()
        {
            Name.Text = string.Empty;
            Description.Text = string.Empty;
            Priority.SelectedIndex = 0;
            Status.SelectedIndex = 0;
            DueDatePicker.Date = null;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            addTaskViewModel.Dispose();
        }
    }
}
