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
using CollaborativeWorkspaceUWP.Models;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CollaborativeWorkspaceUWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TaskView : Page
    {
        TaskViewModel taskViewModel;

        public TaskView()
        {
            this.InitializeComponent();
            taskViewModel = new TaskViewModel();
            this.DataContext = taskViewModel;
            //TaskViewTable.DataContext = taskViewModel;
            
        }

        private void CustomIconButtonControl_ButtonClick(object sender, RoutedEventArgs e)
        {
            taskViewModel.CreateNewTask();
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            taskViewModel.CurrentTask = (UserTask)e.ClickedItem;
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            taskViewModel.AddNewTask(TaskName.Text, TaskStatus.Text, TaskPriority.Text, TaskDescription.Text);
        }
    }
}
