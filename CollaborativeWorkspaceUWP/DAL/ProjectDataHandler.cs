using CollaborativeWorkspaceUWP.Models.Enums;
using CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollaborativeWorkspaceUWP.Models;
using System.Collections.ObjectModel;
using CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject.EntityPersistence.Entity;
using CollaborativeWorkspaceUWP.Utilities.Persistence;

namespace CollaborativeWorkspaceUWP.DAL
{
    public class ProjectDataHandler
    {
        private PersistanceObjectManager persistanceObjectManager;

        public ProjectDataHandler()
        {
            persistanceObjectManager = new PersistanceObjectManager(PersistenceMode.SQLITE);
        }

        public void AddProject(Project project)
        {
            IProjectPersistence persistenceObject = null;
            try
            {
                persistenceObject = persistanceObjectManager.GetProjectPersistenceObject();
                persistenceObject.SetAddContext(project);
                Persistence.Instance.Add(persistenceObject);
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

        public ObservableCollection<Project> GetAllProjects()
        {
            ObservableCollection<Project> projects = new ObservableCollection<Project>();
            IProjectPersistence persistenceObject = null;
            try
            {
                persistenceObject = persistanceObjectManager.GetProjectPersistenceObject();
                persistenceObject.SetGetAllProjectsContext();
                Persistence.Instance.Get(persistenceObject);
                projects = persistenceObject.GetAllProjects();
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return projects;
        }
    }
}
