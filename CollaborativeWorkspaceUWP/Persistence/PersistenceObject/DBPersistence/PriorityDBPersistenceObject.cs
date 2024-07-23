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
    internal class PriorityDBPersistenceObject : DBPersistenceObject, IPriorityPersistence
    {

        public void SetGetPrioritiesContext()
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"SELECT * FROM CW_PRIORITY";
            Query = command;
        }

        public List<Priority> GetPriorities()
        {
            List<Priority> priorityList = new List<Priority>();
            try
            {
                if(Reader != null)
                {
                    while (Reader.Read())
                    {
                        priorityList.Add(new Priority(Reader.GetInt64(0), Reader.GetString(1), Reader.GetString(2)));
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return priorityList;
        }

    }
}
