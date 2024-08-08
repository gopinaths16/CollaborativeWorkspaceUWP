using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Models
{
    public class Comment
    {
        private long id;
        private string message;
        private long taskId;
        private long ownerId;

        public long Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        public long TaskId
        {
            get { return taskId; }
            set { taskId = value; }
        }

        public long OwnerId
        {
            get { return ownerId; }
            set { ownerId = value; }
        }

        public User Owner
        {
            get;
            set;
        }

        public ObservableCollection<Attachment> Attachments { get; set; }

        public Comment()
        {
            Attachments = new ObservableCollection<Attachment>();
        }
    }
}
