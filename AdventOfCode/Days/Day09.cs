using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day09 : Day
    {
        public override (Func<string>, Func<string>) GetParts(string path)
        {
            var input = path.ReadInput();
            return (
                () => Part1(input).ToString(),
                () => Part2(input).ToString()
            );
        }

        public static int Part1(string input)
        {
            var (players, maxMarbles) = ParseInput(input);
            return GetMaxScore(maxMarbles, players);
        }

        public static int Part2(string input) =>
            -1;

        private static (int, int) ParseInput(string input) => (
            int.Parse(
                string.Concat(
                    input.TakeWhile(char.IsDigit)
                )
            ),
            int.Parse(
                string.Concat(
                    input.SkipWhile(char.IsDigit)
                        .SkipWhile(c => !char.IsDigit(c))
                        .TakeWhile(char.IsDigit)
                )
            )
        );

        private static int GetMaxScore(int maxMarbles, int players)
        {
            var marbles = new List<int>(new[] {0});
            var scores = new Dictionary<int, int>();
            var currentMarble = 0;
            for (var i = 1; i <= maxMarbles; i++)
            {
                int newIndex;
                if (i % 23 == 0)
                {
                    newIndex = currentMarble - 7;
                    if (newIndex < 0)
                        newIndex = marbles.Count + newIndex;
                    var valueAtIndex = marbles[newIndex];
                    var score = i + valueAtIndex;
                    marbles.RemoveAt(newIndex);
                    var player = i % players;
                    if (scores.ContainsKey(player))
                        scores[player] += score;
                    else
                        scores.Add(player, score);
                }
                else
                {
                    newIndex = marbles.Count == 1 ? 1 : currentMarble + 2;
                    if (newIndex > marbles.Count)
                        newIndex = newIndex % marbles.Count;
                    marbles.Insert(newIndex, i);
                }

                currentMarble = newIndex;
            }

            return scores.Max(x => x.Value);
        }
    }
}
