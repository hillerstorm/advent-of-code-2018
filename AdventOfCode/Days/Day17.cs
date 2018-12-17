using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace AdventOfCode.Days
{
    public class Day17 : Day
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
            var walls = ParseWalls(input);
            var water = new HashSet<(int X, int Y)>();
            Traverse(water, walls, walls.Max(x => x.X), walls.Max(x => x.Y), (500, 0));
            var minY = walls.Min(x => x.Y);
            return water.Count(x => x.Y >= minY);
        }

        public static int Part2(IEnumerable<string> input)
        {
            var walls = ParseWalls(input);
            var water = new HashSet<(int X, int Y)>();
            Traverse(water, walls, walls.Max(x => x.X), walls.Max(x => x.Y), (500, 0));
            var removed = 0;
            do
            {
                removed = water.RemoveWhere(x =>
                    !water.Contains((x.X - 1, x.Y)) && !walls.Contains((x.X - 1, x.Y)) ||
                    !water.Contains((x.X + 1, x.Y)) && !walls.Contains((x.X + 1, x.Y)) ||
                    !water.Contains((x.X, x.Y + 1)) && !walls.Contains((x.X, x.Y + 1)));
            } while (removed > 0);
            return water.Count;
        }

        private static HashSet<(int X, int Y)> ParseWalls(IEnumerable<string> input) =>
            input.SelectMany(x =>
                {
                    var parts = x.Split(", ");
                    var first = parts[0].Split('=');
                    var firstValue = int.Parse(first[1]);
                    var second = parts[1].Split('=');
                    var range = second[1].SplitAsInt("..").ToArray();
                    return first[0] == "x"
                        ? range[0].To(range[1] - range[0] + 1).Select(y => (firstValue, y))
                        : range[0].To(range[1] - range[0] + 1).Select(y => (y, firstValue));
                })
                .ToHashSet();

        private static bool Traverse(ISet<(int X, int Y)> water, ICollection<(int X, int Y)> walls, int maxX, int maxY, (int X, int Y) current)
        {
            var fromAbove = water.Contains((current.X, current.Y - 1));
            var down = (current.X, current.Y + 1);
            var downVisited = water.Contains(down);
            if (downVisited && fromAbove)
            {
                var surroundedByWalls = false;
                for (var x = current.X - 1; x >= 0; x--)
                    if (water.Contains((x, current.Y)))
                        break;
                    else if (walls.Contains((x, current.Y)))
                    {
                        surroundedByWalls = true;
                        break;
                    }

                if (surroundedByWalls)
                    for (var x = current.X + 1; x <= maxX; x++)
                        if (water.Contains((x, current.Y)))
                        {
                            surroundedByWalls = false;
                            break;
                        }
                        else if (walls.Contains((x, current.Y)))
                            break;

                if (!surroundedByWalls)
                    return true;
            }

            var reachedEnd = false;
            if (!downVisited && !walls.Contains(down))
            {
                water.Add(down);
                reachedEnd = down.Item2 == maxY || Traverse(water, walls, maxX, maxY, down);
            }

            if (reachedEnd)
                return true;

            var left = (current.X - 1, current.Y);
            if (!walls.Contains(left) && !water.Contains(left))
            {
                water.Add(left);
                reachedEnd = Traverse(water, walls, maxX, maxY, left);
            }

            var right = (current.X + 1, current.Y);
            if (!walls.Contains(right) && !water.Contains(right))
            {
                water.Add(right);
                var rightEnded = Traverse(water, walls, maxX, maxY, right);
                reachedEnd = reachedEnd || rightEnded;
            }

            return reachedEnd;
        }

        private static void PrintGrid(HashSet<(int X, int Y)> water, HashSet<(int X, int Y)> walls)
        {
            var minX = water.Min(x => x.X) - 1;
            var maxX = water.Max(x => x.X) + 1;
            var maxY = walls.Max(x => x.Y);
            var width = maxX - minX + 2;
            var grid = new byte[width * (maxY + 1)];
            foreach (var cell in water)
                grid[width * cell.Y + (cell.X - minX)] = 126;
            foreach (var cell in walls.Where(w => w.X >= minX && w.X <= maxX))
                grid[width * cell.Y + (cell.X - minX)] = 35;
            for (var i = width - 1; i < grid.Length; i += width)
                grid[i] = 10;
            for (var i = 0; i < grid.Length; i++)
                grid[i] = grid[i] == default ? (byte)32 : grid[i];
            var tempFile = Path.Combine(Path.GetTempPath(), Path.ChangeExtension(Path.GetRandomFileName(), ".txt"));
            File.WriteAllBytes(tempFile, grid);

            Process.Start(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "explorer" : "open", tempFile);
        }
    }
}
