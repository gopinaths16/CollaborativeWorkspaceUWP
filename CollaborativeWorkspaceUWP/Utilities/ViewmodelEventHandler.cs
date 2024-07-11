using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Utilities
{
    public class ViewmodelEventHandler
    {

        private static readonly Lazy<ViewmodelEventHandler> _instance = new Lazy<ViewmodelEventHandler>(() => new ViewmodelEventHandler());
        private readonly Dictionary<Type, List<Delegate>> _subscribers = new Dictionary<Type, List<Delegate>>();

        public static ViewmodelEventHandler Instance => _instance.Value;

        private ViewmodelEventHandler() { }

        public void Subscribe<T>(Action<T> action)
        {
            if (!_subscribers.ContainsKey(typeof(T)))
            {
                _subscribers[typeof(T)] = new List<Delegate>();
            }
            _subscribers[typeof(T)].Add(action);
        }

        public void Unsubscribe<T>(Action<T> action)
        {
            if (_subscribers.ContainsKey(typeof(T)))
            {
                _subscribers[typeof(T)].Remove(action);
            }
        }

        public void Publish<T>(T eventToPublish)
        {
            if (_subscribers.ContainsKey(eventToPublish.GetType()))
            {
                foreach (var subscriber in _subscribers[eventToPublish.GetType()])
                {
                    ((Action<T>)subscriber)(eventToPublish);
                }
            }
        }

    }
}
