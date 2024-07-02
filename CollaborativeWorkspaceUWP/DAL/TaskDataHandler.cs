using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Utilities.Persistence;
using CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CollaborativeWorkspaceUWP.Models.Enums;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject.EntityPersistence;

namespace CollaborativeWorkspaceUWP.DAL
{
    public class TaskDataHandler
    {
        private PersistanceObjectManager persistanceObjectManager;

        public TaskDataHandler()
        {
            persistanceObjectManager = new PersistanceObjectManager(PersistenceMode.SQLITE);
        }

        public void AddTask(UserTask task)
        {
            ITaskPersistence persistenceObject = persistanceObjectManager.GetTaskPersistenceObject();
            try
            {
                persistenceObject.SetAddContext(task);
                Persistence.Instance.Add(persistenceObject);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                persistenceObject.Dispose();
            }
        }
        public ObservableCollection<UserTask> GetAllTasks()
        {
            ITaskPersistence persistenceObject = null;
            ObservableCollection<UserTask> tasks = new ObservableCollection<UserTask>();
            try
            {
                persistenceObject = persistanceObjectManager.GetTaskPersistenceObject();
                persistenceObject.SetGetAllTasksContext();
                Persistence.Instance.Get(persistenceObject);
                tasks = persistenceObject.GetAllTasks();
            } 
            catch (Exception ex)
            {

            }
            finally
            {
                if(persistenceObject != null)
                {
                    persistenceObject.Dispose();
                }
            }
            return tasks;
        }
        public ObservableCollection<UserTask> GetTasksForProject(double projectId)
        {
            ObservableCollection<UserTask> tasks = new ObservableCollection<UserTask>();
            ITaskPersistence persistenceObject = null;
            try
            {
                persistenceObject = persistanceObjectManager.GetTaskPersistenceObject();
                persistenceObject.SetGetTasksForProjectContext(projectId);
                Persistence.Instance.Get(persistenceObject);
                tasks = persistenceObject.GetAllTasks();
            }
            catch(Exception ex)
            {

            }
            finally
            {
                if(persistenceObject != null)
                {
                    persistenceObject.Dispose();
                }
            }
            return tasks;
        }

    }
}
