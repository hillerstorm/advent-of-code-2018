using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

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

        public static BigInteger Part1(string input)
        {
            var (players, maxMarbles) = ParseInput(input);
            return GetMaxScore(maxMarbles, players);
        }

        public static BigInteger Part2(string input)
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

        private static BigInteger GetMaxScore(int maxMarbles, int players)
        {
            maxMarbles = maxMarbles - maxMarbles % 23;
            var marbles = new Queue<int>(new[] {0});
            var scores = new Dictionary<int, BigInteger>();
            for (var i = 1; i <= maxMarbles; i++)
            {
                if (i % 23 == 0)
                {
                    var newIndex = marbles.Count - 8;
                    if (newIndex < 0)
                        newIndex = marbles.Count + newIndex;
                    for (var x = 0; x < newIndex; x++)
                        marbles.Enqueue(marbles.Dequeue());
                    var score = i + marbles.Dequeue();
                    marbles.Enqueue(marbles.Dequeue());
                    var player = i % players;
                    if (scores.ContainsKey(player))
                        scores[player] += score;
                    else
                        scores.Add(player, score);
                }
                else
                {
                    marbles.Enqueue(marbles.Dequeue());
                    marbles.Enqueue(i);
                }
            }

            return scores.Max(x => x.Value);
        }
    }
}
