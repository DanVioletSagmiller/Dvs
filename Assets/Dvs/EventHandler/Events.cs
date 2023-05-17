// https://github.com/DanVioletSagmiller/Dvs

// Copyright © 2023 Dan Violet Sagmiller

// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the “Software”), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute,
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.

// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
// NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Dvs
{
    using System;
    using System.Collections.Generic;

    public static class Events
    {
        private static Dictionary<Type, object> Listeners
            = new Dictionary<Type, object>();

        private static void Setup<T>()
        {
            if (Listeners.ContainsKey(typeof(T))) return;

            Action<T> action = (T t) => { };
            Listeners[typeof(T)] = action;
        }

        public static void ListenTo<T>(Action<T> listener)
        {
            Setup<T>();
            Action<T> action = (Action<T>)Listeners[typeof(T)];
            action += listener;
            Listeners[typeof(T)] = action;
        }

        public static void StopListeningTo<T>(Action<T> listener)
        {
            Setup<T>();
            Action<T> action = (Action<T>)Listeners[typeof(T)];
            action -= listener;
            Listeners[typeof(T)] = action;
        }

        public static void Trigger<T>(T value)
        {
            Setup<T>();
            Action<T> action = (Action<T>)Listeners[typeof(T)];
            action(value);
        }
    }
}