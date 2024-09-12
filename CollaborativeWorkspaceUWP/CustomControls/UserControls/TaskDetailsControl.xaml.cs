using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
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
        private RoutedEventHandler _onTaskClear;

        public bool IsSeparateWindow
        {
            get { return (bool)GetValue(IsSeparateWindowProperty); }
            set { SetValue(IsSeparateWindowProperty, value); }
        }

        public bool AllowTaskClear
        {
            get { return (bool)GetValue(AllowTaskClearProperty); }
            set { SetValue(AllowTaskClearProperty, value); }
        }

        public event RoutedEventHandler OnOpenInSeparateWindow
        {
            add { _onOpenInSeparateWindow += value; }
            remove { _onOpenInSeparateWindow -= value; }
        }

        public event RoutedEventHandler OnTaskClear
        {
            add { _onTaskClear += value; }
            remove { _onTaskClear -= value; }
        }

        public static readonly DependencyProperty IsSeparateWindowProperty = DependencyProperty.Register("IsSeparateWindow", typeof(bool), typeof(TaskDetailsControl), new PropertyMetadata(false));

        public static readonly DependencyProperty AllowTaskClearProperty = DependencyProperty.Register("AllowTaskClear", typeof(bool), typeof(TaskDetailsControl), new PropertyMetadata(false));

        public TaskDetailsControl()
        {
            this.InitializeComponent();

            taskDetailsViewModel = new TaskDetailsViewModel();
            addTaskViewModel = new AddTaskViewModel();
        }

        public void SetCurrentTask(UserTask task)
        {
            Status.SelectionChanged -= TaskUpdateTriggered;
            Priority.SelectionChanged -= TaskUpdateTriggered;

            taskDetailsViewModel.CurrTask = task;
            taskDetailsViewModel.ClearPrevState();
            if(task != null)
            {
                task.Attachments = taskDetailsViewModel.CurrTask.Attachments;
            }
            AttachmentDialog.SetCurrTask(null);
            CommentDialog.SetCurrTask(task);
            TaskViewPivot.SelectedIndex = 0;

            Status.SelectionChanged += TaskUpdateTriggered;
            Priority.SelectionChanged += TaskUpdateTriggered;
        }

        public UserTask GetCurrentTask()
        {
            return taskDetailsViewModel.CurrTask;
        }

        public async Task UpdateCurrentTask()
        {
            await taskDetailsViewModel.UpdateTask();
        }

        private void OpenAddSubTaskWindowButton_ButtonClick(object sender, RoutedEventArgs e)
        {
            taskDetailsViewModel.IsAddSubTaskContextTriggered = true;
            AddSubTaskDialog.Focus();
        }

        private void AddSubTaskDialog_CancelButtonClick(object sender, RoutedEventArgs e)
        {
            taskDetailsViewModel.IsAddSubTaskContextTriggered = false;
        }

        private async void TaskUpdateTriggered(object sender, SelectionChangedEventArgs e)
        {
            Status status = (Status)Status.SelectedItem;
            Priority priority = (Priority)Priority.SelectedItem;

            if(status != null && priority != null)
            {
                long taskId = (long)TaskCompletionCheckBox.Tag;
                if (taskId == taskDetailsViewModel.CurrTask.Id && !taskDetailsViewModel.IsSubTaskSelected && (status.Id != taskDetailsViewModel.CurrTask.Status || priority.Id != taskDetailsViewModel.CurrTask.Priority || TaskCompletionCheckBox.IsChecked != taskDetailsViewModel.CurrTask.IsCompleted))
                {
                    taskDetailsViewModel.CurrTask.Status = status.Id;
                    taskDetailsViewModel.CurrTask.Priority = priority.Id;
                    await taskDetailsViewModel.UpdateTask(true);
                }
                taskDetailsViewModel.IsSubTaskSelected = false;
            }
        }

        private async void TaskDetailsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (checkBox.Tag != null)
            {
                long taskId = (long)checkBox.Tag;
                bool checkboxStatus = (bool)checkBox.IsChecked;
                await taskDetailsViewModel.UpdateTaskCompletionStatus(taskId, !checkboxStatus);
            }
        }

        private async void SubTaskListCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (checkBox.Tag != null)
            {
                long taskId = (long)checkBox.Tag;
                bool checkboxStatus = (bool)checkBox.IsChecked;
                await taskDetailsViewModel.UpdateSubTaskCompletionStatus(taskId, !checkboxStatus);
            }
        }

        private async void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            await taskDetailsViewModel.DeleteTask();
        }

        private void TaskDetailsChanged(object sender, TextChangedEventArgs e)
        {
            if(TaskTitle.Text != taskDetailsViewModel.CurrTask.Name || TaskDescription.Text != taskDetailsViewModel.CurrTask.Description)
            {
                taskDetailsViewModel.CurrTask.Name = TaskTitle.Text;
                taskDetailsViewModel.CurrTask.Description = TaskDescription.Text;
                taskDetailsViewModel.SetTaskUpdatedContext();
            }
        }

        private async void TaskUpdate(object sender, RoutedEventArgs e)
        {
            await taskDetailsViewModel.UpdateTask();
        }

        private void AddSubTaskFromDialogButton_ButtonClick(object sender, RoutedEventArgs e)
        {
            taskDetailsViewModel.IsAddSubTaskContextTriggered = false;
        }

        public async void DeleteSubTaskButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            long taskId = (long)button.Tag;
            await taskDetailsViewModel.DeleteSubTask(taskId);
        }

        public void ClearAllFields()
        {
            AddSubTaskDialog.ClearAllFields();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            taskDetailsViewModel.Dispose();
        }

        private void AddAttachmentControl_CancelButtonClickEventHandler(object sender, RoutedEventArgs e)
        {
            taskDetailsViewModel.IsAddAttachmentContextTriggered = false;
        }

        private void AddAttachmentDialog_AddButtonClick(object sender, RoutedEventArgs e)
        {
            taskDetailsViewModel.IsAddAttachmentContextTriggered = false;
        }

        private async void AddAttachmentsButton_Click(object sender, RoutedEventArgs e)
        {
            AttachmentDialog.SetCurrTask(taskDetailsViewModel.CurrTask);
            AttachmentDialog.Visibility = Visibility.Visible;
            await AttachmentDialog.PickAndAddAttachment();
            TaskViewPivot.SelectedIndex = 2;
        }

        private void TaskViewPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var pivot = sender as Pivot;
            if (pivot == null) return;

            foreach (var item in pivot.Items)
            {
                var pivotItem = item as PivotItem;
                if (pivotItem == null) continue;

                if(pivot.SelectedItem == pivotItem)
                {
                    if(pivotItem.Name == "AttachementsPivotItem")
                    {
                        AttachmentDialog.SetCurrTask(taskDetailsViewModel.CurrTask);
                        AttachmentDialog.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private void SubTaskListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            taskDetailsViewModel.IsSubTaskSelected = true;
            taskDetailsViewModel.SetSubTaskToCurrTask((UserTask)e.ClickedItem);
            AttachmentDialog.SetCurrTask(null);
            CommentDialog.SetCurrTask(taskDetailsViewModel.CurrTask);
            TaskViewPivot.SelectedIndex = 0;
        }

        private void GoToPrevTaskButton_Click(object sender, RoutedEventArgs e)
        {
            taskDetailsViewModel.IsSubTaskSelected = true;
            taskDetailsViewModel.ReturnToPrevTask();
            AttachmentDialog.SetCurrTask(null);
            CommentDialog.SetCurrTask(taskDetailsViewModel.CurrTask);
            TaskViewPivot.SelectedIndex = 0;
        }

        private void PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Control control = (Control)sender;
            control.BorderBrush = new SolidColorBrush(Colors.Gray);
        }

        private void PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Control control = (Control)sender;
            control.BorderBrush = new SolidColorBrush(Colors.Transparent);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            taskDetailsViewModel.IsLoaeded = true;
        }
    }
}
