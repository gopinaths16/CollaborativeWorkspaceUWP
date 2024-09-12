using CollaborativeWorkspaceUWP.Models.Providers.Boards;
using CollaborativeWorkspaceUWP.Views.ViewObjects.Boards;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Models
{
    public class Group : BaseModel, ICloneable, IBoardGroup, IBoard
    {
        ObservableCollection<UserTask> tasks;
        ObservableCollection<Group> boards;

        public IBoardItemProvider BoardItemProvider { get; set; }

        public long Id { get; set; }
        public string Name { get; set; }
        public long ProjectId { get; set; }
        public long BoardGroupId { get; set; }
        public bool IsBoardGroup { get; set; }
        public string ColorCode {  get; set; }

        public ObservableCollection<UserTask> Tasks
        {
            get {  return tasks; }
            set { tasks = value; }
        }

        public Group Self
        {
            get
            {
                return this;
            }
        }

        public Group()
        {
            Tasks = new ObservableCollection<UserTask>();
        }

        public Object Clone()
        {
            Group group = new Group() { Id = Id, Name = Name, ProjectId = ProjectId, BoardGroupId = BoardGroupId, IsBoardGroup = IsBoardGroup};
            return group;
        }

        public void SetTasks(ObservableCollection<UserTask> tasks)
        {
            Tasks = tasks;
            NotifyPropertyChanged(nameof(Tasks));
        }

        public void SetBoardItemProvider(IBoardItemProvider boardItemProvider)
        {
            BoardItemProvider = boardItemProvider;
        }

        public void NotifyChangesToEntity()
        {
            NotifyPropertyChanged(nameof(Tasks));
        }
    }
}
