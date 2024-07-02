using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollaborativeWorkspaceUWP.DAL;
using CollaborativeWorkspaceUWP.Models;

namespace CollaborativeWorkspaceUWP.ViewModels
{
    public class ProjectListViewModel : BaseViewModel
    {
        ObservableCollection<Project> projects;

        ProjectDataHandler projectDataHandler;

        public ObservableCollection<Project> Projects
        {
            get { return projects; }
            set { projects = value; }
        }
        public ProjectListViewModel()
        {
            projectDataHandler = new ProjectDataHandler();
            Projects = projectDataHandler.GetAllProjects();
        }
        public void AddProject(Project project)
        {
            projectDataHandler.AddProject(project);
            projects = projectDataHandler.GetAllProjects();
            NotifyPropertyChanged(nameof(Projects));
        }

        
    }
}
