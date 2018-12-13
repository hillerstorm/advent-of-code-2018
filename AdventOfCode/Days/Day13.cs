using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day13 : Day
    {
        public override (Func<string>, Func<string>) GetParts(string path)
        {
            var input = path.ReadLines();
            return (
                () => Part1(input).ToString(),
                () => Part2(input).ToString()
            );
        }

        public static (int X, int Y) Part1(IEnumerable<string> input) =>
            FindCoordinate(input, false);

        public static (int X, int Y) Part2(IEnumerable<string> input) =>
            FindCoordinate(input, true);

        private enum Dir
        {
            Left = 0,
            // ReSharper disable once UnusedMember.Local
            Straight = 1,
            Right = 2,
        }

        private static (int X, int Y) FindCoordinate(IEnumerable<string> input, bool findLast)
        {
            var inputArr = input.ToArray();
            var arr = inputArr.SelectMany(x => x).ToArray();
            var width = inputArr[0].Length;
            var deltas = new Dictionary<char, int>
            {
                {'>',  1    },
                {'<', -1    },
                {'v',  width},
                {'^', -width},
            };

            HashSet<(int Index, char Arrow, Dir Dir)> carts = arr
                .Select((x, i) => (i, x, Dir.Left))
                .Where(x => x.x == '>' || x.x == '<' || x.x == 'v' || x.x == '^')
                .ToList()
                .ToHashSet();

            while (true)
            {
                foreach (var cart in carts.OrderBy(x => x.Index).ToArray())
                {
                    if (!carts.Contains(cart))
                        continue;

                    carts.Remove(cart);

                    var nextIndex = cart.Index + deltas[cart.Arrow];
                    var nextCart = carts.FirstOrDefault(x => x.Index == nextIndex);

                    if (nextCart != default)
                    {
                        if (findLast)
                            carts.Remove(nextCart);
                        else
                            return (nextIndex % width, nextIndex / width);
                    }
                    else
                    {
                        var next = arr[nextIndex];
                        var nextDirection = next == '+'
                            ? (Dir) (((int) cart.Dir + 1) % 3)
                            : cart.Dir;
                        switch (cart.Arrow)
                        {
                            case '>' when next == '+' && cart.Dir == Dir.Left:
                            case '>' when next == '/':
                            case '<' when next == '+' && cart.Dir == Dir.Right:
                            case '<' when next == '\\':
                                carts.Add((nextIndex, '^', nextDirection));
                                break;
                            case '>' when next == '+' && cart.Dir == Dir.Right:
                            case '>' when next == '\\':
                            case '<' when next == '+' && cart.Dir == Dir.Left:
                            case '<' when next == '/':
                                carts.Add((nextIndex, 'v', nextDirection));
                                break;
                            case '^' when next == '+' && cart.Dir == Dir.Right:
                            case '^' when next == '/':
                            case 'v' when next == '+' && cart.Dir == Dir.Left:
                            case 'v' when next == '\\':
                                carts.Add((nextIndex, '>', nextDirection));
                                break;
                            case '^' when next == '+' && cart.Dir == Dir.Left:
                            case '^' when next == '\\':
                            case 'v' when next == '+' && cart.Dir == Dir.Right:
                            case 'v' when next == '/':
                                carts.Add((nextIndex, '<', nextDirection));
                                break;
                            default:
                                carts.Add((nextIndex, cart.Arrow, nextDirection));
                                break;
                        }
                    }
                }

                if (!findLast || carts.Count != 1)
                    continue;

                var index = carts.First().Index;
                return (index % width, index / width);
            }
        }
    }
}
