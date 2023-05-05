using System;
using System.Collections.Generic;
using UnityEditor;

namespace Dvs.EventsManager
{
    public static class Events
    {
        private static Dictionary<Type, Action> Listeners
            = new Dictionary<Type, Action>();

        private static void Setup<T>()
        {
            if (Listeners.ContainsKey(typeof(T))) return;

            Action action = () => { };
            Listeners[typeof(T)] = action;
        }

        public static void ListenTo<T>(Action listener)
        {
            Setup<T>();
            Listeners[typeof(T)] += listener;
        }

        public static void StopListeningTo<T>(Action listener)
        {
            Setup<T>();
            Listeners[typeof(T)] -= listener;
        }

        public static void Trigger<T>()
        {
            Setup<T>();
            Listeners[typeof(T)].Invoke();
        }
    }
}