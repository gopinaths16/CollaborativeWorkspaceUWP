using CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace CollaborativeWorkspaceUWP.Utilities.Persistence
{
    public class SqliteDBPersistenceHandler : IPersistenceHandler
    {
        private SQLiteConnection connection;
        private string PathToDBFile = ApplicationData.Current.LocalFolder.Path + Path.DirectorySeparatorChar + "CollaborativeWorkspaceUWPDB.db";
        private string PathToDBSchema = "ms-appx:///Assets/DBSchema/Sqlite/DatabaseSchema.sql";

        public SqliteDBPersistenceHandler()
        {

        }

        public async Task Initialize()
        {
            try
            {
                if (connection == null)
                {
                    //if (!File.Exists(PathToDBFile))
                    //{
                        //StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                        //StorageFile dbFile = await storageFolder.CreateFileAsync("CollaborativeWorkspaceUWPDB.db");
                    //}
                    var connectionString = new SQLiteConnectionStringBuilder();
                    connectionString.DataSource = PathToDBFile;
                    connection = new SQLiteConnection(connectionString.ToString());
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    await CreateDBTablesFromSchema();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("SqliteDBPersistenceHandler :: Initialize() :: Error message - " + ex.ToString());
            }
            finally
            {
            }
        }

        private async Task CreateDBTablesFromSchema()
        {
            try
            {
                var dbSchema = await StorageFile.GetFileFromApplicationUriAsync(new Uri(PathToDBSchema));
                string queries = await FileIO.ReadTextAsync(dbSchema);
                foreach (string query in queries.Split("\r\n"))
                {
                    try
                    {
                        SQLiteCommand command = new SQLiteCommand(query);
                        ExecuteQuery(command);
                    }
                    catch (Exception ex)
                    {

                    }
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
            }
        }

        public SQLiteConnection GetConnection()
        {
            return connection;
        }

        public void ExecuteQuery(SQLiteCommand command)
        {
            try
            {
                SQLiteConnection conn = GetConnection();
                command.Connection = conn;
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("SqliteDBPersistenceHandler :: ExecuteQuery() :: Error message - " + ex.ToString());
            }
            finally
            {

            }
        }

        public SQLiteDataReader ExecuteQueryAndGetReader(SQLiteCommand command)
        {
            SQLiteDataReader reader = null;
            try
            {
                SQLiteConnection conn = GetConnection();
                command.Connection = conn;
                reader = command.ExecuteReader();
            }
            catch (Exception ex)
            {
                Console.WriteLine("SqliteDBPersistenceHandler :: ExecuteQuery() :: Error message - " + ex.ToString());
            }
            finally
            {

            }
            return reader;
        }

        public void Add(IPersistenceObject persistenceObject)
        {
            ExecuteQuery(((DBPersistenceObject) persistenceObject).Query);
        }

        public void Get(IPersistenceObject persistenceObject)
        {
            ((DBPersistenceObject)persistenceObject).Reader = ExecuteQueryAndGetReader(((DBPersistenceObject)persistenceObject).Query);
        }

        public void Update(IPersistenceObject persistenceObject)
        {
            ExecuteQuery(((DBPersistenceObject)persistenceObject).Query);
        }

        public void Delete(IPersistenceObject persistenceObject)
        {
            ExecuteQuery(((DBPersistenceObject)persistenceObject).Query);
        }

    }
}
