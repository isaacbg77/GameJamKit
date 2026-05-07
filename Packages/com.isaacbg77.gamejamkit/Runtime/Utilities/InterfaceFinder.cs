using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameJamKit.Utilities
{
    public static class InterfaceFinder
    {
        public static T? FindInterfaceByType<T>() where T : class
        {
            return Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .OfType<T>()
                .FirstOrDefault();
        }

        public static IEnumerable<T> FindInterfacesByType<T>() where T : class
        {
            return Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .OfType<T>();
        }
    }
}
