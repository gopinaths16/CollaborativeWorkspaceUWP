using CollaborativeWorkspaceUWP.Models;
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

        void SetGetTasksForProjectContext(double projectId);
        ObservableCollection<UserTask> GetAllTasks();
    }
}
