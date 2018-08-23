using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace VAllens.LookupTable
{
    public static class LinqExtensions
    {
        /// <summary>
        /// 根据指定比较找出交集
        /// </summary>
        public static IEnumerable<T> Intersect<T, V>(this IEnumerable<T> source, IEnumerable<T> second,
            Func<T, V> keySelector)
        {
            return source.Intersect(second, new CommonEqualityComparer<T, V>(keySelector));
        }

        /// <summary>
        /// 根据指定比较找出交集
        /// </summary>
        public static IEnumerable<T> Intersect<T, V>(this IEnumerable<T> source, IEnumerable<T> second,
            Func<T, V> keySelector, IEqualityComparer<V> comparer)
        {
            return source.Intersect(second, new CommonEqualityComparer<T, V>(keySelector, comparer));
        }

        /// <summary>
        /// 清空线程安全队列集合
        /// </summary>
        public static void Clear<T>(this ConcurrentQueue<T> queue)
        {
            T item;
            while (queue.TryDequeue(out item))
            {
                // do nothing
            }
        }
    }
}