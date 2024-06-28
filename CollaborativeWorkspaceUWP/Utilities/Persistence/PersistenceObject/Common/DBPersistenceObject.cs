using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject
{
    public class DBPersistenceObject : IPersistenceObject
    {
        private SQLiteCommand query;
        private SQLiteDataReader reader;

        public SQLiteCommand Query
        {
            get { return query; }
            set { query = value; }
        }

        public SQLiteDataReader Reader
        {
            get { return reader; }
            set { reader = value; }
        }

        public void Dispose()
        {
            if(reader != null)
            {
                reader.Dispose();
            }
        }
    }
}
