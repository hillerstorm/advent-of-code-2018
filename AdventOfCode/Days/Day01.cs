using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day01 : Day
    {
        public override (Func<string>, Func<string>) GetParts(string path)
        {
            var input = path.ReadLinesAsInt();
            return (
                () => Part1(input).ToString(),
                () => Part2(input).ToString()
            );
        }

        public static int Part1(IEnumerable<int> input) =>
            input.Sum();

        public static int Part2(IEnumerable<int> input)
        {
            var set = new HashSet<int>(new[]{0});
            var freq = 0;
            foreach (var change in input.Cyclic())
                if (!set.Add(freq += change))
                    break;

            return freq;
        }
    }
}
