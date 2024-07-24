using CollaborativeWorkspaceUWP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Utilities.Events
{
    public class AddAttachmentEvent
    {
        public UserTask Task { get; set; }
        public ObservableCollection<Attachment> Attachments { get; set; }
    }
}
