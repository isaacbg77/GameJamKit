using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameJamKit.Utilities
{
    public static class ListExtensions
    {
        public static IReadOnlyList<T> Shuffle<T>(this IReadOnlyList<T> list)
            => list.OrderBy(_ => Random.value).ToList();
    }
}
