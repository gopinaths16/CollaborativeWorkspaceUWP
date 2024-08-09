using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Models.Enums;
using CollaborativeWorkspaceUWP.Persistence.PersistenceObject.EntityPersistence;
using CollaborativeWorkspaceUWP.Utilities.Persistence;
using CollaborativeWorkspaceUWP.Utilities.Persistence.PersistenceObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.DAL
{
    public class UserDataHandler
    {
        private PersistenceObjectManager persistanceObjectManager;

        public UserDataHandler()
        {
            persistanceObjectManager = new PersistenceObjectManager(PersistenceMode.SQLITE);
        }

        public User AddUser(User user)
        {
            User result = null;
            IUserPersistence persistenceObject = null;
            try
            {
                persistenceObject = persistanceObjectManager.GetUserPersistenceObject();
                persistenceObject.SetAddUserContext(user);
                PersistenceHandler.Instance.Get(persistenceObject);
                result = persistenceObject.GetUser();
            }
            catch(Exception ex)
            {

            }
            finally
            {
                if(persistenceObject != null)
                {
                    persistenceObject.Dispose();
                }
            }
            return result;
        }

        public User GetUser(string username, string password)
        {
            User result = null;
            IUserPersistence persistenceObject = null;
            try
            {
                persistenceObject = persistanceObjectManager.GetUserPersistenceObject();
                if (password == null)
                {
                    persistenceObject.SetGetUserContext(username);
                }
                else
                {
                    persistenceObject.SetGetUserContext(username, password);
                }
                PersistenceHandler.Instance.Get(persistenceObject);
                result = persistenceObject.GetUser();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (persistenceObject != null)
                {
                    persistenceObject.Dispose();
                }
            }
            return result;
        }

        public User GetUser(long userId)
        {
            User result = null;
            IUserPersistence persistenceObject = null;
            try
            {
                persistenceObject = persistanceObjectManager.GetUserPersistenceObject();
                persistenceObject.SetGetUserContext(userId);
                PersistenceHandler.Instance.Get(persistenceObject);
                result = persistenceObject.GetUser();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (persistenceObject != null)
                {
                    persistenceObject.Dispose();
                }
            }
            return result;
        }
    }
}
