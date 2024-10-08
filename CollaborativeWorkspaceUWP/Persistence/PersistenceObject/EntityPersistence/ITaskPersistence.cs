﻿using CollaborativeWorkspaceUWP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject.EntityPersistence
{
    public interface ITaskPersistence : IPersistenceObject
    {
        void SetAddContext(UserTask task);
        void SetGetAllTasksContext();
        void SetGetAllTasksForPriorityContext(long priorityId, long projectId);
        void SetGetAllTasksForStatusContext(long statusId, long projectId);
        void SetGetTasksForProjectContext(long projectId);
        void SetGetNonSubTasksContext(long taskId, long projectId);
        void SetAddSubTaskContext(long parentTaskId, long childTaskId);
        void SetGetAllSubTasksContext(long parentTaskId);
        void SetUpdateDescriptionContext(UserTask task);
        void SetDeleteTaskContext(long taskId);
        void SetUpdateOrderContext(UserTask task);
        void SetUpdateOrderForTasksContext(int start, int end, int value, long taskId, long projectId);
        void SetGetTasksForGroupContext(long groupId);
        void SetUpdateGroupForTaskContext(UserTask task);
        UserTask GetTask();
        ObservableCollection<UserTask> GetAllTasks();
    }
}
