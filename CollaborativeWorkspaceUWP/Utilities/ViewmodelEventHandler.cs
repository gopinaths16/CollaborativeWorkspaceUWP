using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace CollaborativeWorkspaceUWP.Utilities
{
    public class ViewmodelEventHandler
    {

        private static readonly Lazy<ViewmodelEventHandler> _instance = new Lazy<ViewmodelEventHandler>(() => new ViewmodelEventHandler());
        private readonly Dictionary<Type, List<Delegate>> _subscribers = new Dictionary<Type, List<Delegate>>();

        public static ViewmodelEventHandler Instance => _instance.Value;

        private ViewmodelEventHandler() { }

        public void Subscribe<T>(Func<T, Task> action)
        {
            if (!_subscribers.ContainsKey(typeof(T)))
            {
                _subscribers[typeof(T)] = new List<Delegate>();
            }
            _subscribers[typeof(T)].Add(action);
        }

        public void Unsubscribe<T>(Func<T, Task> action)
        {
            if (_subscribers.ContainsKey(typeof(T)))
            {
                _subscribers[typeof(T)].Remove(action);
            }
        }

        public async Task Publish<T>(T eventToPublish)
        {
            if (_subscribers.ContainsKey(eventToPublish.GetType()))
            {
                var handlers = _subscribers[eventToPublish.GetType()];
                foreach (var subscriber in handlers)
                {
                    try
                    {
                        var asyncAction = (Func<T, Task>)subscriber;
                        await asyncAction(eventToPublish);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }
    }
}
