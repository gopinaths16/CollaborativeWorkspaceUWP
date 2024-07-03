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
using CollaborativeWorkspaceUWP.Models.Enums;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CollaborativeWorkspaceUWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TaskView : Page
    {
        ProjectListViewModel projectListViewModel;
        TaskDetailsViewModel taskDetailsViewModel;
        AddProjectViewModel addProjectViewModel;

        TaskListViewModel taskListViewModel;

        public TaskView()
        {
            this.InitializeComponent();
            
            
            projectListViewModel = new ProjectListViewModel();
            taskListViewModel = new TaskListViewModel();
            taskDetailsViewModel = new TaskDetailsViewModel();
            addProjectViewModel = new AddProjectViewModel();

            
        }

        private void CustomIconButtonControl_ButtonClick(object sender, RoutedEventArgs e)
        {

        }
        private void ProjectListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Project currProject = (Project)e.ClickedItem;
            taskListViewModel.GetTasksForProject(currProject);
            SelectProjectMessage.Visibility = Visibility.Collapsed;
            TaskDetailsView.Visibility = Visibility.Collapsed;
            SelectTaskMessage.Visibility = Visibility.Visible;
            TaskListView.Visibility = Visibility.Visible;
        }

        private void TaskListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            taskDetailsViewModel.CurrTask = (UserTask)e.ClickedItem;
            SelectTaskMessage.Visibility = Visibility.Collapsed;
            TaskDetailsView.Visibility = Visibility.Visible;
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            //string taskName = TaskName.Text;
            //StatusEnum taskStatus = GetStatus(TaskStatus.SelectedValue.ToString());
            //PriorityEnum taskPriority = GetPriority(TaskPriority.SelectedValue.ToString());

            //UserTask task = new UserTask();
            //task.Name = taskName;
            //task.Status = (int)taskStatus;
            //task.Priority = (int)taskPriority;


        }

        private async void AddProjectButton_ButtonClick(object sender, RoutedEventArgs e)
        {
            await AddProjectDialog.ShowAsync();
        }

        private async void AddProjectFromDialogButton_ButtonClick(object sender, RoutedEventArgs e)
        {
            string projectName = AddProjectDialogProjectName.Text;
            Status projectStatus = ((Status)AddProjectDialogStatus.SelectedItem);
            Priority projectPriority = ((Priority)AddProjectDialogPriority.SelectedItem);

            Project project = new Project();
            project.Name = projectName;
            project.Status = projectStatus.Value;
            project.Priority = projectPriority.Value;

            Project result = addProjectViewModel.AddProject(project);
            projectListViewModel.AddProjectToList(result);

            AddProjectDialog.Hide();
        }
    }
}
