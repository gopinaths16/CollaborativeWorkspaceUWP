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
        TaskViewModel taskViewModel;
        ProjectListViewModel projectListViewModel;
        TaskDetailsViewModel taskDetailsViewModel;

        TaskListViewModel taskListViewModel;

        public TaskView()
        {
            this.InitializeComponent();
            
            taskViewModel = new TaskViewModel();
            
            projectListViewModel = new ProjectListViewModel();
            taskListViewModel = new TaskListViewModel();
            taskDetailsViewModel = new TaskDetailsViewModel();

            this.DataContext = taskViewModel;
            //TaskViewTable.DataContext = taskViewModel;
            
        }

        private void CustomIconButtonControl_ButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void ProjectListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            projectListViewModel.CurrProj = (Project)e.ClickedItem;
            taskListViewModel.GetTasksForProject(projectListViewModel.CurrProj.Id);
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
            string taskName = TaskName.Text;
            Status taskStatus = GetStatus(TaskStatus.SelectedValue.ToString());
            Priority taskPriority = GetPriority(TaskPriority.SelectedValue.ToString());

            UserTask task = new UserTask();
            task.Name = taskName;
            task.Status = (int)taskStatus;
            task.Priority = (int)taskPriority;
            task.ProjectId = projectListViewModel.CurrProj.Id;

            taskListViewModel.AddNewTask(task);
        }

        private void AddProjectButton_ButtonClick(object sender, RoutedEventArgs e)
        {
            string projectName = ProjectName.Text;
            Status projectStatus = GetStatus(ProjectStatus.SelectedValue.ToString());
            Priority projectPriority = GetPriority(ProjectPriority.SelectedValue.ToString());
            
            Project project = new Project();
            project.Name = projectName;
            project.Status = (int)projectStatus;
            project.Priority = (int)projectPriority;

            projectListViewModel.AddProject(project);
        }

        private Status GetStatus(string status)
        {
            Status result = Status.PLANNING;
            switch(status)
            {
                case "Planning":
                    result = Status.PLANNING;
                    break;

                case "InProgress":
                    result = Status.INPROGRESS;
                    break;

                case "Completed":
                    result = Status.COMPLETED;
                    break;

                default:
                    result = Status.PLANNING;
                    break;
            }
            return result;
        }

        private Priority GetPriority(string priority)
        {
            Priority result = Priority.LOW;
            switch (priority)
            {
                case "Low":
                    result = Priority.LOW;
                    break;

                case "Medium":
                    result = Priority.MEDIUM;
                    break;

                case "High":
                    result = Priority.HIGH;
                    break;

                default:
                    result = Priority.LOW;
                    break;
            }
            return result;
        }
    }
}
