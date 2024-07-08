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
    public class OrganizationDBPersistenceObject : DBPersistenceObject, IOrganizationPersistence
    {
        public OrganizationDBPersistenceObject() { }

        public void SetAddOrganizationContext(Organization organization)
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"INSERT INTO CW_ORGANIZATION_DETAILS(NAME, OWNERID) VALUES(@Name, @OwnerId) RETURNING ID, NAME, OWNERID";
            command.Parameters.AddWithValue("@Name", organization.Name);
            command.Parameters.AddWithValue("@OwnerId", organization.OwnerId);
            Query = command;
        }

        public void SetGetAllOrganizationsContext()
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"SELECT ID, NAME, OWNERID FROM CW_ORGANIZATION_DETAILS";
            Query = command;
        }

        public Organization GetOrganization()
        {
            Organization organization = null;
            try
            {
                if(Reader != null)
                {
                    if(Reader.Read())
                    {
                        organization = new Organization(Reader.GetInt64(0), Reader.GetString(1), Reader.GetInt64(2));
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return organization;
        }

        public ObservableCollection<Organization> GetAllOrganizations()
        {
            ObservableCollection<Organization> organizations = new ObservableCollection<Organization>();
            try
            {
                if (Reader != null)
                {
                    while (Reader.Read())
                    {
                        Organization organization = new Organization(Reader.GetInt64(0), Reader.GetString(1), Reader.GetInt64(2));
                        organizations.Add(organization);
                    }
                }
            }
            catch(Exception ex)
            {

            }
            return organizations;
        }
    }
}
