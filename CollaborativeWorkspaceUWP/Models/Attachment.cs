using CollaborativeWorkspaceUWP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.StartScreen;

namespace CollaborativeWorkspaceUWP.Models
{
    public class Attachment : BaseModel
    {
        private long id;
        private string name;
        private string path;
        private string type;
        private long taskId;
        private long commentId;
        private string size;

        public long Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public long TaskId
        {
            get { return taskId; }
            set { taskId = value; }
        }

        public long CommentId
        {
            get { return commentId; }
            set { commentId = value; }
        }

        public StorageFile Content
        {
            get; set;
        }

        public string AddedTime
        {
            get; set;
        }

        public string Size
        {
            get
            {
                if(size != null)
                {
                    long s = (long)Convert.ToDouble(size);
                    return Util.ConvertBytesToReadableSize(s);
                }
                return size;
            }
            private set
            {
                size = value;
                NotifyPropertyChanged(nameof(Size));
            }
        }

        public async Task SetSize()
        {
            if(Size == null || Size == "")
            {
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                storageFolder = await storageFolder.GetFolderAsync("Attachments");
                StorageFile file = await storageFolder.GetFileAsync(Path);
                BasicProperties basicProperties = await file.GetBasicPropertiesAsync();
                Size = string.Format("{0:n0}", basicProperties.Size);
            }
        }

        public void SetAddedTime(DateTime addedTime)
        {
            AddedTime = addedTime.ToString("D");
        }

        public bool IsOnlyForAddition { get; set; }

        public string OriginalPath { get; set; }
    }
}
