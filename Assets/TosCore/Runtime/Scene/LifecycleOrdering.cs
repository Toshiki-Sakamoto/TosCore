using System;
using System.Collections.Generic;
using System.Linq;

namespace TosCore.Scene
{
    /// <summary>
    /// ILifecyclePrioritizedを保持しているものをソートする
    /// </summary>
    public static class LifecycleOrdering
    {
        public static IReadOnlyList<T> OrderByPriority<T>(IEnumerable<T> source)
        {
            return (source ?? Array.Empty<T>())
                .Where(x => x != null)
                .Select((item, index) => (item, index))
                .OrderBy(x => GetPriority(x.item))
                .ThenBy(x => x.index)
                .Select(x => x.item)
                .ToArray();
        }

        private static int GetPriority<T>(T item)
        {
            return item is ILifecyclePrioritized prioritized ? prioritized.Priority : 0;
        }
    }
}
