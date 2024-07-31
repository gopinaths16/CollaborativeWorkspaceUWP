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
    public class AttachmentDBPersistenceObject : DBPersistenceObject, IAttachmentPersistence
    {

        public void SetAddAttachmentContext(Attachment attachment)
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"INSERT INTO CW_ATTACHMENT_DETAILS (NAME, PATH, TYPE, TASK_ID, COMMENT_ID) VALUES (@Name, @Path, @Type, @TaskId, @CommentId) RETURNING ID, NAME, PATH, TYPE, TASK_ID, COMMENT_ID";
            command.Parameters.AddWithValue("@Name", attachment.Name);
            command.Parameters.AddWithValue("@Path", attachment.Path);
            command.Parameters.AddWithValue("@Type", attachment.Type);
            command.Parameters.AddWithValue("@TaskId", attachment.TaskId);
            command.Parameters.AddWithValue("@CommentId", attachment.CommentId);
            Query = command;
        }

        public void SetGetAllAttachmentsForTaskContext(long taskId)
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"SELECT ID, NAME, PATH, TYPE, TASK_ID, COMMENT_ID FROM CW_ATTACHMENT_DETAILS WHERE TASK_ID=@TaskId";
            command.Parameters.AddWithValue("@TaskId", taskId);
            Query = command;
        }

        public void SetGetAllAttachmentsForCommentContext(long commentId)
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"SELECT ID, NAME, PATH, TYPE, TASK_ID, COMMENT_ID FROM CW_ATTACHMENT_DETAILS WHERE COMMENT_ID=@CommentId";
            command.Parameters.AddWithValue("@CommentId", commentId);
            Query = command;
        }

        public ObservableCollection<Attachment> GetAllAttachments()
        {
            ObservableCollection<Attachment> result = new ObservableCollection<Attachment>();
            try
            {
                if(Reader != null)
                {
                    while (Reader.Read())
                    {
                        Attachment attachment = new Attachment();
                        attachment.Id = Reader.GetInt64(0);
                        attachment.Name = Reader.GetString(1);
                        attachment.Path = Reader.GetString(2);
                        attachment.Type = Reader.GetString(3);
                        attachment.TaskId = Reader.GetInt64(4);
                        attachment.CommentId = Reader.GetInt64(5);
                        result.Add(attachment);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return result;
        }

        public Attachment GetAttachment()
        {
            Attachment attachment = null;
            try
            {
                if(Reader != null)
                {
                    if(Reader.Read())
                    {
                        attachment = new Attachment();
                        attachment.Id = Reader.GetInt64(0);
                        attachment.Name = Reader.GetString(1);
                        attachment.Path = Reader.GetString(2);
                        attachment.Type = Reader.GetString(3);
                        attachment.TaskId = Reader.GetInt64(4);
                        attachment.CommentId = Reader.GetInt64(5);
                    }
                }
            }
            catch
            {

            }
            finally
            {

            }
            return attachment;
        }

    }
}
