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
    public class CommentDBPersistenceObject : DBPersistenceObject, ICommentPersistence
    {
        public void SetAddCommentContext(Comment comment)
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"INSERT INTO CW_COMMENT_DETAILS(MESSAGE, TASK_ID, OWNER_ID) VALUES (@Message, @TaskId, @OwnerId) RETURNING ID, MESSAGE, TASK_ID, OWNER_ID";
            command.Parameters.AddWithValue("@Message", comment.Message);
            command.Parameters.AddWithValue("@TaskId", comment.TaskId);
            command.Parameters.AddWithValue("@OwnerId", comment.OwnerId);
            Query = command;
        }

        public void SetGetAllCommentsForCurrentTaskContext(long taskId)
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"SELECT ID, MESSAGE, TASK_ID, OWNER_ID FROM CW_COMMENT_DETAILS WHERE TASK_ID=@TaskId";
            command.Parameters.AddWithValue("@TaskId", taskId);
            Query = command;
        }

        public void SetDeleteCommentContext(long id)
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"DELETE FROM CW_COMMENT_DETAILS WHERE ID=@Id";
            command.Parameters.AddWithValue("@Id", id);
            Query = command;
        }

        public Comment GetComment()
        {
            Comment result = null;
            try
            {
                if(Reader != null)
                {
                    if (Reader.Read())
                    {
                        result = new Comment();
                        result.Id = Reader.GetInt64(0);
                        result.Message  = Reader.GetString(1);
                        result.TaskId = Reader.GetInt64(2);
                        result.OwnerId = Reader.GetInt64(3);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public ObservableCollection<Comment> GetComments()
        {
            ObservableCollection<Comment> result = new ObservableCollection<Comment>();
            try
            {
                if (Reader != null)
                {
                    while (Reader.Read())
                    {
                        Comment comment = new Comment();
                        comment.Id = Reader.GetInt64(0);
                        comment.Message = Reader.GetString(1);
                        comment.TaskId = Reader.GetInt64(2);
                        comment.OwnerId = Reader.GetInt64(3);
                        result.Add(comment);
                    }
                }
            }
            catch(Exception ex)
            {

            }
            return result;
        }
    }
}
