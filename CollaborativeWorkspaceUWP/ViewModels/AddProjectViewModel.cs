using CollaborativeWorkspaceUWP.DAL;
using CollaborativeWorkspaceUWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.ViewModels
{
    public class AddProjectViewModel : BaseViewModel
    {
        private Project project;
        private List<Priority> priorityData;
        private List<Status> statusData;

        ProjectDataHandler projectDataHandler;
        PriorityDataHandler priorityDataHandler;
        StatusDataHandler statusDataHandler;

        public Project Project
        {
            get { return project; }
            set { project = value; }
        }

        public List<Priority> PriorityData
        {
            get { return priorityData; }
            set { priorityData = value; }
        }

        public List<Status> StatusData
        {
            get { return statusData; }
            set { statusData = value; }
        }

        public AddProjectViewModel()
        {
            priorityDataHandler = new PriorityDataHandler();
            statusDataHandler = new StatusDataHandler();
            projectDataHandler = new ProjectDataHandler();
            PriorityData = priorityDataHandler.GetPriorityDataForUI();
            StatusData = statusDataHandler.GetStatusDataForUI();
        }

        public Project AddProject(Project project)
        {
            return projectDataHandler.AddProject(project);
        }

    }
}
