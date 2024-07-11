using CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject;
using CollaborativeWorkspaceUWP.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject.EntityPersistence;
using CollaborativeWorkspaceUWP.Utilities.Persistence;

namespace CollaborativeWorkspaceUWP.DAL
{
    public class TeamspaceDataHandler
    {
        private PersistenceObjectManager persistenceObjectManager;

        public TeamspaceDataHandler()
        {
            persistenceObjectManager = new PersistenceObjectManager(PersistenceMode.SQLITE);
        }

        public Teamspace AddTeamspace(Teamspace teamspace)
        {
            Teamspace result = null;
            ITeamspacePersistence persistenceObject = null;
            try
            {
                persistenceObject = persistenceObjectManager.GetTeamspacePersistenceObject();
                persistenceObject.SetAddTeamspaceContext(teamspace);
                PersistenceHandler.Instance.Get(persistenceObject);
                result = persistenceObject.GetTeamspace();
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
            return result;
        }

        public ObservableCollection<Teamspace> GetAllTeamspacesForCurrOrganization(long orgId)
        {
            ObservableCollection<Teamspace> teamspaces = new ObservableCollection<Teamspace>();
            ITeamspacePersistence persistenceObject = null;
            try
            {
                persistenceObject = persistenceObjectManager.GetTeamspacePersistenceObject();
                persistenceObject.SetGetTeamspacesForCurrentOrgContext(orgId);
                PersistenceHandler.Instance.Get(persistenceObject);
                teamspaces = persistenceObject.GetTeamspaces();
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
            return teamspaces;
        }
    }
}
