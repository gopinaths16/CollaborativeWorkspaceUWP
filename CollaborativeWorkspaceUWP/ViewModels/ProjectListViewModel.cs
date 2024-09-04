using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollaborativeWorkspaceUWP.DAL;
using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Utilities;
using CollaborativeWorkspaceUWP.Utilities.Events;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;

namespace CollaborativeWorkspaceUWP.ViewModels
{
    public class ProjectListViewModel : BaseViewModel
    {
        ObservableCollection<Project> projects;
        ProjectDataHandler projectDataHandler;
        GroupDataHandler boardDataHandler;
        bool isProjectListPaneOpen;

        public ObservableCollection<Project> Projects
        {
            get { return projects; }
            set { 
                projects = value;
                ProjectListWithGroup = new ObservableCollection<object>();
                foreach (var project in projects)
                {
                    ProjectListWithGroup.Add(project);
                    if(project.IsOpen)
                    {
                        foreach (var group in project.Groups)
                        {
                            ProjectListWithGroup.Add(group);
                        }
                    }
                }
                NotifyPropertyChanged(nameof(Projects));
                NotifyPropertyChanged(nameof(ProjectListWithGroup));
            }
        }

        public ObservableCollection<object> ProjectListWithGroup
        {
            get; set;
        }

        public ProjectListViewModel()
        {
            projectDataHandler = new ProjectDataHandler();
            boardDataHandler = new GroupDataHandler();
            Projects = new ObservableCollection<Project>();
            IsProjectListPaneOpen = true;

            ViewmodelEventHandler.Instance.Subscribe<AddProjectEvent>(OnProjectAddition);
            ViewmodelEventHandler.Instance.Subscribe<AddGroupEvent>(OnBoardGroupAddition);
        }

        public bool IsProjectListPaneOpen
        {
            get { return isProjectListPaneOpen; }
            set { 
                isProjectListPaneOpen = value;
                NotifyPropertyChanged(nameof(IsProjectListPaneOpen));
            }
        }

        public void AddProjectToList(Project project)
        {
            project.BoardGroups = boardDataHandler.GetAllBoardsForProject(project.Id);
            project.Groups = boardDataHandler.GetAllGroupsForProject(project.Id);
            Projects.Add(project);
            NotifyPropertyChanged(nameof(Projects));
        }

        public void GetProjectsForCurrentTeamspace(long teamspaceId)
        {
            foreach (var project in projectDataHandler.GetProjectsForTeamspace(teamspaceId))
            {
                project.BoardGroups = boardDataHandler.GetAllBoardsForProject(project.Id);
                project.Groups = boardDataHandler.GetAllGroupsForProject(project.Id);
                Projects.Add(project);
                ProjectListWithGroup.Add(project);
                if(project.IsOpen)
                {
                    foreach (var group in project.Groups)
                    {
                        ProjectListWithGroup.Add(group);
                    }
                }
            }
            NotifyPropertyChanged(nameof(Projects));
        }

        private async Task OnProjectAddition(AddProjectEvent e)
        {
            AddProjectToList((Project)e.Project.Clone());
        }

        private async Task OnBoardGroupAddition(AddGroupEvent e)
        {
            if (e.Group != null)
            {
                foreach (Project project in Projects)
                {
                    if(project.Id == e.Group.ProjectId && e.Group.IsBoardGroup)
                    {
                        project.BoardGroups.Add(e.Group);
                    }
                    else if(project.Id == e.Group.ProjectId)
                    {
                        project.Groups.Add(e.Group);
                    }
                }
            }
        }
    }
}
