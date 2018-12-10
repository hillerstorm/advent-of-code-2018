using System;
using System.Linq;
using Nito.Collections;

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

        public static long Part1(string input)
        {
            var (players, maxMarbles) = ParseInput(input);
            return GetMaxScore(maxMarbles, players);
        }

        public static long Part2(string input)
        {
            var (players, maxMarbles) = ParseInput(input);
            return GetMaxScore(maxMarbles * 100, players);
        }

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

        private static long GetMaxScore(int maxMarbles, int players)
        {
            maxMarbles = maxMarbles - maxMarbles % 23;
            var marbles = new Deque<int>(new[] {0});
            var scores = new long[players];
            for (var i = 1; i <= maxMarbles; i++)
            {
                if (i % 23 == 0)
                {
                    marbles.Rotate(-7);
                    var score = i + marbles.RemoveFromBack();
                    marbles.AddToBack(marbles.RemoveFromFront());
                    scores[i % players] += score;
                }
                else
                {
                    marbles.AddToBack(marbles.RemoveFromFront());
                    marbles.AddToBack(i);
                }
            }

            return scores.Max();
        }
    }
}
