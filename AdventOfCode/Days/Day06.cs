using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day06 : Day
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
            var coords = ParseCoords(input).ToArray();
            var minX = coords.Min(x => x.X);
            var maxX = coords.Max(x => x.X);
            var minY = coords.Min(x => x.Y);
            var maxY = coords.Max(x => x.Y);
            return Extensions.Square(minX, minY, Math.Abs(maxX - minX) + 1, Math.Abs(maxY - minY) + 1)
                .Select(x => (
                    x,
                    coords
                        .Select(y => (y, Distance(x, y)))
                        .GroupBy(y => y.Item2)
                        .OrderBy(y => y.Key)
                        .First()
                        .Select(y => y.Item1)
                        .ToArray()
                ))
                .Where(x => x.Item2.Length == 1)
                .Select(x => (x.Item1, x.Item2[0]))
                .GroupBy(x => x.Item2)
                .Where(x => x.All(y => y.Item1.X > minX && y.Item1.X < maxX && y.Item1.Y > minY && y.Item1.Y < maxY))
                .Select(x => x.Count())
                .OrderBy(x => x)
                .Last();
        }

        public static int Part2(IEnumerable<string> input, int maxDistance = 10000)
        {
            var coords = ParseCoords(input).ToArray();
            var minX = coords.Min(x => x.X);
            var maxX = coords.Max(x => x.X);
            var minY = coords.Min(x => x.Y);
            var maxY = coords.Max(x => x.Y);
            return Extensions.Square(minX, minY, Math.Abs(maxX - minX) + 1, Math.Abs(maxY - minY) + 1)
                .Select(x => coords.Sum(y => Distance(x, y)))
                .Count(x => x < maxDistance);
        }

        private static int Distance((int X, int Y) source, (int X, int Y) other) =>
            Math.Abs(source.X - other.X) + Math.Abs(source.Y - other.Y);

        private static IEnumerable<(int X, int Y)> ParseCoords(IEnumerable<string> input) =>
            input
                .Select(x =>
                {
                    var coords = x.Split(", ");
                    return (int.Parse(coords[0]), int.Parse(coords[1]));
                });
    }
}
