using CollaborativeWorkspaceUWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Utilities.Events
{
    public class DeleteAttachmentEvent
    {
        public Attachment Attachment { get; set; }
    }
}
