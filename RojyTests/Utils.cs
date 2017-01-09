using System;
using System.Collections.Generic;

namespace AvalonAssets.RojyTests
{
    internal static class Utils
    {
        public static IList<int> UniqueNumberList(this Random rand, int count = 10)
        {
            var nodes = new List<int>();
            for (var i = 0; i < count; i++)
            {
                var num = rand.Next();
                while (nodes.Contains(num))
                    num = rand.Next();
                nodes.Add(num);
            }
            return nodes;
        }

        public static Queue<int> UniqueNumberQueue(this Random rand, int count = 10)
        {
            var nodes = new Queue<int>();
            foreach (var num in UniqueNumberList(rand, count))
                nodes.Enqueue(num);
            return nodes;
        }
    }
}