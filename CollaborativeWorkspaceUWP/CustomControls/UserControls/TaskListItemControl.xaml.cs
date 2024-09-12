using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.ViewModels;
using CollaborativeWorkspaceUWP.Views;
using CollaborativeWorkspaceUWP.Views.ViewObjects.Boards;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CollaborativeWorkspaceUWP.CustomControls.UserControls
{
    public sealed partial class TaskListItemControl : UserControl
    {
        TaskListItemViewModel taskItemViewModel;

        public UserTask Task
        {
            get { return (UserTask)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }

        public SolidColorBrush ItemBackground
        {
            get { return (SolidColorBrush)GetValue(ItemBackgroundProperty); }
            set { SetValue(ItemBackgroundProperty, value); }
        }

        public bool IsDragging
        {
            get; set;
        }

        public bool IsStateModified
        {
            get; set;
        }

        public static readonly DependencyProperty TaskProperty = DependencyProperty.Register("Task", typeof(UserTask), typeof(TaskListItemControl), new PropertyMetadata(null, OnMyPropertyChanged));

        public static readonly DependencyProperty ItemBackgroundProperty = DependencyProperty.Register("ItemBackground", typeof(SolidColorBrush), typeof(TaskListItemControl), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        private static void OnMyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TaskListItemControl control = d as TaskListItemControl;
            UserTask task = e.NewValue as UserTask;
            if (task != null)
            {
                control.taskItemViewModel.Task = task;
            }
        }

        public TaskListItemControl()
        {
            this.InitializeComponent();
            
            taskItemViewModel = new TaskListItemViewModel();
        }

        private async void TaskListCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if(!IsDragging)
            {
                CheckBox checkBox = (CheckBox)sender;
                bool checkboxStatus = (bool)checkBox.IsChecked;
                if (taskItemViewModel.Task != null)
                {
                    if (taskItemViewModel.Task.IsCompleted != checkboxStatus)
                    {
                        taskItemViewModel.Task.IsCompleted = checkboxStatus;
                        await taskItemViewModel.UpdateTaskCompletionStatus(!checkboxStatus);
                    }
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void Grid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "PointerOver", true);
        }

        private void Grid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Normal", true);
        }

        public void DisableControl()
        {
            TaskCompletionCheckBox.Checked -= TaskListCheckBox_Checked;
            TaskCompletionCheckBox.Unchecked -= TaskListCheckBox_Checked;
        }

        public void EnableControl()
        {
            TaskCompletionCheckBox.Checked += TaskListCheckBox_Checked;
            TaskCompletionCheckBox.Unchecked += TaskListCheckBox_Checked;
        }

        private async void UserControl_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            AppWindow appWindow;
            Grid newWindowLayout = new Grid();
            newWindowLayout.Style = TaskDetailsViewSeparateDisplay;
            TaskDetailsControl taskDetailsControl = new TaskDetailsControl();
            taskDetailsControl.SetCurrentTask((UserTask)taskItemViewModel.Task.Clone());
            taskDetailsControl.IsSeparateWindow = true;
            taskDetailsControl.AllowTaskClear = false;
            newWindowLayout.Children.Add(taskDetailsControl);
            appWindow = await AppWindow.TryCreateAsync();
            ElementCompositionPreview.SetAppWindowContent(appWindow, newWindowLayout);
            await appWindow.TryShowAsync();
            appWindow.Closed += delegate
            {
                appWindow = null;
            };
        }

        public void LoadData<T>(T task)
        {
            if (task != null)
            {
                taskItemViewModel.Task = task as UserTask;
            }
        }
    }
}
