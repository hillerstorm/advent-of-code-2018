using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day01 : Day
    {
        public override (string, string) Run()
        {
            var array = "Inputs/01.txt".ReadLinesAsInt().ToArray();
            return (Part1(array).ToString(), Part2(array).ToString());
        }

        public static int Part1(IEnumerable<int> input) =>
            input.Sum();

        public static int Part2(int[] input)
        {
            var map = new HashSet<int>(new[]{0});
            var freq = 0;
            for (var i = 0; ; i = (i + 1) % input.Length)
            {
                freq += input[i];
                if (!map.Add(freq))
                    return freq;
            }
        }
    }
}
