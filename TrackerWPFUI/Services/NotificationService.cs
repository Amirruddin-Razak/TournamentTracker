using System;
using System.Collections.Concurrent;
using System.Linq;

namespace TrackerWPFUI.Services
{
    public class NotificationService : INotificationService
    {
        private ConcurrentDictionary<Type, ConcurrentDictionary<object, Action<object>>> _subscriptions = new
            ConcurrentDictionary<Type, ConcurrentDictionary<object, Action<object>>>();

        public void Subscribe<TMessage>(object subscriberObject, Action<object> subscriberMethodToCall)
        {
            Type messageType = typeof(TMessage);

            if (!_subscriptions.ContainsKey(messageType))
            {
                _subscriptions.TryAdd(messageType, new ConcurrentDictionary<object, Action<object>>());
            }

            _subscriptions[messageType].TryAdd(subscriberObject, subscriberMethodToCall);
        }

        public void Unsubscribe<TMessage>(object subscriberObject)
        {
            Type messageType = typeof(TMessage);

            if (!_subscriptions.ContainsKey(messageType))
            {
                return;
            }

            _subscriptions[messageType].TryRemove(subscriberObject, out _);

            if (!_subscriptions[messageType].Any())
            {
                _subscriptions.TryRemove(messageType, out _);
            }
        }

        public void Notify<TMessage>(TMessage message)
        {
            Type messageType = typeof(TMessage);

            if (message == null)
            {
                throw new ArgumentNullException();
            }

            if (_subscriptions.ContainsKey(messageType) == false)
            {
                return;
            }

            foreach (Action<object> methodToCall in _subscriptions[messageType].Values)
            {
                methodToCall?.Invoke(message);
            }
        }
    }
}
