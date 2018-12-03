using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        public static IEnumerable<T> Cyclic<T>(this IEnumerable<T> source)
        {
            IEnumerator<T> enu = null;
            try
            {
                enu = source.GetEnumerator();
                if (!enu.MoveNext())
                    yield break;
                while (true)
                {
                    yield return enu.Current;
                    if (enu.MoveNext())
                        continue;
                    enu.Dispose();
                    enu = source.GetEnumerator();
                    enu.MoveNext();
                }
            }
            finally
            {
                enu?.Dispose();
            }
        }

        public static IEnumerable<(int X, int Y)> Square(int x, int y, int width, int height) =>
            Enumerable.Range(x, width).Pairs(Enumerable.Range(y, height));

        public static IEnumerable<(T1, T2)> Pairs<T1, T2>(this IEnumerable<T1> source, IEnumerable<T2> other) =>
            source.SelectMany(x => other.Select(y => (x, y)));
    }
}
