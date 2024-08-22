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
    public class BoardDataHandler
    {
        private PersistenceObjectManager persistanceObjectManager;

        public BoardDataHandler()
        {
            persistanceObjectManager = new PersistenceObjectManager(PersistenceMode.SQLITE);
        }

        public ObservableCollection<BoardGroup> GetAllBoardsForProject(long projectId)
        {
            ObservableCollection<BoardGroup> result = new ObservableCollection<BoardGroup>();
            IBoardPersistence persistenceObject = null;
            try
            {
                persistenceObject = persistanceObjectManager.GetBoardPersistenceObject();
                persistenceObject.SetGetAllBoardGroupsForProjectContext(projectId);
                PersistenceHandler.Instance.Get(persistenceObject);
                result = persistenceObject.GetAllBoardGroups();
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

        public BoardGroup AddBoardGroupForProject(string name, long projectId)
        {
            BoardGroup result = null;
            IBoardPersistence persistenceObject = null;
            try
            {
                persistenceObject = persistanceObjectManager.GetBoardPersistenceObject();
                persistenceObject.SetAddBoardGroupForProjectContext(name, projectId);
                PersistenceHandler.Instance.Get(persistenceObject);
                result = persistenceObject.GetBoardGroup();
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
    }
}
