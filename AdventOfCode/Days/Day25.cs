using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day25 : Day
    {
        public override (Func<string>, Func<string>) GetParts(string path)
        {
            var input = path.ReadLines();
            return (
                () => Part1(input).ToString(),
                () => string.Empty
            );
        }

        public static int Part1(IEnumerable<string> input)
        {
            var stars = input
                .Select(ParseStar)
                .OrderBy(x => x.X)
                .ThenBy(x => x.Y)
                .ThenBy(x => x.Z)
                .ThenBy(x => x.T)
                .ToList();
            var constellations = new List<HashSet<(int X, int Y, int Z, int T)>>();
            while (stars.Count > 0)
            {
                var current = stars.First();
                var constellation = constellations
                    .FirstOrDefault(c => c.Any(s => Distance(s, current) <= 3));
                if (constellation == null)
                {
                    constellation = new HashSet<(int X, int Y, int Z, int T)>();
                    constellations.Add(constellation);
                }
                constellation.Add(current);
                stars.Remove(current);
                FindMore(constellation, stars, stars.Where(s => Distance(s, current) <= 3).ToList());
            }

            return constellations.Count;
        }

        private static void FindMore(
            ISet<(int X, int Y, int Z, int T)> constellation,
            ICollection<(int X, int Y, int Z, int T)> stars,
            IReadOnlyCollection<(int X, int Y, int Z, int T)> others)
        {
            foreach (var star in others)
            {
                constellation.Add(star);
                stars.Remove(star);
            }

            foreach (var star in others)
                FindMore(constellation, stars, stars.Where(s => Distance(s, star) <= 3).ToList());
        }

        private static int Distance((int X, int Y, int Z, int T) s1, (int X, int Y, int Z, int T) s2) =>
            Math.Abs(s1.X - s2.X) + Math.Abs(s1.Y - s2.Y) + Math.Abs(s1.Z - s2.Z) + Math.Abs(s1.T - s2.T);

        private static (int X, int Y, int Z, int T) ParseStar(string line)
        {
            var parts = line.SplitAsInt(",").ToArray();
            return (parts[0], parts[1], parts[2], parts[3]);
        }
    }
}
