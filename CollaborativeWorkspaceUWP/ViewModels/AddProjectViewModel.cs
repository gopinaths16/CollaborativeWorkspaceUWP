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
        private List<PriorityData> priorityData;

        PriorityDataHandler priorityDataHandler;

        public Project Project
        {
            get { return project; }
            set { project = value; }
        }

        public List<PriorityData> PriorityData
        {
            get { return priorityData; }
            set { priorityData = value; }
        }

        public AddProjectViewModel()
        {
            priorityDataHandler = new PriorityDataHandler();
            PriorityData = priorityDataHandler.GetPriorityDataForUI();
        }

    }
}
