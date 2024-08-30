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
using CollaborativeWorkspaceUWP.Auth.Handlers;
using System.Collections.ObjectModel;
using CollaborativeWorkspaceUWP.Utilities;
using System.Security.Cryptography.X509Certificates;
using CollaborativeWorkspaceUWP.DAL;

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
        MainViewModel mainViewModel;
        CurrentTeamspaceViewModel currTeamspaceViewModel;
        BoardGroupViewModel boardGroupViewModel;
        AddGroupViewModel addGroupViewModel;

        ResourceDictionary staticStyles = new ResourceDictionary();

        private bool isDragging = false;

        public TaskView()
        {
            this.InitializeComponent();

            projectListViewModel = new ProjectListViewModel();
            taskListViewModel = new TaskListViewModel();
            addProjectViewModel = new AddProjectViewModel();
            addTaskViewModel = new AddTaskViewModel();
            currTeamspaceViewModel = new CurrentTeamspaceViewModel();
            boardGroupViewModel = new BoardGroupViewModel();

            staticStyles.Source = new Uri("ms-appx:///Styles/StaticStyles.xaml");
        }

        private void OnVisualStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            if (e.NewState != null && e.NewState.Name != null)
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
                projectListViewModel.IsProjectListPaneOpen = false;
            }
            taskListViewModel.GetTasksForProject((Project)currProject.Clone());
            TaskDetailsView.SetCurrentTask(null);
            taskListViewModel.CurrTask = null;
        }

        private async void TaskListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (TaskDetailsView.GetCurrentTask() != null)
            {
                await TaskDetailsView.UpdateCurrentTask();
            }
            TaskDetailsView.SetCurrentTask(null);
            if (taskListViewModel.IsSingleWindowLayoutTriggered)
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
            AddTaskDialog.Focus();
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
            project.OwnerId = UserSessionHandler.Instance.CurrUser.Id;

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
            OpenSplitViewButton.Content = projectListViewModel.IsProjectListPaneOpen ? "\ue8a0" : "\ue89f";
        }

        private void AddTeamspace_ButtonClick(object sender, RoutedEventArgs e)
        {
            Teamspace teamspace = new Teamspace() { Name = TeamspaceName.Text, OrgId = mainViewModel.CurrOrganization.Id, OwnerId = UserSessionHandler.Instance.CurrUser.Id };
            currTeamspaceViewModel.CurrTeamspace = mainViewModel.CreateTeamspaceInCurrentOrganization(teamspace);
            projectListViewModel.GetProjectsForCurrentTeamspace(currTeamspaceViewModel.CurrTeamspace.Id);
            if (projectListViewModel.Projects.Count > 0)
            {
                taskListViewModel.GetTasksForProject((Project)projectListViewModel.Projects[0].Clone());
            }
            AddTeamspaceDialog.Hide();
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
            if (mainViewModel.TeamspacesForCurrOrganization.Count <= 0)
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
                if (taskListViewModel.Tasks.Count > 0)
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
            taskListViewModel.IsLoaded = true;
        }

        private void TaskDetailsView_OnTaskClear(object sender, RoutedEventArgs e)
        {
            taskListViewModel.CurrTask = null;
            if (taskListViewModel.IsSingleWindowLayoutTriggered)
            {
                TaskListView.Visibility = Visibility.Visible;
                TaskDetailsViewContainer.Visibility = Visibility.Collapsed;
            }
        }

        private void TaskListViewByProject_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            UserTask draggedTask = e.Items.First() as UserTask;
            if (draggedTask != null)
            {
                ListViewItem draggedItem = TaskListViewByProject.ContainerFromItem(draggedTask) as ListViewItem;
                TaskListItemControl completionCheckBox = Util.FindChild<TaskListItemControl>(draggedItem, draggedTask.Id.ToString());
                completionCheckBox.DisableControl();
            }
            isDragging = true;
        }

        private async void TaskListViewByProject_DragItemsCompleted(ListViewBase sender, DragItemsCompletedEventArgs args)
        {
            UserTask draggedTask = args.Items.First() as UserTask;
            await taskListViewModel.ReOrderTasks(draggedTask);
            if (draggedTask != null)
            {
                ListViewItem draggedItem = TaskListViewByProject.ContainerFromItem(draggedTask) as ListViewItem;
                TaskListItemControl completionCheckBox = Util.FindChild<TaskListItemControl>(draggedItem, draggedTask.Id.ToString());
                completionCheckBox.EnableControl();
            }
            isDragging = false;
        }

        private void Grid_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var grid = sender as Grid;
            if (grid != null)
            {
                var flyoutBase = FlyoutBase.GetAttachedFlyout(grid);
                if (flyoutBase != null)
                {
                    flyoutBase.ShowAt(grid);
                }
            }
        }

        private void Grid_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            ProjectInfoView.IsOpen = true;
        }

        private void TaskTabView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Pivot pivot = sender as Pivot;
            if (pivot != null)
            {
                DataTemplate listViewItemTemplate = pivot.SelectedIndex == 1 ? this.Resources["ProjectListViewItemTemplateForBoardView"] as DataTemplate : this.Resources["ProjectListViewItemTemplate"] as DataTemplate ;
                ProjectListDropDown.SetListViewItemTemplate(listViewItemTemplate);
                ProjectListDropDown.ListViewItemClickEnabled = pivot.SelectedIndex == 1 ? false : true;
            }
        }

        private void ProjectGroupDropDown_OnItemAddClick(object sender, RoutedEventArgs e)
        {

        }

        private void ProjectGroupDropDown_ListViewItemClicked(object sender, ItemClickEventArgs e)
        {

        }

        private void ProjectBoardGroupDropDown_OnItemAddClick(object sender, RoutedEventArgs e)
        {

        }

        private async void ProjectBoardGroupDropDown_ListViewItemClicked(object sender, ItemClickEventArgs e)
        {
            Group boardGroup = e.ClickedItem as Group;
            boardGroupViewModel.BoardGroup = boardGroup;
            if (boardGroup != null)
            {
                boardGroupViewModel.GetBoards();
                boardGroupViewModel.ProjectId = boardGroup.ProjectId;
                boardGroupViewModel.BoardGroupId = boardGroup.Id;
                boardGroupViewModel.IsBoardGroupContext = false;
            }
        }

        private void AddBoardGroupControl_CancelButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.Parent is FlyoutPresenter presenter)
            {
                if (presenter.Parent is Flyout flyout)
                {
                    flyout.Hide();
                }
            }
        }

        private async void BoardGroupView_OnBoardAddition(object sender, RoutedEventArgs e)
        {
            string boardName = BoardGroupView.BoardName;
            await boardGroupViewModel.AddBoardGroup(boardName);
            BoardGroupView.Clear();
        }

        private void BoardGroupView_CancelButtonClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
