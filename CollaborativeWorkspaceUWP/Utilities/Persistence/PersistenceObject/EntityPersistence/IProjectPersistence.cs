using CollaborativeWorkspaceUWP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject.EntityPersistence.Entity
{
    public interface IProjectPersistence : IPersistenceObject
    {
        void SetAddContext(Project project);
        void SetGetAllProjectsContext();
        ObservableCollection<Project> GetAllProjects();
    }
}
