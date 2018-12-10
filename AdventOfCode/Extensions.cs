using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nito.Collections;

namespace AdventOfCode
{
    public static class Extensions
    {
        public static string ReadInput(this string path) =>
            File.ReadAllText(path);

        public static IEnumerable<string> ReadLines(this string path) =>
            File.ReadLines(path);

        public static IEnumerable<int> ReadLinesAsInt(this string path) =>
            File.ReadLines(path).Select(int.Parse);

        public static IEnumerable<int> SplitAsInt(this string input, string separator = ", ") =>
            input.Split(separator).Select(int.Parse);

        public static IEnumerable<T> Cyclic<T>(this IEnumerable<T> source)
        {
            var arr = source.ToArray();
            var i = 0;
            while (true)
            {
                yield return arr[i % arr.Length];
                i++;
            }
            // ReSharper disable once IteratorNeverReturns
        }

        public static IEnumerable<(int X, int Y)> Square(int x, int y, int width, int height) =>
            x.To(width).Pairs(y.To(height));

        private static IEnumerable<(T1, T2)> Pairs<T1, T2>(this IEnumerable<T1> source, IEnumerable<T2> other) =>
            source.SelectMany(x => other.Select(y => (x, y)));

        public static IEnumerable<int> To(this int from, int max) =>
            Enumerable.Range(from, max);

        public static void Rotate<T>(this Deque<T> source, long offset)
        {
            if (offset < 0)
            {
                offset = Math.Abs(offset);
                for (long i = 0; i < offset; i++)
                    source.AddToFront(source.RemoveFromBack());
            }
            else if (offset > 0)
                for (long i = 0; i < offset; i++)
                    source.AddToBack(source.RemoveFromFront());
        }

        public static IEnumerable<IEnumerable<T1>> PartitionBy<T1>(this IEnumerable<T1> source, int width)
        {
            var entries = source.LongCount() / width;
            for (var i = 0; i < entries; i++)
                yield return source.Skip(i * width).Take(width);
        }
    }
}
