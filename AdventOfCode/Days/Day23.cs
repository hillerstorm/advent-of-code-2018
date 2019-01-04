using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Z3;

namespace AdventOfCode.Days
{
    public class Day23 : Day
    {
        public override (Func<string>, Func<string>) GetParts(string path)
        {
            var input = path.ReadLines();
            return (
                () => Part1(input).ToString(),
                () => Part2(input)
            );
        }

        public static int Part1(IEnumerable<string> input)
        {
            var bots = ParseInput(input);
            var strongest = bots
                .OrderBy(x => x.R)
                .Last();
            return bots
                .Count(x => Math.Abs(x.X - strongest.X) +
                            Math.Abs(x.Y - strongest.Y) +
                            Math.Abs(x.Z - strongest.Z) <=
                            strongest.R);
        }

        // Original (python) solution by /u/mserrano at
        // https://www.reddit.com/r/adventofcode/comments/a8s17l/2018_day_23_solutions/ecdbux2/
        // Thanks for showing Z3 and what it can do!
        public static string Part2(IEnumerable<string> input)
        {
            var bots = ParseInput(input);
            string result;
            using (var ctx = new Context(new Dictionary<string, string> {{"model", "true"}}))
            {
                T Abs<T>(T expr) where T : ArithExpr =>
                    (T)ctx.MkITE(expr >= 0, expr, -expr);

                var x = ctx.MkIntConst("x");
                var y = ctx.MkIntConst("y");
                var z = ctx.MkIntConst("z");

                var inRanges = bots.Select((_, i) => ctx.MkIntConst($"in_range_{i}")).ToArray();
                var o = ctx.MkOptimize();
                foreach (var ((nx, ny, nz, r), i) in bots.Select((bot, i) => (bot, i)))
                    o.Add(ctx.MkEq(
                        inRanges[i],
                        ctx.MkITE(
                            Abs(x - nx) + Abs(y - ny) + Abs(z - nz) <= r,
                            ctx.MkInt(1),
                            ctx.MkInt(0)
                        ))
                    );

                var rangeCount = ctx.MkIntConst("sum");
                var sum = inRanges.Skip(1).Aggregate<IntExpr, ArithExpr>(
                    inRanges.First(),
                    (a, b) => a + b
                );
                o.Add(ctx.MkEq(rangeCount, sum));

                var distFromZero = ctx.MkIntConst("dist");
                o.Add(ctx.MkEq(distFromZero, Abs(x) + Abs(y) + Abs(z)));

                o.MkMaximize(rangeCount);
                var minDistFromZero = o.MkMinimize(distFromZero);

                if (o.Check() == Status.SATISFIABLE)
                    result = minDistFromZero.Lower.ToString();
                else
                    result = "No result found";

                ctx.Dispose();
            }

            return result;
        }

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
