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
            command.CommandText = @"INSERT INTO CW_TASK_DETAILS(NAME, DESCRIPTION, STATUS, PRIORITY, PROJECTID, OWNERID, ASSIGNEEID, PARENT_TASK_ID, TASK_ORDER, DUE_DATE, GROUP_ID) VALUES(@Name, @Description, @Status, @Priority, @ProjectId, @OwnerId, @AssigneeId, @ParentTaskId, @TaskOrder, @DueDate, @GroupId) RETURNING ID, NAME, DESCRIPTION, STATUS, PRIORITY, PROJECTID, OWNERID, ASSIGNEEID, PARENT_TASK_ID, MODIFIED_TIME, TASK_ORDER, DUE_DATE, GROUP_ID";
            command.Parameters.AddWithValue("@Name", task.Name);
            command.Parameters.AddWithValue("@Description", task.Description);
            command.Parameters.AddWithValue("@Status", task.Status);
            command.Parameters.AddWithValue("@Priority", task.Priority);
            command.Parameters.AddWithValue("@ProjectId", task.ProjectId);
            command.Parameters.AddWithValue("@OwnerId", task.OwnerId);
            command.Parameters.AddWithValue("@AssigneeId", task.AssigneeId);
            command.Parameters.AddWithValue("@ParentTaskId", task.ParentTaskId);
            command.Parameters.AddWithValue("@TaskOrder", task.Order);
            command.Parameters.AddWithValue("@DueDate", task.DueDate);
            command.Parameters.AddWithValue("@GroupId", task.GroupId);
            Query = command;
        }

        public void SetGetAllTasksContext()
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"SELECT CWT.ID, CWT.NAME, CWT.DESCRIPTION, CWT.STATUS, CWT.PRIORITY, CWT.PROJECTID, CWT.OWNERID, CWT.ASSIGNEEID, CWT.PARENT_TASK_ID, CWT.MODIFIED_TIME, CS.ID, CS.NAME, CP.ID, CP.NAME, CP.COLOR_CODE, CUD.ID, CUD.USERNAME, CUD.DISPLAYNAME, CWT.TASK_ORDER, CWT.DUE_DATE, CS.COLOR_CODE, CWT.GROUP_ID FROM CW_TASK_DETAILS AS CWT JOIN CW_STATUS AS CS ON CWT.STATUS=CS.ID JOIN CW_PRIORITY AS CP ON CWT.PRIORITY=CP.ID JOIN CW_USER_DETAILS AS CUD ON CWT.OWNERID=CUD.ID ORDER BY CWT.TASK_ORDER ASC";
            Query = command;
        }

        public void SetGetTasksForProjectContext(long projectId)
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"SELECT CWT.ID, CWT.NAME, CWT.DESCRIPTION, CWT.STATUS, CWT.PRIORITY, CWT.PROJECTID, CWT.OWNERID, CWT.ASSIGNEEID, CWT.PARENT_TASK_ID, CWT.MODIFIED_TIME, CS.ID, CS.NAME, CP.ID, CP.NAME, CP.COLOR_CODE, CUD.ID, CUD.USERNAME, CUD.DISPLAYNAME, CWT.TASK_ORDER, CWT.DUE_DATE, CS.COLOR_CODE, CWT.GROUP_ID FROM CW_TASK_DETAILS AS CWT JOIN CW_STATUS AS CS ON CWT.STATUS=CS.ID JOIN CW_PRIORITY AS CP ON CWT.PRIORITY=CP.ID JOIN CW_USER_DETAILS AS CUD ON CWT.OWNERID=CUD.ID WHERE CWT.PROJECTID=@ProjectId ORDER BY CWT.TASK_ORDER ASC";
            command.Parameters.AddWithValue("@ProjectId", projectId);
            Query = command;
        }

        public void SetGetNonSubTasksContext(long taskId, long projectId)
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"SELECT CWT.ID, CWT.NAME, CWT.DESCRIPTION, CWT.STATUS, CWT.PRIORITY, CWT.PROJECTID, CWT.OWNERID, CWT.ASSIGNEEID, CWT.PARENT_TASK_ID, CWT.MODIFIED_TIME, CS.ID, CS.NAME, CP.ID, CP.NAME, CP.COLOR_CODE, CUD.ID, CUD.USERNAME, CUD.DISPLAYNAME, CWT.TASK_ORDER, CWT.DUE_DATE, CS.COLOR_CODE, CWT.GROUP_ID FROM CW_TASK_DETAILS AS CWT JOIN CW_STATUS AS CS ON CWT.STATUS=CS.ID JOIN CW_PRIORITY AS CP ON CWT.PRIORITY=CP.ID JOIN CW_USER_DETAILS AS CUD ON CWT.OWNERID=CUD.ID WHERE CWT.PROJECTID=@ProjectId AND CWT.PARENT_TASK_ID=0 AND CWT.ID != @Id ORDER BY CWT.TASK_ORDER ASC";
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
            command.CommandText = @"SELECT CWT.ID, CWT.NAME, CWT.DESCRIPTION, CWT.STATUS, CWT.PRIORITY, CWT.PROJECTID, CWT.OWNERID, CWT.ASSIGNEEID, CWT.PARENT_TASK_ID, CWT.MODIFIED_TIME, CS.ID, CS.NAME, CP.ID, CP.NAME, CP.COLOR_CODE, CUD.ID, CUD.USERNAME, CUD.DISPLAYNAME, CWT.TASK_ORDER, CWT.DUE_DATE, CS.COLOR_CODE, CWT.GROUP_ID FROM CW_TASK_DETAILS AS CWT JOIN CW_STATUS AS CS ON CWT.STATUS=CS.ID JOIN CW_PRIORITY AS CP ON CWT.PRIORITY=CP.ID JOIN CW_USER_DETAILS AS CUD ON CWT.OWNERID=CUD.ID WHERE CWT.PARENT_TASK_ID=@ParentTaskId ORDER BY CWT.TASK_ORDER ASC";
            command.Parameters.AddWithValue("@ParentTaskId", parentTaskId);
            Query = command;
        }

        public void SetUpdateDescriptionContext(UserTask task)
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"UPDATE CW_TASK_DETAILS SET NAME=@Name, DESCRIPTION=@Description, STATUS=@Status, PRIORITY=@Priority, PROJECTID=@ProjectId, OWNERID=@OwnerId, ASSIGNEEID=@AssigneeId, PARENT_TASK_ID=@ParentTaskId, MODIFIED_TIME=@ModifiedTime, DUE_DATE=@DueDate, GROUP_ID=@GroupId WHERE ID=@Id RETURNING ID, NAME, DESCRIPTION, STATUS, PRIORITY, PROJECTID, OWNERID, ASSIGNEEID, PARENT_TASK_ID, MODIFIED_TIME, TASK_ORDER, DUE_DATE, GROUP_ID";
            command.Parameters.AddWithValue("@Name", task.Name);
            command.Parameters.AddWithValue("@Description", task.Description);
            command.Parameters.AddWithValue("@Status", task.Status);
            command.Parameters.AddWithValue("@Priority", task.Priority);
            command.Parameters.AddWithValue("@ProjectId", task.ProjectId);
            command.Parameters.AddWithValue("@OwnerId", task.OwnerId);
            command.Parameters.AddWithValue("@AssigneeId", task.AssigneeId);
            command.Parameters.AddWithValue("@ParentTaskId", task.ParentTaskId);
            command.Parameters.AddWithValue("@Id", task.Id);
            command.Parameters.AddWithValue("@ModifiedTime", DateTime.Now);
            command.Parameters.AddWithValue("@DueDate", task.DueDate);
            command.Parameters.AddWithValue("@GroupId", task.GroupId);
            Query = command;
        }

        public void SetDeleteTaskContext(long taskId)
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"DELETE FROM CW_TASK_DETAILS WHERE ID=@TaskId";
            command.Parameters.AddWithValue("@TaskId", taskId);
            Query = command;
        }

        public void SetUpdateOrderContext(UserTask task)
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"UPDATE CW_TASK_DETAILS SET TASK_ORDER=@Order WHERE ID=@Id";
            command.Parameters.AddWithValue("@Order", task.Order);
            command.Parameters.AddWithValue("@Id", task.Id);
            Query = command;
        }

        public void SetUpdateOrderForTasksContext(int start, int end, int value, long taskId)
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"UPDATE CW_TASK_DETAILS SET TASK_ORDER=TASK_ORDER+@Value WHERE TASK_ORDER >= @Start AND TASK_ORDER <= @End AND ID!=@Id";
            command.Parameters.AddWithValue("@Value", value);
            command.Parameters.AddWithValue("@Start", start);
            command.Parameters.AddWithValue("@End", end);
            command.Parameters.AddWithValue("@Id", taskId);
            Query = command;
        }

        public void SetGetTasksForGroupContext(long groupId)
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = @"SELECT CWT.ID, CWT.NAME, CWT.DESCRIPTION, CWT.STATUS, CWT.PRIORITY, CWT.PROJECTID, CWT.OWNERID, CWT.ASSIGNEEID, CWT.PARENT_TASK_ID, CWT.MODIFIED_TIME, CS.ID, CS.NAME, CP.ID, CP.NAME, CP.COLOR_CODE, CUD.ID, CUD.USERNAME, CUD.DISPLAYNAME, CWT.TASK_ORDER, CWT.DUE_DATE, CS.COLOR_CODE, CWT.GROUP_ID FROM CW_TASK_DETAILS AS CWT JOIN CW_STATUS AS CS ON CWT.STATUS=CS.ID JOIN CW_PRIORITY AS CP ON CWT.PRIORITY=CP.ID JOIN CW_USER_DETAILS AS CUD ON CWT.OWNERID=CUD.ID WHERE CWT.GROUP_ID=@GroupId ORDER BY CWT.TASK_ORDER ASC";
            command.Parameters.AddWithValue("@GroupId", groupId);
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
                        if(!Reader.IsDBNull(9))
                        {
                            task.SetModifiedTime(Reader.GetDateTime(9));
                        }
                        task.StatusData = new Status(Reader.GetInt64(10), Reader.GetString(11), Reader.GetString(20));
                        task.PriorityData = new Priority(Reader.GetInt64(12), Reader.GetString(13), Reader.GetString(14));
                        task.Owner = new User() { Id = Reader.GetInt64(15), Username = Reader.GetString(16), DisplayName = Reader.GetString(17) };
                        task.Order = Reader.GetInt32(18);
                        if(!Reader.IsDBNull(19))
                        {
                            task.DueDate = Reader.GetDateTime(19);
                        }
                        task.GroupId = Reader.GetInt64(21);
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
                    if (!Reader.IsDBNull(9))
                    {
                        task.SetModifiedTime(Reader.GetDateTime(9));
                    }
                    task.Order = Reader.GetInt32(10);
                    if(!Reader.IsDBNull(11))
                    {
                        task.DueDate = Reader.GetDateTime(11);
                    }
                    task.GroupId = Reader.GetInt64(12);
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
