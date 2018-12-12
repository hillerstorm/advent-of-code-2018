using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Nito.Collections;

namespace AdventOfCode.Days
{
    public class Day12 : Day
    {
        public override (Func<string>, Func<string>) GetParts(string path)
        {
            var input = path.ReadLines();
            return (
                () => Part1(input).ToString(),
                () => Part2(input).ToString()
            );
        }

        public static long Part1(IEnumerable<string> input) =>
            SumPlants(input, 20);

        public static long Part2(IEnumerable<string> input) =>
            SumPlants(input, 50000000000, true);

        private static long SumPlants(IEnumerable<string> input, long iterations, bool earlyOut = false)
        {
            var inputArray = input.ToArray();
            var plants =
                new Deque<(char State, long Index)>(inputArray[0].Substring(15).Select((x, i) => (x, (long) i)));
            var mutations = ParseMutations(inputArray.Skip(2));
            long totalSum = -1;
            long previousDiff = -1;
            long previousSum = -1;
            for (long i = 0; i < iterations; i++)
            {
                while (plants[0].State == '#' ||
                       plants[1].State == '#' ||
                       plants[2].State == '#' ||
                       plants[3].State == '#')
                {
                    plants.AddToFront(('.', plants[0].Index - 1));
                }

                while (plants[plants.Count - 1].State == '#' ||
                       plants[plants.Count - 2].State == '#' ||
                       plants[plants.Count - 3].State == '#' ||
                       plants[plants.Count - 4].State == '#')
                {
                    plants.AddToBack(('.', plants[plants.Count - 1].Index + 1));
                }

                var clone = plants.Select(x => x.State).ToArray().AsSpan();
                for (var p = 2; p < plants.Count - 2; p++)
                {
                    var key = clone.Slice(p - 2, 5).ToString();
                    if (mutations.ContainsKey(key))
                        plants[p] = (mutations[key], plants[p].Index);
                }

                while (plants[0].State == '.' && plants[1].State == '.' && plants[2].State == '.' && plants[3].State == '.' && plants[4].State == '.')
                    plants.RemoveFromFront();

                if (!earlyOut)
                    continue;

                var sum = plants
                    .Sum(x => x.State == '#' ? x.Index : 0);
                var diff = sum - previousSum;
                if (diff == previousDiff)
                {
                    totalSum = (iterations - i) * diff + previousSum;
                    break;
                }

                previousDiff = diff;
                previousSum = sum;
            }

            return totalSum == -1
                ? plants
                    .Sum(x => x.State == '#' ? x.Index : 0)
                : totalSum;
        }

        private static Dictionary<string, char> ParseMutations(IEnumerable<string> input) =>
            input.Select(x => x.Split(" => "))
                .ToDictionary(x => x[0], x => x[1][0]);
    }
}
