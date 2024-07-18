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
            taskDetailsViewModel.UpdateTask();
            Project currProject = (Project)e.ClickedItem;
            taskListViewModel.GetTasksForProject((Project)currProject.Clone());
            taskDetailsViewModel.CurrTask = null;
        }

        private void TaskListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            taskDetailsViewModel.UpdateTask();
            taskDetailsViewModel.CurrTask = null;
            taskDetailsViewModel.CurrTask = (UserTask)((UserTask)e.ClickedItem).Clone();
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

        private void OpenAddSubTaskWindowButton_ButtonClick(object sender, RoutedEventArgs e)
        {
            taskDetailsViewModel.IsAddSubTaskContextTriggered = true;
        }

        private void CloseSubTaskDialog_Click(object sender, RoutedEventArgs e)
        {
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
            }
        }

        private void AddTeamspace_ButtonClick(object sender, RoutedEventArgs e)
        {
            Teamspace teamspace = new Teamspace() { Name = TeamspaceName.Text, OrgId = mainViewModel.CurrOrganization.Id, OwnerId = 0 };
            currTeamspaceViewModel.CurrTeamspace = mainViewModel.CreateTeamspaceInCurrentOrganization(teamspace);
            projectListViewModel.GetProjectsForCurrentTeamspace(currTeamspaceViewModel.CurrTeamspace.Id);
            AddTeamspaceDialog.Hide();
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
            }
        }

        private void CloseSubTaskDialogButton_Click(object sender, RoutedEventArgs e)
        {
            taskDetailsViewModel.IsAddSubTaskContextTriggered = false;
        }

        private void AddSubTaskFromDialogButton_ButtonClick(object sender, RoutedEventArgs e)
        {
            UserTask task = new UserTask();

            task.Name = TaskDetailsView.SubTaskName;
            task.Description = TaskDetailsView.SubTaskDescription;
            task.Status = TaskDetailsView.SubTaskStatus.Id;
            task.Priority = TaskDetailsView.SubTaskPriority.Id;
            task.ProjectId = taskListViewModel.CurrentProject.Id;
            task.OwnerId = 0;
            task.AssigneeId = 0;
            task.ParentTaskId = taskDetailsViewModel.CurrTask.Id;

            addTaskViewModel.AddTask(task);

            taskDetailsViewModel.IsAddSubTaskContextTriggered = false;

            TaskDetailsView.ClearAllFields();
            TaskDetailsView.UpdateStates();
        }


        private void TaskListCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            long taskId = (long)checkBox.Tag;
            bool checkboxStatus = (bool)checkBox.IsChecked;
            taskListViewModel.UpdateTaskCompletionStatus(taskId, !checkboxStatus);
            if (taskDetailsViewModel.CurrTask != null && (taskDetailsViewModel.CurrTask.Id == taskId || taskDetailsViewModel.CurrTask.Id == taskListViewModel.GetTaskForTaskId(taskId).ParentTaskId))
            {
                taskDetailsViewModel.CurrTaskPropertyChanged();
            }
        }

        private void TaskDetailsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if(checkBox.Tag != null)
            {
                long taskId = (long)checkBox.Tag;
                bool checkboxStatus = (bool)checkBox.IsChecked;
                taskDetailsViewModel.UpdateTaskCompletionStatus(taskId, !checkboxStatus);
            }
        }

        private void SubTaskListCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (checkBox.Tag != null)
            {
                long taskId = (long)checkBox.Tag;
                bool checkboxStatus = (bool)checkBox.IsChecked;
                taskDetailsViewModel.UpdateSubTaskCompletionStatus(taskId, !checkboxStatus);
            }
        }

        private void TaskUpdate(object sender, RoutedEventArgs e)
        {
            taskDetailsViewModel.UpdateTask();
        }

        private void TaskDetailsChanged(object sender, TextChangedEventArgs e)
        {
            taskDetailsViewModel.SetTaskUpdatedContext();
        }

        private void TaskUpdateTriggered(object sender, SelectionChangedEventArgs e)
        {
            taskDetailsViewModel.UpdateTask(true);
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            taskDetailsViewModel.DeleteTask();
        }

        public void DeleteSubTaskButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            long taskId = (long)button.Tag;
            taskDetailsViewModel.DeleteSubTask(taskId);
        }

        private void AddAttachmentsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OpenAddAttachementWindowButton_ButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private async void OpenTaskInSeparateWindow(object sender, RoutedEventArgs e)
        {
        }
    }
}
