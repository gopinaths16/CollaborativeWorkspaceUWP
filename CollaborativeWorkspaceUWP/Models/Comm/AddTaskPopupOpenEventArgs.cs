using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Models.Comm
{
    public class AddTaskWindowOpenEvent : EventArgs
    {
        private bool isWindowOpen;

        public bool IsWindowOpen
        {
            get { return isWindowOpen; }
            set { isWindowOpen = value; }
        }
    }
}
