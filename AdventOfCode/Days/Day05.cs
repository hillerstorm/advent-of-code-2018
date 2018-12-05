using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Days
{
    public class Day05 : Day
    {
        private static readonly Regex Regexp = new Regex(@"(\p{L})((?!\1)(?i:\1))");

        public override (Func<string>, Func<string>) GetParts(string path)
        {
            var input = path.ReadInput();
            return (
                () => Part1(input).ToString(),
                () => Part2(input).ToString()
            );
        }

        public static int Part1(string input)
        {
            while (Regexp.IsMatch(input))
                input = Regexp.Replace(input, string.Empty);
            return input.Length;
        }

        public static int Part2(string input) =>
            input
                .ToLower()
                .ToHashSet()
                .AsParallel()
                .Select(x =>
                    Part1(input.Replace(x.ToString(), string.Empty, StringComparison.InvariantCultureIgnoreCase)))
                .OrderBy(x => x)
                .First();
    }
}
