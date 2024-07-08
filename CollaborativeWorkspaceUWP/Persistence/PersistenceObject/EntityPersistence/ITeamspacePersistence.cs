using CollaborativeWorkspaceUWP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject.EntityPersistence
{
    public interface ITeamspacePersistence : IPersistenceObject
    {
        void SetGetTeamspacesForCurrentOrgContext(long orgId);
        void SetAddTeamspaceContext(Teamspace teamspace);
        ObservableCollection<Teamspace> GetTeamspaces();
        Teamspace GetTeamspace();
    }
}
