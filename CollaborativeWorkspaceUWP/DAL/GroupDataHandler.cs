using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Models.Enums;
using CollaborativeWorkspaceUWP.Persistence.PersistenceObject.EntityPersistence;
using CollaborativeWorkspaceUWP.Utilities.Persistence;
using CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.DAL
{
    public class GroupDataHandler
    {
        private PersistenceObjectManager persistanceObjectManager;

        public GroupDataHandler()
        {
            persistanceObjectManager = new PersistenceObjectManager(PersistenceMode.SQLITE);
        }

        public ObservableCollection<Group> GetAllBoardsForProject(long projectId)
        {
            ObservableCollection<Group> result = new ObservableCollection<Group>();
            IGroupPersistence persistenceObject = null;
            try
            {
                persistenceObject = persistanceObjectManager.GetGroupPersistenceObject();
                persistenceObject.SetGetAllBoardGroupsForProjectContext(projectId);
                PersistenceHandler.Instance.Get(persistenceObject);
                result = persistenceObject.GetAllGroups();
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

        public ObservableCollection<Group> GetAllGroupsForProject(long projectId)
        {
            ObservableCollection<Group> result = new ObservableCollection<Group>();
            IGroupPersistence persistenceObject = null;
            try
            {
                persistenceObject = persistanceObjectManager.GetGroupPersistenceObject();
                persistenceObject.SetGetAllGroupsForProjectContext(projectId);
                PersistenceHandler.Instance.Get(persistenceObject);
                result = persistenceObject.GetAllGroups();
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

        public Group AddBoardGroupForProject(string name, long projectId, long boardGroupId, bool isBoardGroup)
        {
            Group result = null;
            IGroupPersistence persistenceObject = null;
            try
            {
                persistenceObject = persistanceObjectManager.GetGroupPersistenceObject();
                persistenceObject.SetAddBoardGroupForProjectContext(name, projectId, boardGroupId, isBoardGroup);
                PersistenceHandler.Instance.Get(persistenceObject);
                result = persistenceObject.GetGroup();
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

        public ObservableCollection<Group> GetBoardsForBoardGroup(long boardGrouId)
        {
            ObservableCollection<Group> result = new ObservableCollection<Group>();
            IGroupPersistence persistenceObject = null;
            try
            {
                persistenceObject = persistanceObjectManager.GetGroupPersistenceObject();
                persistenceObject.SetGetAllBoardsForBoardGroupContext(boardGrouId);
                PersistenceHandler.Instance.Get(persistenceObject);
                result = persistenceObject.GetAllGroups();
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
    }
}
