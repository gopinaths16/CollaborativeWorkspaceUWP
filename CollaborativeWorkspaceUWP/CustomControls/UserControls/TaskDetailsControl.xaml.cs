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
    public sealed partial class TaskDetailsControl : UserControl
    {
        private SelectionChangedEventHandler _selectionChanged;
        private RoutedEventHandler _taskDetailsCheckboxChecked;
        private RoutedEventHandler _onTaskDelete;
        private RoutedEventHandler _updateTask;
        private TextChangedEventHandler _onTaskDetailsChange;
        private RoutedEventHandler _addSubTask;
        private RoutedEventHandler _deleteSubTask;
        private RoutedEventHandler _subTaskDetailsCheckboxChecked;

        public string SubTaskName
        {
            get
            {
                return AddSubTaskDialog.TaskName;
            }
        }

        public string SubTaskDescription
        {
            get
            {
                return AddSubTaskDialog.TaskDescription;
            }
        }

        public Status SubTaskStatus
        {
            get
            {
                return AddSubTaskDialog.TaskStatus;
            }
        }

        public Priority SubTaskPriority
        {
            get
            {
                return AddSubTaskDialog.TaskPriority;
            }
        }

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

        public UserTask CurrTask
        {
            get { return (UserTask)GetValue(CurrTaskProperty); }
            set { SetValue(CurrTaskProperty, value); }
        }

        public bool IsAddSubTaskContextTriggered
        {
            get { return (bool)GetValue(IsAddSubTaskContextTriggeredProperty); }
            set { SetValue(IsAddSubTaskContextTriggeredProperty, value); }
        }

        public event SelectionChangedEventHandler SelectionChanged
        {
            add { _selectionChanged += value; }
            remove {  _selectionChanged -= value; }
        }

        public event RoutedEventHandler TaskDetailsCheckboxChecked
        {
            add { _taskDetailsCheckboxChecked += value; }
            remove { _taskDetailsCheckboxChecked -= value; }
        }

        public event RoutedEventHandler OnTaskDelete
        {
            add { _onTaskDelete += value; }
            remove { _onTaskDelete -= value; }
        }

        public event TextChangedEventHandler OnTaskDetailsChanged
        {
            add { _onTaskDetailsChange += value; }
            remove { _onTaskDetailsChange -= value; }
        }

        public event RoutedEventHandler UpdateTask
        {
            add { _updateTask += value; }
            remove { _updateTask -= value; }
        }

        public event RoutedEventHandler AddSubTask
        {
            add { _addSubTask += value; }
            remove { _addSubTask -= value; }
        }

        public event RoutedEventHandler DeleteSubTask
        {
            add { _deleteSubTask += value; }
            remove { _deleteSubTask -= value; }
        }

        public event RoutedEventHandler SubTaskDetailsCheckBoxChecked
        {
            add { _subTaskDetailsCheckboxChecked += value; }
            remove { _subTaskDetailsCheckboxChecked -= value; }
        }

        public static readonly DependencyProperty PriorityComboBoxSourceProperty = DependencyProperty.Register("PriorityComboBoxSource", typeof(object), typeof(TaskDetailsControl), new PropertyMetadata(0));

        public static readonly DependencyProperty StatusComboBoxSourceProperty = DependencyProperty.Register("StatusComboBoxSource", typeof(object), typeof(TaskDetailsControl), new PropertyMetadata(0));

        public static readonly DependencyProperty CurrTaskProperty = DependencyProperty.Register("CurrTask", typeof(UserTask), typeof(TaskDetailsControl), new PropertyMetadata(0));

        public static readonly DependencyProperty IsAddSubTaskContextTriggeredProperty = DependencyProperty.Register("IsAddSubTaskContextTriggered", typeof(bool), typeof(TaskDetailsControl), new PropertyMetadata(0));

        public TaskDetailsControl()
        {
            this.InitializeComponent();
        }

        private void OpenAddSubTaskWindowButton_ButtonClick(object sender, RoutedEventArgs e)
        {
            IsAddSubTaskContextTriggered = true;
        }

        private void AddSubTaskDialog_CancelButtonClick(object sender, RoutedEventArgs e)
        {
            IsAddSubTaskContextTriggered = false;
        }

        private void DeleteSubTaskButton_ButtonClick(object sender, RoutedEventArgs e)
        {
            _deleteSubTask?.Invoke(sender, e);
            UpdateStates();
        }

        public void SubTaskListCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            _subTaskDetailsCheckboxChecked?.Invoke(sender, e);
        }

        public void UpdateStates()
        {
            NoSubtasksMessage.Visibility = CurrTask.SubTasks.Count > 0 ? Visibility.Collapsed : Visibility.Visible;
        }

        public void ClearAllFields()
        {
            AddSubTaskDialog.ClearAllFields();
        }
    }
}
