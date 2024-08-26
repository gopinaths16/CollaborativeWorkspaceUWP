using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Persistence.PersistenceObject.EntityPersistence;
using CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace CollaborativeWorkspaceUWP.Persistence.PersistenceObject.DBPersistence
{
    public class GroupDBPersistenceObject : DBPersistenceObject, IGroupPersistence
    {
        public void SetGetAllBoardGroupsForProjectContext(long projectId)
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"SELECT ID, NAME, PROJECT_ID, BOARD_GROUP_ID, IS_BOARD_GROUP FROM CW_GROUP_DETAILS WHERE PROJECT_ID=@ProjectId AND IS_BOARD_GROUP=@IsBoardGroup";
            command.Parameters.AddWithValue("@ProjectId", projectId);
            command.Parameters.AddWithValue("@IsBoardGroup", true);
            Query = command;
        }

        public void SetGetAllBoardsForBoardGroupContext(long boardGroupId)
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"SELECT ID, NAME, PROJECT_ID, BOARD_GROUP_ID, IS_BOARD_GROUP FROM CW_GROUP_DETAILS WHERE BOARD_GROUP_ID=@BoardGroupId";
            command.Parameters.AddWithValue("@BoardGroupId", boardGroupId);
            Query = command;
        }

        public void SetAddBoardGroupForProjectContext(string boardName, long projectId, long boardGroupId, bool isBoardGroup)
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"INSERT INTO CW_GROUP_DETAILS (NAME, PROJECT_ID, BOARD_GROUP_ID, IS_BOARD_GROUP) VALUES (@Name, @ProjectId, @BoardGroupId, @IsBoardGroup) RETURNING ID, NAME, PROJECT_ID, BOARD_GROUP_ID, IS_BOARD_GROUP";
            command.Parameters.AddWithValue("@Name", boardName);
            command.Parameters.AddWithValue("@ProjectId", projectId);
            command.Parameters.AddWithValue("@BoardGroupId", boardGroupId);
            command.Parameters.AddWithValue("@IsBoardGroup", isBoardGroup);
            Query = command;
        }

        public ObservableCollection<Group> GetAllGroups()
        {
            ObservableCollection<Group> boards = new ObservableCollection<Group>();
            try
            {
                if (Reader != null)
                {
                    while (Reader.Read())
                    {
                        boards.Add(new Group() { Id = Reader.GetInt64(0), Name = Reader.GetString(1), ProjectId = Reader.GetInt64(2), BoardGroupId = Reader.GetInt64(3), IsBoardGroup = Reader.GetBoolean(4)});
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return boards;
        }

        public Group GetGroup()
        {
            Group boardGroup = null;
            try
            {
                if (Reader != null && Reader.Read())
                {
                    boardGroup = new Group() { Id = Reader.GetInt64(0), Name = Reader.GetString(1), ProjectId = Reader.GetInt64(2), BoardGroupId = Reader.GetInt64(3), IsBoardGroup = Reader.GetBoolean(4) };
                }
            }
            catch (Exception ex)
            {

            }
            return boardGroup;
        }
    }
}
