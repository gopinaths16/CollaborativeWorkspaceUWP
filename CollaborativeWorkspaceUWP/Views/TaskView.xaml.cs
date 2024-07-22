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
using CollaborativeWorkspaceUWP.CustomControls.UserControls;
using Windows.UI.WindowManagement;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Hosting;
using System.Diagnostics;

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

            VisualStateManager.GetVisualStateGroups(this).First().CurrentStateChanged += OnVisualStateChanged;
        }

        private void OnVisualStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            Debug.WriteLine($"Visual state changed from {e.OldState?.Name} to {e.NewState?.Name}");

            if (e.NewState.Name == "Pressed")
            {

            }
        }

        private void ProjectListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            TaskDetailsView.UpdateCurrentTask();
            Project currProject = (Project)e.ClickedItem;
            taskListViewModel.GetTasksForProject((Project)currProject.Clone());
            TaskDetailsView.SetCurrentTask(null);
            taskListViewModel.CurrTask = null;
        }

        private void TaskListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if(TaskDetailsView.GetCurrentTask() != null)
            {
                TaskDetailsView.UpdateCurrentTask();
            }
            TaskDetailsView.SetCurrentTask(null);
            UserTask task = (UserTask)e.ClickedItem;
            taskListViewModel.CurrTask = task;
            TaskDetailsView.SetCurrentTask((UserTask)task.Clone());
        }

        private async void AddProjectButton_ButtonClick(object sender, RoutedEventArgs e)
        {
            await AddProjectDialog.ShowAsync();
        }

        private void OpenAddTaskWindowButton_ButtonClick(object sender, RoutedEventArgs e)
        {
            taskListViewModel.IsAddTaskContextTriggered = true;
        }

        private void AddProjectFromDialogButton_ButtonClick(object sender, RoutedEventArgs e)
        {
            string projectName = AddProjectDialogProjectName.Text;
            Status projectStatus = ((Status)AddProjectDialogStatus.SelectedItem);
            Priority projectPriority = ((Priority)AddProjectDialogPriority.SelectedItem);

            Project project = new Project();
            project.Name = projectName;
            project.Status = projectStatus != null ? projectStatus.Id : 1;
            project.Priority = projectPriority != null ? projectPriority.Id : 1;
            project.TeamsapceId = currTeamspaceViewModel.CurrTeamspace.Id;

            addProjectViewModel.AddProject(project);

            AddProjectDialog.Hide();
        }

        private void AddTaskFromDialogButton_ButtonClick(object sender, RoutedEventArgs e)
        {
            UserTask task = new UserTask();

            task.Name = AddTaskDialog.TaskName;
            task.Description = AddTaskDialog.TaskDescription;
            task.Status = AddTaskDialog.TaskStatus.Id;
            task.Priority = AddTaskDialog.TaskPriority.Id;
            task.ProjectId = taskListViewModel.CurrentProject.Id;
            task.OwnerId = 0;
            task.AssigneeId = 0;
            task.ParentTaskId = -1;

            addTaskViewModel.AddTask(task);

            taskListViewModel.IsAddTaskContextTriggered = false;

            AddTaskDialog.ClearAllFields();
        }

        private void CloseProjectDialogButton_Click(object sender, RoutedEventArgs e)
        {
            AddProjectDialog.Hide();
        }

        private void CloseTaskDialogButton_Click(object sender, RoutedEventArgs e)
        {
            taskListViewModel.IsAddTaskContextTriggered = false;
        }

        private void OpenSplitViewButton_Click(object sender, RoutedEventArgs e)
        {
            projectListViewModel.IsProjectListPaneOpen = !projectListViewModel.IsProjectListPaneOpen;
        }

        private void AddTeamspace_ButtonClick(object sender, RoutedEventArgs e)
        {
            Teamspace teamspace = new Teamspace() { Name = TeamspaceName.Text, OrgId = mainViewModel.CurrOrganization.Id, OwnerId = 0 };
            currTeamspaceViewModel.CurrTeamspace = mainViewModel.CreateTeamspaceInCurrentOrganization(teamspace);
            projectListViewModel.GetProjectsForCurrentTeamspace(currTeamspaceViewModel.CurrTeamspace.Id);
            if(projectListViewModel.Projects.Count > 0)
            {
                taskListViewModel.GetTasksForProject((Project)projectListViewModel.Projects[0].Clone());
            }
            AddTeamspaceDialog.Hide();
        }

        private void TaskListCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            long taskId = (long)checkBox.Tag;
            bool checkboxStatus = (bool)checkBox.IsChecked;
            taskListViewModel.UpdateTaskCompletionStatus(taskId, !checkboxStatus);
        }

        private async void OpenTaskInSeparateWindow(object sender, RoutedEventArgs e)
        {
            AppWindow appWindow;
            Grid newWindowLayout = new Grid();
            newWindowLayout.Style = TaskDetailsViewSeparateDisplay;
            TaskDetailsControl taskDetailsControl = new TaskDetailsControl();
            taskDetailsControl.SetCurrentTask((UserTask)TaskDetailsView.GetCurrentTask().Clone());
            taskDetailsControl.PriorityComboBoxSource = addTaskViewModel.PriorityData;
            taskDetailsControl.StatusComboBoxSource = addTaskViewModel.StatusData;
            taskDetailsControl.IsSeparateWindow = true;
            newWindowLayout.Children.Add(taskDetailsControl);
            appWindow = await AppWindow.TryCreateAsync();
            ElementCompositionPreview.SetAppWindowContent(appWindow, newWindowLayout);
            await appWindow.TryShowAsync();
            appWindow.Closed += delegate
            {
                appWindow = null;
            };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is MainViewModel mainViewModel)
            {
                this.mainViewModel = mainViewModel;
                this.DataContext = mainViewModel;
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if(mainViewModel.TeamspacesForCurrOrganization.Count <= 0)
            {
                await AddTeamspaceDialog.ShowAsync();
            }
            else
            {
                currTeamspaceViewModel.CurrTeamspace = mainViewModel.TeamspacesForCurrOrganization[0];
                projectListViewModel.GetProjectsForCurrentTeamspace(currTeamspaceViewModel.CurrTeamspace.Id);
                if (projectListViewModel.Projects.Count > 0)
                {
                    taskListViewModel.GetTasksForProject((Project)projectListViewModel.Projects[0].Clone());
                }
            }
        }
    }
}
