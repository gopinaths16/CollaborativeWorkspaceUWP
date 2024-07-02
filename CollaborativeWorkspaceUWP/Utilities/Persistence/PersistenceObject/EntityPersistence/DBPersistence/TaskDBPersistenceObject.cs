using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject.EntityPersistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject
{
    public class TaskDBPersistenceObject : DBPersistenceObject, ITaskPersistence
    {
        public void SetAddContext(UserTask task)
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"INSERT INTO CW_TASK_DETAILS(NAME, DESCRIPTION, STATUS, PRIORITY, PROJECTID, OWNERID, ASSIGNEEID) VALUES(@Name, @Description, @Status, @Priority, @ProjectId, @OwnerId, @AssigneeId)";
            command.Parameters.AddWithValue("@Name", task.Name);
            command.Parameters.AddWithValue("@Description", task.Description);
            command.Parameters.AddWithValue("@Status", task.Status);
            command.Parameters.AddWithValue("@Priority", task.Priority);
            command.Parameters.AddWithValue("@ProjectId", task.ProjectId);
            command.Parameters.AddWithValue("@OwnerId", task.OwnerId);
            command.Parameters.AddWithValue("@AssigneeId", task.AssigneeId);
            Query = command;
        }

        public void SetGetAllTasksContext()
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"SELECT * FROM CW_TASK_DETAILS";
            Query = command;
        }

        public void SetGetTasksForProjectContext(double projectId)
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"SELECT * FROM CW_TASK_DETAILS WHERE PROJECTID=@ProjectId";
            command.Parameters.AddWithValue("@ProjectId", projectId);
            Query = command;
        }
        public ObservableCollection<UserTask> GetAllTasks()
        {
            ObservableCollection<UserTask> tasks = new ObservableCollection<UserTask>();
            try
            {
                if(Reader != null)
                {
                    while (Reader.Read())
                    {
                        UserTask task = new UserTask(Reader.GetDouble(0), Reader.GetString(1), null, Reader.GetInt64(3), Reader.GetInt64(4), Reader.GetDouble(5), Reader.GetDouble(6), Reader.GetDouble(7));
                        tasks.Add(task);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return tasks;
        }

        
    }
}
