using CollaborativeWorkspaceUWP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject.EntityPersistence.DBPersistence
{
    public class TeamspaceDBPersistenceObject : DBPersistenceObject, ITeamspacePersistence
    {
        public void SetGetTeamspacesForCurrentOrgContext(long orgId)
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"SELECT * FROM CW_TEAMSPACE_DETAILS WHERE ORGANIZATIONID=@OrgId";
            command.Parameters.AddWithValue("@OrgId", orgId);
            Query = command;
        }

        public void SetAddTeamspaceContext(Teamspace teamspace)
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"INSERT INTO CW_TEAMSPACE_DETAILS(NAME, ORGANIZATIONID, OWNERID) VALUES(@Name, @OrgId, @OwnerId) RETURNING ID, NAME, ORGANIZATIONID, OWNERID";
            command.Parameters.AddWithValue("@Name", teamspace.Name);
            command.Parameters.AddWithValue("@OrgId", teamspace.OrgId);
            command.Parameters.AddWithValue("@OwnerId", teamspace.OwnerId);
            Query = command;
        }

        public ObservableCollection<Teamspace> GetTeamspaces()
        {
            ObservableCollection<Teamspace> teamspaces = new ObservableCollection<Teamspace>();
            try
            {
                if(Reader != null)
                {
                    while (Reader.Read())
                    {
                        Teamspace teamspace = new Teamspace(Reader.GetInt64(0), Reader.GetString(1), Reader.GetInt64(2), Reader.GetInt64(3));
                        teamspaces.Add(teamspace);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return teamspaces;
        }

        public Teamspace GetTeamspace()
        {
            Teamspace teamspace = null;
            try
            {
                if(Reader != null)
                {
                    if(Reader.Read())
                    {
                        teamspace = new Teamspace(Reader.GetInt64(0), Reader.GetString(1), Reader.GetInt64(2), Reader.GetInt64(3));
                    }
                }
            }
            catch(Exception ex)
            {

            }
            finally
            {

            }
            return teamspace;
        }
    }
}
