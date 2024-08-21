using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Persistence.PersistenceObject.EntityPersistence;
using CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Persistence.PersistenceObject.DBPersistence
{
    internal class StatusDBPersistenceObject : DBPersistenceObject, IStatusPersistence
    {
        
        public void SetGetAllStatusContext()
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"SELECT * FROM CW_STATUS";
            Query = command;
        }

        public List<Status> GetAllStatus() 
        {
            List<Status> statusList = new List<Status>();
            try
            {
                if(Reader != null)
                {
                    while (Reader.Read())
                    {
                        statusList.Add(new Status(Reader.GetInt64(0), Reader.GetString(1), Reader.GetString(2)));
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return statusList;
        }

    }
}
