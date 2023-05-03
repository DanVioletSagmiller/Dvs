
namespace Dvs.ObservableLocator
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public static class Locator
    {
        private enum ReferenceType
        {
            NotSetup,
            HasConstructor,
            HasValue,
        }

        private class Reference<T>
        {
            public Action<T> OnChange = (t) => {};

            public T Value { get; set; }

            public System.Func<T> Construct { get; set; }

            public ReferenceType State { get; set; }
        }

        private static readonly Dictionary<System.Type, object> References
            = new Dictionary<System.Type, object>();

        private static Reference<T> GetReference<T>()
        {
            var key = typeof(T);
            if (References.ContainsKey(key)) return (Reference<T>)References[key];

            var reference = new Reference<T>();
            reference.State = ReferenceType.NotSetup;
            References[key] = reference;
            return reference;
        }
        public static bool HasReference<T>()
        {
            return GetReference<T>().State != ReferenceType.NotSetup;
        }

        public static void Observe<T>(Action<T> whenReady)
        {
            var key = typeof(T);
            var reference = GetReference<T>();
            reference.OnChange += whenReady;

            if (reference.State == ReferenceType.HasConstructor)
            {
                reference.State = ReferenceType.HasValue;
                reference.Value = reference.Construct();
                reference.Construct = null;
            }

            if (reference.State == ReferenceType.HasValue)
            {
                whenReady(reference.Value);
            }
        }

        public static void StopObserving<T>(Action<T> whenReady)
        {
            var reference = GetReference<T>();
            reference.OnChange -= whenReady;
        }

        public static void Set<T>(T instance)
        {
            var reference = GetReference<T>();
            reference.Value = instance;
            reference.State = ReferenceType.HasValue;
            TryToAnnounceChange(reference);
        }

        /// <summary>
        /// Set a default (T)ype to be instantiated automatically for the (I)nterface
        /// </summary>
        /// <typeparam name="I">Interface</typeparam>
        /// <typeparam name="T">Type</typeparam>
        /// <remarks>If T is Scriptable, a new Scriptable Instance will be generated and reused.
        /// If T is Component, a new GameObject "[TypeName]-LocatorInstantiated" will be generated to hold it on construction.
        ///  - the gameobject will also be given a LocatorInstanceCleanup script to notify Locator of its removal.</remarks>
        public static void Set<I, T>() where T : I
        {
            // if the type is a MonoBehaviour, create a game object to hold it. 
            if (typeof(MonoBehaviour).IsAssignableFrom(typeof(T)))
            {
                Set<I>(() =>
                {
                    var go = new GameObject(typeof(T).Name + "-LocatorInstantiated");
                    T component = (T)(object)go.AddComponent(typeof(T));

                    // Add a separate behaviour to announce its destruction and remove it from the locator. 
                    go.AddComponent<LocatorInstanceCleanup>().OnDestruction += () => Locator.Set<T>(null);
                    return component;
                });

                return;
            }

            // if the type is a ScriptableObject, properly create the instance.
            if (typeof(ScriptableObject).IsAssignableFrom(typeof(T)))
            {
                Set<I>(() =>
                {
                    return (T)(object)ScriptableObject.CreateInstance(typeof(T));
                });

                return;
            }

            // if nothing else handled it use of Activator to construct the instance.
            Set<I>(() => { return System.Activator.CreateInstance<T>(); });
        }

        public static void Set<T>(System.Func<T> function)
        {
            var key = typeof(T);
            var reference = GetReference<T>();
            reference.State = ReferenceType.HasConstructor;
            reference.Construct = function;
            TryToAnnounceChange(reference);
        }

        private static void TryToAnnounceChange<T>(Reference<T> reference)
        {
            // check there is a potential value
            if (reference.State == ReferenceType.NotSetup) return;

            // check there are listeners (there is a default)
            if (reference.OnChange.GetInvocationList().Length ==1) return;

            if (reference.State == ReferenceType.HasConstructor)
            {
                reference.Value = reference.Construct();
                reference.Construct = null;
                reference.State = ReferenceType.HasValue;
            }

            reference.OnChange(reference.Value);
        }

        [Obsolete("Only available for testing", false)]
        public static void Clear()
        {
            References.Clear();
        }
    }
}
