using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day04 : Day
    {
        public override (string, string) Run(string path)
        {
            var input = ParseTimestamps(path.ReadLines());
            return (Part1(input).ToString(), Part2(input).ToString());
        }

        public static int Part1(IEnumerable<(int Id, HashSet<int> Asleep)> shifts) =>
            shifts
                .GroupBy(x => x.Id, x => x.Asleep)
                .Select(x => (
                    x.Key * x.SelectMany(y => y)
                        .Distinct()
                        .Select(y => (
                            y,
                            x.Count(z => z.Contains(y))
                        ))
                        .OrderBy(y => y.Item2)
                        .Last()
                        .Item1,
                    x.Sum(y => y.Count)
                ))
                .OrderBy(x => x.Item2)
                .Last()
                .Item1;

        public static int Part2(IEnumerable<(int Id, HashSet<int> Asleep)> shifts) =>
            shifts
                .GroupBy(x => x.Id, x => x.Asleep)
                .Select(x =>
                    x.SelectMany(y => y)
                        .Distinct()
                        .Select(y => (
                            x.Key * y,
                            x.Count(z => z.Contains(y))
                        ))
                        .OrderBy(y => y.Item2)
                        .Last()
                )
                .OrderBy(x => x.Item2)
                .Last()
                .Item1;

        public static IEnumerable<(int Id, HashSet<int> Asleep)> ParseTimestamps(IEnumerable<string> lines)
        {
            var asleep = new HashSet<int>();
            int? currentGuardId = null;
            DateTime? fellAsleep = null;

            foreach (var (line, timestamp) in lines
                .Select(x => (x, DateTime.Parse(string.Concat(x.Skip(1).TakeWhile(c => c != ']')))))
                .OrderBy(x => x.Item2))
            {
                if (line.Contains("falls"))
                    fellAsleep = timestamp;
                else if (line.Contains("wakes") && fellAsleep.HasValue)
                {
                    for (var toAdd = 0; toAdd < (int) (timestamp - fellAsleep.Value).TotalMinutes; toAdd++)
                        asleep.Add(fellAsleep.Value.AddMinutes(toAdd).Minute);

                    fellAsleep = null;
                }
                else if (line.Contains("Guard"))
                {
                    if (currentGuardId.HasValue && asleep.Count > 0)
                        yield return (currentGuardId.Value, asleep);

                    currentGuardId =
                        int.Parse(string.Concat(line.SkipWhile(x => x != '#').Skip(1).TakeWhile(char.IsDigit)));
                    asleep = new HashSet<int>();
                    fellAsleep = null;
                }
            }

            if (currentGuardId.HasValue && asleep.Count > 0)
                yield return (currentGuardId.Value, asleep);
        }
    }
}
