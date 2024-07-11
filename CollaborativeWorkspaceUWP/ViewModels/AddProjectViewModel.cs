using CollaborativeWorkspaceUWP.DAL;
using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Utilities;
using CollaborativeWorkspaceUWP.Utilities.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.ViewModels
{
    public class AddProjectViewModel : BaseViewModel
    {
        private List<Priority> priorityData;
        private List<Status> statusData;

        ProjectDataHandler projectDataHandler;
        PriorityDataHandler priorityDataHandler;
        StatusDataHandler statusDataHandler;

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
            PriorityData = priorityDataHandler.GetPriorityData();
            StatusData = statusDataHandler.GetStatusData();
        }

        public void AddProject(Project project)
        {
            Project result = projectDataHandler.AddProject(project);
            ViewmodelEventHandler.Instance.Publish(new AddProjectEvent() { Project = result });
        }

    }
}
