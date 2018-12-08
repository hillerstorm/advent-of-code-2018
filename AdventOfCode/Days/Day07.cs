using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day07 : Day
    {
        public override (Func<string>, Func<string>) GetParts(string path)
        {
            var input = path.ReadLines();
            return (
                () => Part1(input),
                () => Part2(input).ToString()
            );
        }

        public static string Part1(IEnumerable<string> input) =>
            Calc(input).Order;

        public static int Part2(IEnumerable<string> input, int workerCount = 5, int durationDiff = -4) =>
            Calc(input, workerCount, durationDiff).Duration;

        private static (string Order, int Duration) Calc(IEnumerable<string> input, int workerCount = 1, int durationDiff = -64)
        {
            var constraints = ParseConstraints(input).ToArray();
            var byPrerequisite = constraints
                .GroupBy(x => x.Prerequisite, x => x.Step)
                .ToDictionary(x => x.Key, x => x.ToList());
            var byStep = constraints
                .GroupBy(x => x.Step, x => x.Prerequisite)
                .ToDictionary(x => x.Key, x => x.ToList());
            var queue = byPrerequisite.Keys
                .Where(x => !byStep.ContainsKey(x))
                .OrderBy(x => x)
                .ToImmutableSortedSet();
            var workers = new (string Step, int Duration)[workerCount];
            var completed = string.Empty;
            var ticks = 0;
            while (queue.Count > 0 || workers.Any(x => !string.IsNullOrEmpty(x.Step)))
            {
                for (var i = 0; i < workers.Length; i++)
                {
                    if (string.IsNullOrEmpty(workers[i].Step) || --workers[i].Duration != 0)
                        continue;

                    completed += workers[i].Step;

                    if (byPrerequisite.ContainsKey(workers[i].Step))
                        queue = queue.Union(byPrerequisite[workers[i].Step]);

                    workers[i].Step = string.Empty;
                }

                for (var i = 0; i < workers.Length; i++)
                {
                    if (!string.IsNullOrEmpty(workers[i].Step))
                        continue;

                    var next = queue
                        .FirstOrDefault(x => !byStep.ContainsKey(x) || byStep[x].All(completed.Contains));

                    if (string.IsNullOrEmpty(next))
                        continue;

                    workers[i] = (next, (int) next[0] + durationDiff);
                    queue = queue.Remove(next);
                }

                ticks++;
            }

            return (completed, ticks - 1);
        }

        private static IEnumerable<(string Prerequisite, string Step)> ParseConstraints(IEnumerable<string> input) =>
            input
                .Select(x =>
                {
                    var parts = x.Split(" ");
                    return (parts[1], parts[7]);
                });
    }
}
