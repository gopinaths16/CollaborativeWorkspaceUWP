using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Models.Comm
{
    internal class TaskWindowOpenEventArgs : EventArgs
    {
        private bool isWindowOpen;
        private UserTask currentTask;

        public bool IsWindowOpen
        {
            get { return isWindowOpen; }
            set { isWindowOpen = value; }
        }

        public UserTask CurrentTask
        {
            get { return currentTask; }
            set { currentTask = value; }
        }
    }
}
