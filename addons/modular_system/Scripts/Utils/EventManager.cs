using System;
using System.Collections.Generic;

namespace Modular
{
    public static class EventManager
    {
        private static readonly Dictionary<string, Action<object>> _events =
            new Dictionary<string, Action<object>>();

        public static void Subscribe(string eventName, Action<object> listener)
        {
            if (!_events.ContainsKey(eventName))
            {
                _events[eventName] = listener;
            }
            else
            {
                _events[eventName] += listener;
            }
        }

        public static void Unsubscribe(string eventName, Action<object> listener)
        {
            if (_events.ContainsKey(eventName))
            {
                _events[eventName] -= listener;
            }
        }

        public static void Emit(string eventName, object data = null)
        {
            if (_events.ContainsKey(eventName))
            {
                _events[eventName]?.Invoke(data);
            }
        }
    }
}
