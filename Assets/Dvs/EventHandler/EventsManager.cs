using System;
using System.Collections.Generic;

namespace Dvs.EventHandler
{
    public static class EventHandler
    {
        private static Dictionary<Type, Action> Listeners
            = new Dictionary<Type, Action>();

        private static Action GetAction<T>()
        {
            var key = typeof(T);
            if (Listeners.ContainsKey(key)) return Listeners[key];

            Action action = () => { };
            Listeners[key] = action;
            return action;
        }

        public static void ListenTo<T>(Action listener)
        {
            var action = GetAction<T>();
            action += listener;
        }

        public static void StopListeningTo<T>(Action listener)
        {
            var action = GetAction<T>();
            action -= listener;
        }

        public static void Trigger<T>()
        {
            GetAction<T>()();
        }
    }
}