namespace Dvs.ObservableLocator
{
    using System;
    using UnityEngine;

    /// <summary>
    /// This is only used to Cleanup after a locator auto generates a reference in order to clean it out of locator on destruction.
    /// </summary>
    public class LocatorInstanceCleanup : MonoBehaviour
    {
        public Action OnDestruction;

        private void OnDestroy()
        {
            OnDestruction();
        }
    }
}
