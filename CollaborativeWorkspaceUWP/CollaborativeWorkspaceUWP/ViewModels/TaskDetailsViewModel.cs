using CollaborativeWorkspaceUWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.ViewModels
{
    public class TaskDetailsViewModel : BaseViewModel
    {
        UserTask currTask;

        public UserTask CurrTask
        {
            get { return currTask; }
            set
            {
                currTask = value;
                NotifyPropertyChanged(nameof(CurrTask));
            }
        }

        public TaskDetailsViewModel()
        {

        }

    }
}
