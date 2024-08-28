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
using Windows.UI.Xaml.Media;

namespace CollaborativeWorkspaceUWP.DAL
{
    public class TaskDataHandler
    {
        private PersistenceObjectManager persistanceObjectManager;

        public TaskDataHandler()
        {
            persistanceObjectManager = new PersistenceObjectManager(PersistenceMode.SQLITE);
        }

        public UserTask AddTask(UserTask task)
        {
            ITaskPersistence persistenceObject = persistanceObjectManager.GetTaskPersistenceObject();
            UserTask result = null;
            try
            {
                persistenceObject.SetAddContext(task);
                PersistenceHandler.Instance.Get(persistenceObject);
                result = persistenceObject.GetTask();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                persistenceObject.Dispose();
            }
            return result;
        }
        public ObservableCollection<UserTask> GetAllTasks()
        {
            ITaskPersistence persistenceObject = null;
            ObservableCollection<UserTask> tasks = new ObservableCollection<UserTask>();
            try
            {
                persistenceObject = persistanceObjectManager.GetTaskPersistenceObject();
                persistenceObject.SetGetAllTasksContext();
                PersistenceHandler.Instance.Get(persistenceObject);
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
        public ObservableCollection<UserTask> GetTasksForProject(long projectId)
        {
            ObservableCollection<UserTask> tasks = new ObservableCollection<UserTask>();
            ITaskPersistence persistenceObject = null;
            try
            {
                persistenceObject = persistanceObjectManager.GetTaskPersistenceObject();
                persistenceObject.SetGetTasksForProjectContext(projectId);
                PersistenceHandler.Instance.Get(persistenceObject);
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

        public  ObservableCollection<UserTask> GetNonSubTasks(long taskId, long projectId)
        {
            ObservableCollection<UserTask> tasks = new ObservableCollection<UserTask>();
            ITaskPersistence persistenceObject = null;
            try
            {
                persistenceObject = persistanceObjectManager.GetTaskPersistenceObject();
                persistenceObject.SetGetNonSubTasksContext(taskId, projectId);
                PersistenceHandler.Instance.Get(persistenceObject);
                tasks = persistenceObject.GetAllTasks();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (persistenceObject != null)
                {
                    persistenceObject.Dispose();
                }
            }
            return tasks;
        }

        public void AddSubTaskForTask(long parentTaskId, long childTaskId)
        {
            ITaskPersistence persistenceObject = null;
            try
            {
                persistenceObject = persistanceObjectManager.GetTaskPersistenceObject();
                persistenceObject.SetAddSubTaskContext(parentTaskId, childTaskId);
                PersistenceHandler.Instance.Add(persistenceObject);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (persistenceObject != null)
                {
                    persistenceObject.Dispose();
                }
            }
        }

        public ObservableCollection<UserTask> GetAllSubTasks(long parentTaskId)
        {
            ITaskPersistence persistenceObject = null;
            ObservableCollection<UserTask> tasks = new ObservableCollection<UserTask>();
            try
            {
                persistenceObject = persistanceObjectManager.GetTaskPersistenceObject();
                persistenceObject.SetGetAllSubTasksContext(parentTaskId);
                PersistenceHandler.Instance.Get(persistenceObject);
                tasks = persistenceObject.GetAllTasks();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (persistenceObject != null)
                {
                    persistenceObject.Dispose();
                }
            }
            return tasks;
        }

        public void UpdateTaskStatus(long taskId, long status)
        {
            ITaskPersistence persistenceObject = null;
            try
            {

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
        }

        public UserTask UpdateTask(UserTask task)
        {
            ITaskPersistence persistenceObject = null;
            UserTask result = null;
            try
            {
                persistenceObject = persistanceObjectManager.GetTaskPersistenceObject();
                persistenceObject.SetUpdateDescriptionContext(task);
                PersistenceHandler.Instance.Get(persistenceObject);
                result = persistenceObject.GetTask();
            }
            catch(Exception ex)
            {

            }
            finally
            {
                if (persistenceObject != null)
                {
                    persistenceObject.Dispose();
                }
            }
            return result;
        }

        public void DeleteTask(long taskId)
        {
            ITaskPersistence persistenceObject = null;
            try
            {
                persistenceObject = persistanceObjectManager.GetTaskPersistenceObject();
                persistenceObject.SetDeleteTaskContext(taskId);
                PersistenceHandler.Instance.Delete(persistenceObject);
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
        }

        public void UpdateOrderForTasks(int start, int end, int value, UserTask taskToBeReordered, long projectId)
        {
            ITaskPersistence persistenceObject = null;
            ITaskPersistence persistenceObjectAlt = null;
            List<IPersistenceObject> persistenceObjects = new List<IPersistenceObject>();
            try
            {
                persistenceObject = persistanceObjectManager.GetTaskPersistenceObject();
                persistenceObjectAlt = persistanceObjectManager.GetTaskPersistenceObject();
                persistenceObject.SetUpdateOrderContext(taskToBeReordered);
                persistenceObjectAlt.SetUpdateOrderForTasksContext(start, end, value, taskToBeReordered.Id, projectId);
                persistenceObjects.Add(persistenceObject);
                persistenceObjects.Add(persistenceObjectAlt);
                PersistenceHandler.Instance.PerformTransaction(persistenceObjects);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                foreach (IPersistenceObject persObj in persistenceObjects)
                {
                    if(persObj != null)
                    {
                        persObj.Dispose();
                    }
                }
            }
        }

        public ObservableCollection<UserTask> GetTasksForGroup(long groupId)
        {
            ObservableCollection<UserTask> result = null;
            ITaskPersistence persistenceObject = null;
            try
            {
                persistenceObject = persistanceObjectManager.GetTaskPersistenceObject();
                persistenceObject.SetGetTasksForGroupContext(groupId);
                PersistenceHandler.Instance.Get(persistenceObject);
                result = persistenceObject.GetAllTasks();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (persistenceObject != null)
                {
                    persistenceObject.Dispose();
                }
            }
            return result;
        }

        public void UpdateGroupIdForTask(UserTask task)
        {
            ITaskPersistence persistenceObject = null;
            try
            {
                persistenceObject = persistanceObjectManager.GetTaskPersistenceObject();
                persistenceObject.SetUpdateGroupForTaskContext(task);
                PersistenceHandler.Instance.Update(persistenceObject);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (persistenceObject != null)
                {
                    persistenceObject.Dispose();
                }
            }
        }
    }
}
