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
    }
}
