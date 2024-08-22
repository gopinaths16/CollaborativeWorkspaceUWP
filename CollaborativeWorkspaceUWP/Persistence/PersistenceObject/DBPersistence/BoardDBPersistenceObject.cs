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

namespace CollaborativeWorkspaceUWP.Persistence.PersistenceObject.DBPersistence
{
    public class BoardDBPersistenceObject : DBPersistenceObject, IBoardPersistence
    {
        public void SetGetAllBoardGroupsForProjectContext(long projectId)
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"SELECT ID, NAME, PROJECT_ID FROM CW_BOARD_GROUP_DETAILS WHERE PROJECT_ID=@ProjectId";
            command.Parameters.AddWithValue("@ProjectId", projectId);
            Query = command;
        }

        public void SetAddBoardGroupForProjectContext(string boardName, long projectId)
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"INSERT INTO CW_BOARD_GROUP_DETAILS (NAME, PROJECT_ID) VALUES (@Name, @ProjectId) RETURNING ID, NAME, PROJECT_ID";
            command.Parameters.AddWithValue("@Name", boardName);
            command.Parameters.AddWithValue("@ProjectId", projectId);
            Query = command;
        }

        public ObservableCollection<BoardGroup> GetAllBoardGroups()
        {
            ObservableCollection<BoardGroup> boards = new ObservableCollection<BoardGroup>();
            try
            {
                if (Reader != null)
                {
                    while (Reader.Read())
                    {
                        boards.Add(new BoardGroup() { Id = Reader.GetInt64(0), Name = Reader.GetString(1), ProjectId = Reader.GetInt64(2)});
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return boards;
        }

        public BoardGroup GetBoardGroup()
        {
            BoardGroup boardGroup = null;
            try
            {
                if (Reader != null && Reader.Read())
                {
                    boardGroup = new BoardGroup() { Id = Reader.GetInt64(0), Name = Reader.GetString(1), ProjectId = Reader.GetInt64(2) };
                }
            }
            catch (Exception ex)
            {

            }
            return boardGroup;
        }
    }
}
