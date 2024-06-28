using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Utilities.Comm
{
    public sealed class ViewModelEventManager : IEventManager
    {
        private static ViewModelEventManager viewModelEventManager;

        private readonly Dictionary<Type, List<Delegate>> subscribers = new Dictionary<Type, List<Delegate>>();
        private static object lockObj = new Object();

        public static ViewModelEventManager Instance
        {
            get
            {
                if(viewModelEventManager == null)
                {
                    lock(lockObj)
                    {
                        if(viewModelEventManager == null)
                        {
                            viewModelEventManager = new ViewModelEventManager();
                        }
                    }
                }
                return viewModelEventManager;
            }
        }

        public ViewModelEventManager() {}

        public void Subscribe<T>(Action<T> action)
        {
            var messageType = typeof(T);
            if (!subscribers.ContainsKey(messageType))
            {
                subscribers[messageType] = new List<Delegate>();
            }
            subscribers[messageType].Add(action);
        }

        public void Unsubscribe<T>(Action<T> action)
        {
            var messageType = typeof(T);
            if (subscribers.ContainsKey(messageType))
            {
                subscribers[messageType].Remove(action);
            }
        }

        public void Publish<T>(T message)
        {
            var messageType = typeof(T);
            if (subscribers.ContainsKey(messageType))
            {
                foreach (var subscriber in subscribers[messageType])
                {
                    ((Action<T>)subscriber)(message);
                }
            }
        }
    }
}
