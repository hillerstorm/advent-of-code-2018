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
    }
}
