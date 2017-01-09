using System;
using System.Collections.Generic;
using System.Linq;

namespace AvalonAssets.Core.Utility
{
    public static class EnumUtils
    {
        /// <summary>
        ///     Gets all the values of enum <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">Enum type.</typeparam>
        /// <returns>All enum value of <typeparamref name="T" /></returns>
        public static IEnumerable<T> Values<T>() where T : struct
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        /// <summary>
        ///     Shifts the <paramref name="enumerable" /> by <paramref name="offset" />.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="enumerable">Enumerable to be shifted.</param>
        /// <param name="offset">Amount of shift.</param>
        /// <returns>Shifted enumerable.</returns>
        public static IEnumerable<T> Shift<T>(this IEnumerable<T> enumerable, int offset)
        {
            var list = enumerable.ToList();
            // Shift backward
            while (offset < 0)
                offset += list.Count;
            offset %= list.Count;
            var count = list.Count - offset;
            var subList = list.GetRange(offset, count);
            list.RemoveRange(offset, count);
            subList.AddRange(list);
            return subList;
        }
    }
}