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
        TaskListViewModel taskListViewModel;
        ProjectListViewModel projectListViewModel;
        TaskDetailsViewModel taskDetailsViewModel;
        AddProjectViewModel addProjectViewModel;
        AddTaskViewModel addTaskViewModel;
        AddSubTaskViewModel addSubTaskViewModel;
        MainViewModel mainViewModel;
        CurrentTeamspaceViewModel currTeamspaceViewModel;

        public TaskView()
        {
            this.InitializeComponent();
            
            projectListViewModel = new ProjectListViewModel();
            taskListViewModel = new TaskListViewModel();
            taskDetailsViewModel = new TaskDetailsViewModel();
            addProjectViewModel = new AddProjectViewModel();
            addTaskViewModel = new AddTaskViewModel();
            addSubTaskViewModel = new AddSubTaskViewModel();
            currTeamspaceViewModel = new CurrentTeamspaceViewModel();
        }

        private void CustomIconButtonControl_ButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void ProjectListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Project currProject = (Project)e.ClickedItem;
            taskListViewModel.GetTasksForProject(currProject);
            SelectProjectMessage.Visibility = Visibility.Collapsed;
            SelectTaskMessage.Visibility = Visibility.Visible;
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

        private async void OpenAddTaskWindowButton_ButtonClick(object sender, RoutedEventArgs e)
        {
            await AddTaskDialog.ShowAsync();
        }

        private async void AddProjectFromDialogButton_ButtonClick(object sender, RoutedEventArgs e)
        {
            string projectName = AddProjectDialogProjectName.Text;
            Status projectStatus = ((Status)AddProjectDialogStatus.SelectedItem);
            Priority projectPriority = ((Priority)AddProjectDialogPriority.SelectedItem);

            Project project = new Project();
            project.Name = projectName;
            project.Status = projectStatus.Id;
            project.Priority = projectPriority.Id;
            project.TeamsapceId = currTeamspaceViewModel.CurrTeamspace.Id;

            Project result = addProjectViewModel.AddProject(project);
            projectListViewModel.AddProjectToList(result);

            AddProjectDialog.Hide();
        }

        private void AddTaskFromDialogButton_ButtonClick(object sender, RoutedEventArgs e)
        {
            UserTask task = new UserTask();

            task.Name = AddTaskDialogTaskName.Text;
            task.Description = AddTaskDialogDescription.Text;
            task.Status = ((Status)AddTaskDialogStatus.SelectedItem).Id;
            task.Priority = ((Priority)AddTaskDialogPriority.SelectedItem).Id;
            task.ProjectId = taskListViewModel.CurrentProject.Id;
            task.OwnerId = 0;
            task.AssigneeId = 0;

            task = addTaskViewModel.AddTask(task);
            taskListViewModel.AddTaskToList(task);

            AddTaskDialog.Hide();
        }

        private void CloseProjectDialogButton_Click(object sender, RoutedEventArgs e)
        {
            AddProjectDialog.Hide();
        }

        private void CloseTaskDialogButton_Click(object sender, RoutedEventArgs e)
        {
            AddTaskDialog.Hide();
        }

        private void OpenSplitViewButton_Click(object sender, RoutedEventArgs e)
        {
            projectListViewModel.IsProjectListPaneOpen = !projectListViewModel.IsProjectListPaneOpen;
        }

        private async void OpenAddSubTaskWindowButton_ButtonClick(object sender, RoutedEventArgs e)
        {
            addSubTaskViewModel.CurrTask = taskDetailsViewModel.CurrTask;
            addSubTaskViewModel.LoadNonSubTasks();
            await AddSubTaskDialog.ShowAsync();
        }

        private void CloseSubTaskDialog_Click(object sender, RoutedEventArgs e)
        {
            AddSubTaskDialog.Hide();
        }

        private void SubTaskListViewByProject_ItemClick(object sender, ItemClickEventArgs e)
        {
            UserTask task = (UserTask)e.ClickedItem;
            addSubTaskViewModel.AddSubTaskForCurrTask(task);
        }

        private void OpenSubTaskButton_ButtonClick(object sender, RoutedEventArgs e)
        {

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if(e.Parameter is MainViewModel mainViewModel)
            {
                this.mainViewModel = mainViewModel;
                this.DataContext = mainViewModel;

                currTeamspaceViewModel.CurrTeamspace = mainViewModel.TeamspacesForCurrOrganization[0];
                projectListViewModel.GetProjectsForCurrentTeamspace(currTeamspaceViewModel.CurrTeamspace.Id);
            }
        }

        private void AddTeamspace_ButtonClick(object sender, RoutedEventArgs e)
        {
            Teamspace teamspace = new Teamspace() { Name = TeamspaceName.Text, OrgId = mainViewModel.CurrOrganization.Id, OwnerId = 0 };
            mainViewModel.CreateTeamspaceInCurrentOrganization(teamspace);
            AddTeamspaceDialog.Hide();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if(mainViewModel.TeamspacesForCurrOrganization.Count <= 0)
            {
                await AddTeamspaceDialog.ShowAsync();
            }
        }
    }
}
