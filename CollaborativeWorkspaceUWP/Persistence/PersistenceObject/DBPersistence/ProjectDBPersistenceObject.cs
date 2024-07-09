using CollaborativeWorkspaceUWP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject.EntityPersistence.Entity.Imp.DBPersistence
{
    public class ProjectDBPersistenceObject : DBPersistenceObject, IProjectPersistence
    {
        public void SetAddContext(Project project)
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"INSERT INTO CW_PROJECT_DETAILS(NAME, STATUS, PRIORITY, TEAMSPACEID, OWNERID) VALUES(@Name, @Status, @Priority, @TeamspaceId, @OwnerId) RETURNING ID, NAME, STATUS, PRIORITY, TEAMSPACEID, OWNERID";
            command.Parameters.AddWithValue("@Name", project.Name);
            command.Parameters.AddWithValue("@Status", project.Status);
            command.Parameters.AddWithValue("@Priority", project.Priority);
            command.Parameters.AddWithValue("@TeamspaceId", project.TeamsapceId);
            command.Parameters.AddWithValue("@OwnerId", project.OwnerId);
            Query = command;
        }

        public void SetGetAllProjectsContext()
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"SELECT * FROM CW_PROJECT_DETAILS";
            Query = command;
        }

        public void SetGetProjectsForTeamspaceContext(long teamspaceId)
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"SELECT * FROM CW_PROJECT_DETAILS WHERE TEAMSPACEID=@TeamspaceId";
            command.Parameters.AddWithValue("@TeamspaceId", teamspaceId);
            Query = command;
        }

        public ObservableCollection<Project> GetAllProjects()
        {
            ObservableCollection<Project> projects = new ObservableCollection<Project>();
            try
            {
                if (Reader != null)
                {
                    while (Reader.Read())
                    {
                        Project project = new Project(Reader.GetInt64(0), Reader.GetString(1), Reader.GetInt32(2), Reader.GetInt32(3), Reader.GetInt64(4), Reader.GetInt64(5));
                        projects.Add(project);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return projects;
        }

        public Project GetProject()
        {
            Project project = null;
            try
            {
                if (Reader != null && Reader.Read())
                {
                    project = new Project(Reader.GetInt64(0), Reader.GetString(1), Reader.GetInt32(2), Reader.GetInt32(3), Reader.GetInt64(4), Reader.GetInt64(5));
                }
            }
            catch(Exception ex)
            {

            }
            finally
            {

            }
            return project;
        }
    }
}
