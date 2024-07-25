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
using Windows.UI.Xaml.Media;

namespace CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject
{
    public class TaskDBPersistenceObject : DBPersistenceObject, ITaskPersistence
    {
        public void SetAddContext(UserTask task)
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"INSERT INTO CW_TASK_DETAILS(NAME, DESCRIPTION, STATUS, PRIORITY, PROJECTID, OWNERID, ASSIGNEEID, PARENT_TASK_ID) VALUES(@Name, @Description, @Status, @Priority, @ProjectId, @OwnerId, @AssigneeId, @ParentTaskId) RETURNING ID, NAME, DESCRIPTION, STATUS, PRIORITY, PROJECTID, OWNERID, ASSIGNEEID, PARENT_TASK_ID";
            command.Parameters.AddWithValue("@Name", task.Name);
            command.Parameters.AddWithValue("@Description", task.Description);
            command.Parameters.AddWithValue("@Status", task.Status);
            command.Parameters.AddWithValue("@Priority", task.Priority);
            command.Parameters.AddWithValue("@ProjectId", task.ProjectId);
            command.Parameters.AddWithValue("@OwnerId", task.OwnerId);
            command.Parameters.AddWithValue("@AssigneeId", task.AssigneeId);
            command.Parameters.AddWithValue("@ParentTaskId", task.ParentTaskId);
            Query = command;
        }

        public void SetGetAllTasksContext()
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"SELECT * FROM CW_TASK_DETAILS AS CWT JOIN CW_STATUS AS CS ON CWT.STATUS=CS.ID JOIN CW_PRIORITY AS CP ON CWT.PRIORITY=CP.ID";
            Query = command;
        }

        public void SetGetTasksForProjectContext(long projectId)
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"SELECT * FROM CW_TASK_DETAILS AS CWT JOIN CW_STATUS AS CS ON CWT.STATUS=CS.ID JOIN CW_PRIORITY AS CP ON CWT.PRIORITY=CP.ID WHERE CWT.PROJECTID=@ProjectId";
            command.Parameters.AddWithValue("@ProjectId", projectId);
            Query = command;
        }

        public void SetGetNonSubTasksContext(long taskId, long projectId)
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"SELECT * FROM CW_TASK_DETAILS AS CWT JOIN CW_STATUS AS CS ON CWT.STATUS=CS.ID JOIN CW_PRIORITY AS CP ON CWT.PRIORITY=CP.ID WHERE CWT.PROJECTID=@ProjectId AND CWT.PARENT_TASK_ID=0 AND CWT.ID != @Id";
            command.Parameters.AddWithValue("@ProjectId", projectId);
            command.Parameters.AddWithValue("@Id", taskId);
            Query = command;
        }

        public void SetAddSubTaskContext(long parentTaskId, long childTaskId)
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"UPDATE CW_TASK_DETAILS SET PARENT_TASK_ID=@ParentTaskId WHERE ID=@ChildTaskId";
            command.Parameters.AddWithValue("@ParentTaskId", parentTaskId);
            command.Parameters.AddWithValue("@ChildTaskId", childTaskId);
            Query = command;
        }

        public void SetGetAllSubTasksContext(long parentTaskId)
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"SELECT * FROM CW_TASK_DETAILS AS CWT JOIN CW_STATUS AS CS ON CWT.STATUS=CS.ID JOIN CW_PRIORITY AS CP ON CWT.PRIORITY=CP.ID WHERE CWT.PARENT_TASK_ID=@ParentTaskId";
            command.Parameters.AddWithValue("@ParentTaskId", parentTaskId);
            Query = command;
        }

        public void SetUpdateDescriptionContext(UserTask task)
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"UPDATE CW_TASK_DETAILS SET NAME=@Name, DESCRIPTION=@Description, STATUS=@Status, PRIORITY=@Priority, PROJECTID=@ProjectId, OWNERID=@OwnerId, ASSIGNEEID=@AssigneeId, PARENT_TASK_ID=@ParentTaskId WHERE ID=@Id RETURNING ID, NAME, DESCRIPTION, STATUS, PRIORITY, PROJECTID, OWNERID, ASSIGNEEID, PARENT_TASK_ID";
            command.Parameters.AddWithValue("@Name", task.Name);
            command.Parameters.AddWithValue("@Description", task.Description);
            command.Parameters.AddWithValue("@Status", task.Status);
            command.Parameters.AddWithValue("@Priority", task.Priority);
            command.Parameters.AddWithValue("@ProjectId", task.ProjectId);
            command.Parameters.AddWithValue("@OwnerId", task.OwnerId);
            command.Parameters.AddWithValue("@AssigneeId", task.AssigneeId);
            command.Parameters.AddWithValue("@ParentTaskId", task.ParentTaskId);
            command.Parameters.AddWithValue("@Id", task.Id);
            Query = command;
        }

        public void SetDeleteTaskContext(long taskId)
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"DELETE FROM CW_TASK_DETAILS WHERE ID=@TaskId";
            command.Parameters.AddWithValue("@TaskId", taskId);
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
                        UserTask task = new UserTask(Reader.GetInt64(0), Reader.GetString(1), Reader.GetString(2), Reader.GetInt32(3), Reader.GetInt32(4), Reader.GetInt64(5), Reader.GetInt64(6), Reader.GetInt64(7), Reader.GetInt64(8));
                        task.StatusData = new Status(Reader.GetInt64(9), Reader.GetString(10));
                        task.PriorityData = new Priority(Reader.GetInt64(11), Reader.GetString(12), Reader.GetString(13));
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

        public UserTask GetTask()
        {
            UserTask task = null;
            try
            {
                if (Reader != null && Reader.Read())
                {
                    task = new UserTask(Reader.GetInt64(0), Reader.GetString(1), Reader.GetString(2), Reader.GetInt32(3), Reader.GetInt32(4), Reader.GetInt64(5), Reader.GetInt64(6), Reader.GetInt64(7), Reader.GetInt64(8));
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return task;
        }
    }
}
