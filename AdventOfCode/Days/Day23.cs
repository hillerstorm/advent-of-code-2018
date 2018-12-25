using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Days
{
    public class Day23 : Day
    {
        public override (Func<string>, Func<string>) GetParts(string path)
        {
            var input = path.ReadLines();
            return (
                () => Part1(input).ToString(),
                () => Part2(input).ToString()
            );
        }

        public static int Part1(IEnumerable<string> input)
        {
            var bots = ParseInput(input);
            var strongest = bots
                .OrderBy(x => x.R)
                .Last();
            return bots
                .Count(x => Math.Abs(x.X - strongest.X) + Math.Abs(x.Y - strongest.Y) + Math.Abs(x.Z - strongest.Z) <=
                            strongest.R);
        }

        public static int Part2(IEnumerable<string> input) =>
            -1;

        private static readonly Regex pattern = new Regex(@"(\-*\d+)");
        private static (int X, int Y, int Z, int R)[] ParseInput(IEnumerable<string> input)
        {
            return input
                .Select(line => pattern.Matches(line))
                .Where(matches => matches.Count == 4)
                .Select(matches => (
                    int.Parse(matches[0].Value),
                    int.Parse(matches[1].Value),
                    int.Parse(matches[2].Value),
                    int.Parse(matches[3].Value)
                ))
                .ToArray();
        }
    }
}
