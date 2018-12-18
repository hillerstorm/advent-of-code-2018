using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day18 : Day
    {
        public override (Func<string>, Func<string>) GetParts(string path)
        {
            var input = path.ReadLines();
            return (
                () => Part1(input).ToString(),
                () => Part2(input).ToString()
            );
        }

        public static int Part1(IEnumerable<string> input) =>
            Calc(input, findLoop: false);

        public static int Part2(IEnumerable<string> input) =>
            Calc(input, findLoop: true);

        private static int Calc(IEnumerable<string> input, bool findLoop)
        {
            var rows = input.Select(x => x.ToCharArray()).ToArray();
            var width = rows[0].Length;
            var height = rows.Length;
            var grid = rows.SelectMany(x => x).ToArray();
            var neighbours = grid.Select((chr, i) =>
            {
                var x = i % width;
                var y = i / height;
                return new[]
                    {
                        (x - 1, y - 1),
                        (x, y - 1),
                        (x + 1, y - 1),
                        (x - 1, y),
                        (x + 1, y),
                        (x - 1, y + 1),
                        (x, y + 1),
                        (x + 1, y + 1),
                    }.Where(pos => pos.Item1 >= 0 && pos.Item2 >= 0 && pos.Item1 < width && pos.Item2 < height)
                    .Select(n => width * n.Item2 + n.Item1)
                    .ToArray();
            }).ToArray();

            var firstDupe = -1;
            var findOneMore = false;
            var sums = new HashSet<int>();
            var maxMinutes = findLoop ? 1000000000 : 10;
            for (long min = 0; min < maxMinutes; min++)
            {
                var copy = new char[grid.Length];
                for (var i = 0; i < grid.Length; i++)
                {
                    switch (grid[i])
                    {
                        case '.' when neighbours[i].Count(n => grid[n] == '|') > 2:
                            copy[i] = '|';
                            break;
                        case '|' when neighbours[i].Count(n => grid[n] == '#') > 2:
                            copy[i] = '#';
                            break;
                        case '#' when neighbours[i].Any(n => grid[n] == '|') &&
                                      neighbours[i].Any(n => grid[n] == '#'):
                            copy[i] = grid[i];
                            break;
                        case '#':
                            copy[i] = '.';
                            break;
                        default:
                            copy[i] = grid[i];
                            break;
                    }
                }

                grid = copy;

                if (!findLoop)
                    continue;

                var sum = grid.Count(x => x == '#') * grid.Count(x => x == '|');
                var added = sums.Add(sum);
                if (findOneMore && !added)
                {
                    var tmp = maxMinutes - (min - sums.Count);
                    return sums.ToArray()[tmp % 28];
                }

                if (findOneMore)
                {
                    firstDupe = sum;
                    sums = new HashSet<int>(new[] {sum});
                    findOneMore = false;
                    continue;
                }

                if (added || firstDupe != -1 && sum == firstDupe)
                    continue;

                firstDupe = sum;
                var test = sums.ToList();
                sums = sums.ToArray().AsSpan(test.IndexOf(sum)).ToArray().ToHashSet();
                findOneMore = true;
            }

            return grid.Count(x => x == '#') * grid.Count(x => x == '|');
        }
    }
}
