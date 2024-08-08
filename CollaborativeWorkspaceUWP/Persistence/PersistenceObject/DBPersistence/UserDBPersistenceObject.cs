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
    public class UserDBPersistenceObject : DBPersistenceObject, IUserPersistence
    {
        public void SetAddUserContext(User user)
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"INSERT INTO CW_USER_DETAILS (USERNAME, PASSWORD, DISPLAYNAME) VALUES (@Username, @Password, @Displayname) RETURNING ID, USERNAME, DISPLAYNAME";
            command.Parameters.AddWithValue("@Username", user.Username);
            command.Parameters.AddWithValue("@Password", user.Password);
            command.Parameters.AddWithValue("@Displayname", user.DisplayName);
            Query = command;
        }

        public void SetGetUserContext(string username, string password)
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"SELECT ID, USERNAME, DISPLAYNAME FROM CW_USER_DETAILS WHERE USERNAME=@Username AND PASSWORD=@Password";
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Password", password);
            Query = command;
        }

        public void SetGetUserContext(string username)
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"SELECT ID, USERNAME, DISPLAYNAME FROM CW_USER_DETAILS WHERE USERNAME=@Username";
            command.Parameters.AddWithValue("@Username", username);
            Query = command;
        }

        public User GetUser()
        {
            User user = null;
            try
            {
                if(Reader.Read())
                {
                    user = new User();
                    user.Id = Reader.GetInt64(0);
                    user.Username = Reader.GetString(1);
                    user.DisplayName = Reader.GetString(2);
                }
            }
            catch (Exception ex)
            {

            }
            return user;
        }
    }
}
