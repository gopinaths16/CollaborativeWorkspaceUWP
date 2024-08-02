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
using System.ServiceModel.Channels;
using System.Threading.Tasks;

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
        AddProjectViewModel addProjectViewModel;
        AddTaskViewModel addTaskViewModel;
        AddSubTaskViewModel addSubTaskViewModel;
        MainViewModel mainViewModel;
        CurrentTeamspaceViewModel currTeamspaceViewModel;

        private bool isDragging = false;
        private double initialPosition;

        public TaskView()
        {
            this.InitializeComponent();
            
            projectListViewModel = new ProjectListViewModel();
            taskListViewModel = new TaskListViewModel();
            addProjectViewModel = new AddProjectViewModel();
            addTaskViewModel = new AddTaskViewModel();
            addSubTaskViewModel = new AddSubTaskViewModel();
            currTeamspaceViewModel = new CurrentTeamspaceViewModel();
        }

        private void OnVisualStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            if(e.NewState != null && e.NewState.Name != null)
            {
                GoToVisualState(e.NewState.Name, 1200);
            }
        }

        private void GoToVisualState(string layoutName, double width)
        {
            if (layoutName == "SingleWindowLayout" || width <= 800)
            {
                TaskViewByProject.ColumnDefinitions.Clear();
                TaskViewByProject.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                Grid.SetColumn(TaskDetailsViewContainer, 0);
                VisualStateManager.GoToState(this, "SingleWindowLayout", true);
                bool isCurrentTaskSelected = taskListViewModel.CurrTask != null;
                taskListViewModel.IsSingleWindowLayoutTriggered = true;
                TaskListView.Visibility = isCurrentTaskSelected ? Visibility.Collapsed : Visibility.Visible;
                TaskDetailsViewContainer.Visibility = isCurrentTaskSelected ? Visibility.Visible : Visibility.Collapsed;

            }
            else if (layoutName == "NarrowLayout" || width <= 1100)
            {
                TaskViewByProject.ColumnDefinitions.Clear();
                TaskViewByProject.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(550) });
                TaskViewByProject.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                Grid.SetColumn(TaskDetailsViewContainer, 1);
                VisualStateManager.GoToState(this, "NarrowLayout", true);
                TaskListView.Visibility = Visibility.Visible;
                TaskDetailsView.Visibility = Visibility.Visible;
                TaskDetailsViewContainer.Visibility = Visibility.Visible;
                taskListViewModel.IsSingleWindowLayoutTriggered = false;
            }
            else if (layoutName == "WideLayout" || width <= 1200)
            {
                TaskViewByProject.ColumnDefinitions.Clear();
                TaskViewByProject.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(550) });
                TaskViewByProject.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                Grid.SetColumn(TaskDetailsViewContainer, 1);
                VisualStateManager.GoToState(this, "WideLayout", true);
                TaskListView.Visibility = Visibility.Visible;
                TaskDetailsView.Visibility = Visibility.Visible;
                TaskDetailsViewContainer.Visibility = Visibility.Visible;
                taskListViewModel.IsSingleWindowLayoutTriggered = false;
            }
        }

        private async void ProjectListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            await TaskDetailsView.UpdateCurrentTask();
            taskListViewModel.Dispose();
            Project currProject = (Project)e.ClickedItem;
            if (taskListViewModel.IsSingleWindowLayoutTriggered)
            {
                TaskListView.Visibility = Visibility.Visible;
                TaskDetailsViewContainer.Visibility = Visibility.Collapsed;
            }
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
            if(taskListViewModel.IsSingleWindowLayoutTriggered)
            {
                TaskListView.Visibility = Visibility.Collapsed;
                TaskDetailsViewContainer.Visibility = Visibility.Visible;
            }
            UserTask task = (UserTask)e.ClickedItem;
            taskListViewModel.CurrTask = task;
            TaskDetailsView.SetCurrentTask(task);
        }

        private async void AddProjectButton_ButtonClick(object sender, RoutedEventArgs e)
        {
            await AddProjectDialog.ShowAsync();
        }

        private void OpenAddTaskWindowButton_ButtonClick(object sender, RoutedEventArgs e)
        {
            taskListViewModel.IsAddTaskContextTriggered = true;
        }

        private async void AddProjectFromDialogButton_ButtonClick(object sender, RoutedEventArgs e)
        {
            string projectName = AddProjectDialogProjectName.Text;
            Status projectStatus = ((Status)AddProjectDialogStatus.SelectedItem);
            Priority projectPriority = ((Priority)AddProjectDialogPriority.SelectedItem);

            Project project = new Project();
            project.Name = projectName;
            project.Status = projectStatus != null ? projectStatus.Id : 1;
            project.Priority = projectPriority != null ? projectPriority.Id : 1;
            project.TeamsapceId = currTeamspaceViewModel.CurrTeamspace.Id;

            await addProjectViewModel.AddProject(project);

            AddProjectDialog.Hide();
        }

        private void AddTaskFromDialogButton_ButtonClick(object sender, RoutedEventArgs e)
        {
            taskListViewModel.IsAddTaskContextTriggered = false;
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

        private async void TaskListCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            long taskId = (long)checkBox.Tag;
            bool checkboxStatus = (bool)checkBox.IsChecked;
            await taskListViewModel.UpdateTaskCompletionStatus(taskId, !checkboxStatus);
        }

        private async void OpenTaskInSeparateWindow(object sender, RoutedEventArgs e)
        {
            AppWindow appWindow;
            Grid newWindowLayout = new Grid();
            newWindowLayout.Style = TaskDetailsViewSeparateDisplay;
            TaskDetailsControl taskDetailsControl = new TaskDetailsControl();
            taskDetailsControl.SetCurrentTask((UserTask)TaskDetailsView.GetCurrentTask().Clone());
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
                if(taskListViewModel.Tasks.Count > 0)
                {
                    taskListViewModel.CurrTask = taskListViewModel.Tasks[0];
                    TaskDetailsView.SetCurrentTask(taskListViewModel.Tasks[0]);
                }
            }
            var visualStates = VisualStateManager.GetVisualStateGroups(TaskViewPanel).ToList();
            visualStates.First().CurrentStateChanging += OnVisualStateChanged;
            var windowBounds = Window.Current.Bounds;
            double windowWidth = windowBounds.Width;
            GoToVisualState(string.Empty, windowWidth);
            AddTaskDialog.ParentTaskId = -1;
        }

        private void TaskDetailsView_OnTaskClear(object sender, RoutedEventArgs e)
        {
            taskListViewModel.CurrTask = null;
            if(taskListViewModel.IsSingleWindowLayoutTriggered)
            {
                TaskListView.Visibility = Visibility.Visible;
                TaskDetailsViewContainer.Visibility = Visibility.Collapsed;
            }
        }
    }
}
