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

namespace CollaborativeWorkspaceUWP.ViewModels
{
    public class ProjectListViewModel : BaseViewModel
    {
        ObservableCollection<Project> projects;
        ProjectDataHandler projectDataHandler;
        bool isProjectListPaneOpen;

        public ObservableCollection<Project> Projects
        {
            get { return projects; }
            set { 
                projects = value; 
                NotifyPropertyChanged(nameof(Projects));
            }
        }

        public ProjectListViewModel()
        {
            projectDataHandler = new ProjectDataHandler();
            Projects = new ObservableCollection<Project>();
            IsProjectListPaneOpen = true;

            ViewmodelEventHandler.Instance.Subscribe<AddProjectEvent>(OnProjectAddition);
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
            Projects.Add(project);
            NotifyPropertyChanged(nameof(Projects));
        }

        public void GetProjectsForCurrentTeamspace(long teamspaceId)
        {
            foreach (var project in projectDataHandler.GetProjectsForTeamspace(teamspaceId))
            {
                Projects.Add(project);
            }
            NotifyPropertyChanged(nameof(Projects));
        }

        private void OnProjectAddition(AddProjectEvent e)
        {
            AddProjectToList((Project)e.Project.Clone());
        }

        //private void OnTaskAddtion(AddTaskEvent e)
    }
}
