using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Tests
{
    public static class Extensions
    {
        public static int[] SplitAsInt(this string input, string separator = ", ") =>
            input.Split(separator).Select(int.Parse).ToArray();

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }
    }
}
