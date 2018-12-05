using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day03 : Day
    {
        public struct Claim
        {
            public string Id;
            public IEnumerable<(int X, int Y)> Positions;
        }

        public override (Func<string>, Func<string>) GetParts(string path)
        {
            var input = path.ReadLines();
            return (
                () => Part1(input).ToString(),
                () => Part2(input)
            );
        }

        public static int Part1(IEnumerable<string> input) =>
            GetOverlap(ParseClaims(input)).Count;

        public static string Part2(IEnumerable<string> input)
        {
            var claims = ParseClaims(input);
            var overlap = GetOverlap(claims);
            return claims
                .First(x => !overlap.Overlaps(x.Positions))
                .Id;
        }

        private static IEnumerable<Claim> ParseClaims(
            IEnumerable<string> lines) =>
            lines
                .Select(x => new Claim
                {
                    Id = string.Concat(x.TakeWhile(c => !char.IsWhiteSpace(c))),
                    Positions = Extensions.Square(
                        int.Parse(string.Concat(x.Skip(x.IndexOf("@") + 2).TakeWhile(char.IsDigit))),
                        int.Parse(string.Concat(x.Skip(x.IndexOf(",") + 1).TakeWhile(char.IsDigit))),
                        int.Parse(string.Concat(x.Skip(x.IndexOf(":") + 2).TakeWhile(char.IsDigit))),
                        int.Parse(string.Concat(x.Skip(x.IndexOf("x") + 1).TakeWhile(char.IsDigit)))
                    )
                });

        private static HashSet<(int X, int Y)> GetOverlap(IEnumerable<Claim> claims)
        {
            var unique = new HashSet<(int X, int Y)>();
            var overlap = new HashSet<(int X, int Y)>();

            foreach (var pos in claims.SelectMany(x => x.Positions))
                if (!unique.Add(pos))
                    overlap.Add(pos);

            return overlap;
        }
    }
}
