﻿using CollaborativeWorkspaceUWP.DAL;
using CollaborativeWorkspaceUWP.Utilities.Custom;
using CollaborativeWorkspaceUWP.Views.ViewObjects.Boards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Models.Providers.Boards
{
    public class TaskOnPriorityBoardItemProvider : IBoardItemProvider
    {
        TaskDataHandler taskDataHandler;
        PriorityDataHandler priorityDataHandler;

        public long BoardId
        {
            get; set;
        }

        public bool IsDefaultProvider
        {
            get; private set;
        }

        public ICollection<IDefaultArgs> DefaultArgs { get; private set; }

        public TaskOnPriorityBoardItemProvider()
        {
            taskDataHandler = new TaskDataHandler();

            IsDefaultProvider = true;
            DefaultArgs = new List<IDefaultArgs>();
        }

        public ICollection<IBoardItem> GetBoardItems(long boardId, long projectId)
        {
            return taskDataHandler.GetAllTasksForPriorityBoard(boardId, projectId);
        }

        public bool DoesItemBelongToBoard(IBoardItem item)
        {
            if (item != null && (item as UserTask).Priority == BoardId)
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
                item.Priority = BoardId;
                taskDataHandler.UpdateTask(item);
                result.Add(item);
            }
            return boardItems;
        }

        public async Task UpdateSource(IBoardItem item, ICollection<IBoardItem> source)
        {
            var tasks = (source as IncrementalLoadingCollection<IBoardItem>).Where(target => target.Id == item.Id);
            UserTask task = tasks.FirstOrDefault() as UserTask;
            if (task != null)
            {
                if ((item as UserTask).Priority != BoardId)
                {
                    (source as IncrementalLoadingCollection<IBoardItem>).Remove(task);
                }
            }
            else if ((item as UserTask).Priority == BoardId)
            {
                (source as IncrementalLoadingCollection<IBoardItem>).Add(item);
            }
        }
    }
}
