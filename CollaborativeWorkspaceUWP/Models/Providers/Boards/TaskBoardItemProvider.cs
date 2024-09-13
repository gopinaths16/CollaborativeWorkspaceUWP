using CollaborativeWorkspaceUWP.DAL;
using CollaborativeWorkspaceUWP.Utilities.Custom;
using CollaborativeWorkspaceUWP.Views.ViewObjects.Boards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Models.Providers.Boards
{
    public class TaskBoardItemProvider : IBoardItemProvider
    {
        TaskDataHandler taskDataHandler;

        public long BoardId
        {
            get; set;
        }

        public bool IsDefaultProvider
        {
            get; private set;
        }

        public ICollection<IDefaultArgs> DefaultArgs
        {
            get; private set;
        }

        public TaskBoardItemProvider()
        {
            taskDataHandler = new TaskDataHandler();

            DefaultArgs = new List<IDefaultArgs>();
            IsDefaultProvider = false;
        }

        public ICollection<IBoardItem> GetBoardItems(long boardId, long projectId)
        {
            return taskDataHandler.GetTasksForBoard(boardId);
        }

        public bool DoesItemBelongToBoard(IBoardItem item)
        {
            if (item != null && (item as UserTask).GroupId == BoardId)
            {
                return true;
            }
            return false;
        }

        public ICollection<IDefaultArgs> GetDefaultArgs()
        {
            return DefaultArgs;
        }

        public ICollection<IBoardItem> UpdateBoardItems(ICollection<IBoardItem> boardItems)
        {
            ICollection<IBoardItem> result = new List<IBoardItem>();
            foreach (UserTask item in boardItems)
            {
                item.GroupId = BoardId;
                taskDataHandler.UpdateGroupIdForTask(item);
                result.Add(item);
            }
            return boardItems;
        }

        public async Task UpdateSource(IBoardItem item, ICollection<IBoardItem> source)
        {
            UserTask task = (source as IncrementalLoadingCollection<IBoardItem>).Where(target => target.Id == item.Id).FirstOrDefault() as UserTask;
            if (task != null)
            {
                if((item as UserTask).GroupId != BoardId)
                {
                    (source as IncrementalLoadingCollection<IBoardItem>).Remove(task);
                }
            }
            else if((item as UserTask).GroupId == BoardId)
            {
                (source as IncrementalLoadingCollection<IBoardItem>).Add(item);
            }
        }
    }
}
