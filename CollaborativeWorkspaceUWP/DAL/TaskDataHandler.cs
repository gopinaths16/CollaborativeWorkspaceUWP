﻿using CollaborativeWorkspaceUWP.Models;
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
                Persistence.Instance.Get(persistenceObject);
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
        public ObservableCollection<UserTask> GetTasksForProject(long projectId)
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

        public  ObservableCollection<UserTask> GetNonSubTasks(long taskId, long projectId)
        {
            ObservableCollection<UserTask> tasks = new ObservableCollection<UserTask>();
            ITaskPersistence persistenceObject = null;
            try
            {
                persistenceObject = persistanceObjectManager.GetTaskPersistenceObject();
                persistenceObject.SetGetNonSubTasksContext(taskId, projectId);
                Persistence.Instance.Get(persistenceObject);
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
                Persistence.Instance.Add(persistenceObject);
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
                Persistence.Instance.Get(persistenceObject);
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

    }
}
