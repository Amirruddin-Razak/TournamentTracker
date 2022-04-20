using System;

namespace TrackerWPFUI.Services
{
    public interface INotificationService
    {
        void Notify<TMessage>(TMessage message);
        void Subscribe<TMessage>(object subscriberObject, Action<object> subscriberMethodToCall);
        void Unsubscribe<TMessage>(object subscriberObject);
    }
}