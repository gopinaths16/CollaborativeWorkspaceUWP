using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
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
    public sealed partial class TaskDetailsControl : UserControl
    {
        TaskDetailsViewModel taskDetailsViewModel;
        AddTaskViewModel addTaskViewModel;

        private RoutedEventHandler _onOpenInSeparateWindow;

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

        public bool IsSeparateWindow
        {
            get { return (bool)GetValue(IsSeparateWindowProperty); }
            set { SetValue(IsSeparateWindowProperty, value); }
        }

        public event RoutedEventHandler OnOpenInSeparateWindow
        {
            add { _onOpenInSeparateWindow += value; }
            remove { _onOpenInSeparateWindow -= value; }
        }

        public static readonly DependencyProperty PriorityComboBoxSourceProperty = DependencyProperty.Register("PriorityComboBoxSource", typeof(object), typeof(TaskDetailsControl), new PropertyMetadata(0));

        public static readonly DependencyProperty StatusComboBoxSourceProperty = DependencyProperty.Register("StatusComboBoxSource", typeof(object), typeof(TaskDetailsControl), new PropertyMetadata(0));

        public static readonly DependencyProperty IsSeparateWindowProperty = DependencyProperty.Register("IsSeparateWindow", typeof(bool), typeof(TaskDetailsControl), new PropertyMetadata(0));

        public TaskDetailsControl()
        {
            this.InitializeComponent();

            taskDetailsViewModel = new TaskDetailsViewModel();
            addTaskViewModel = new AddTaskViewModel();
        }

        public void SetCurrentTask(UserTask task)
        {
            taskDetailsViewModel.CurrTask = task;
        }

        public UserTask GetCurrentTask()
        {
            return taskDetailsViewModel.CurrTask;
        }

        public void UpdateCurrentTask()
        {
            taskDetailsViewModel.UpdateTask();
        }

        private void OpenAddSubTaskWindowButton_ButtonClick(object sender, RoutedEventArgs e)
        {
            taskDetailsViewModel.IsAddSubTaskContextTriggered = true;
        }

        private void AddSubTaskDialog_CancelButtonClick(object sender, RoutedEventArgs e)
        {
            taskDetailsViewModel.IsAddSubTaskContextTriggered = false;
        }

        private void TaskUpdateTriggered(object sender, SelectionChangedEventArgs e)
        {
            taskDetailsViewModel.UpdateTask(true);
        }

        private void TaskDetailsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (checkBox.Tag != null)
            {
                long taskId = (long)checkBox.Tag;
                bool checkboxStatus = (bool)checkBox.IsChecked;
                taskDetailsViewModel.UpdateTaskCompletionStatus(taskId, !checkboxStatus);
            }
        }

        private void SubTaskListCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (checkBox.Tag != null)
            {
                long taskId = (long)checkBox.Tag;
                bool checkboxStatus = (bool)checkBox.IsChecked;
                taskDetailsViewModel.UpdateSubTaskCompletionStatus(taskId, !checkboxStatus);
            }
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            taskDetailsViewModel.DeleteTask();
        }

        private void TaskDetailsChanged(object sender, TextChangedEventArgs e)
        {
            taskDetailsViewModel.SetTaskUpdatedContext();
        }

        private void TaskUpdate(object sender, RoutedEventArgs e)
        {
            taskDetailsViewModel.UpdateTask();
        }

        private void AddSubTaskFromDialogButton_ButtonClick(object sender, RoutedEventArgs e)
        {
            UserTask task = new UserTask();

            task.Name = AddSubTaskDialog.TaskName;
            task.Description = AddSubTaskDialog.TaskDescription;
            task.Status = AddSubTaskDialog.TaskStatus.Id;
            task.Priority = AddSubTaskDialog.TaskPriority.Id;
            task.ProjectId = taskDetailsViewModel.CurrTask.ProjectId;
            task.OwnerId = 0;
            task.AssigneeId = 0;
            task.ParentTaskId = taskDetailsViewModel.CurrTask.Id;

            addTaskViewModel.AddTask(task);

            taskDetailsViewModel.IsAddSubTaskContextTriggered = false;

            ClearAllFields();
        }

        public void DeleteSubTaskButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            long taskId = (long)button.Tag;
            taskDetailsViewModel.DeleteSubTask(taskId);
        }

        public void ClearAllFields()
        {
            AddSubTaskDialog.ClearAllFields();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            taskDetailsViewModel.Dispose();
        }
    }
}
