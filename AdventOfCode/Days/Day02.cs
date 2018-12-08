using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day02 : Day
    {
        public override (Func<string>, Func<string>) GetParts(string path)
        {
            var input = path.ReadLines();
            return (
                () => Part1(input).ToString(),
                () => Part2(input)
            );
        }

        public static int Part1(IEnumerable<string> boxIds)
        {
            var valid = boxIds
                .Select(x =>
                {
                    var groups = x.ToArray().GroupBy(y => y).ToArray();
                    return new
                    {
                        Twos = groups.Count(g => g.Count() == 2) > 0,
                        Threes = groups.Count(g => g.Count() == 3) > 0
                    };
                })
                .Where(x => x.Twos || x.Threes)
                .ToArray();

            return valid.Count(x => x.Twos) * valid.Count(x => x.Threes);
        }

        public static string Part2(IEnumerable<string> boxIds) =>
            boxIds
                .SelectMany(x =>
                    Enumerable.Range(0, x.Length)
                        .Select(y => $"{x.Substring(0, y)}#{x.Substring(y + 1)}"))
                .GroupBy(x => x)
                .First(x => x.Count() == 2)
                .Key
                .Replace("#", "");
    }
}
