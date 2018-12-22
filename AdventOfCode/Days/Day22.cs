using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day22 : Day
    {
        public override (Func<string>, Func<string>) GetParts(string path)
        {
            var input = path.ReadLines().ToArray();
            return (
                () => Part1(input).ToString(),
                () => Part2(input).ToString()
            );
        }

        public static int Part1(string[] input)
        {
            var (depth, target) = ParseInput(input);
            var geoIndices = new Dictionary<(int X, int Y), int>();
            var sum = 0;
            for (var y = 0; y <= target.Item2; y++)
            for (var x = 0; x <= target.Item1; x++)
            {
                var pos = (x, y);
                var geoIndex = GeoIndex(geoIndices, pos, target, depth);
                var erosion = Erosion(geoIndex, depth);
                var type = erosion % 3;
                sum += type;
            }

            return sum;
        }

        public static float Part2(string[] input)
        {
            var (depth, target) = ParseInput(input);
            var geoIndices = new Dictionary<(int X, int Y), int>();
            var startPos = (0, 0);
            var start = new Region(
                startPos,
                Erosion(GeoIndex(geoIndices, startPos, target, depth), depth) % 3,
                Tool.Torch
            );
            var goal = new Region(
                target,
                Erosion(GeoIndex(geoIndices, target, target, depth), depth) % 3,
                Tool.Torch
            );
            var path = FindPath(
                start,
                goal,
                // Neighbours
                region => new[]
                    {
                        (region.Pos.X - 1, region.Pos.Y),
                        (region.Pos.X + 1, region.Pos.Y),
                        (region.Pos.X, region.Pos.Y - 1),
                        (region.Pos.X, region.Pos.Y + 1),
                    }.Where(x => x.Item1 >= 0 && x.Item2 >= 0)
                    .Select(x =>
                    {
                        var pos = x;
                        var type = Erosion(GeoIndex(geoIndices, pos, target, depth), depth) % 3;
                        return (pos, type);
                    })
                    // Filter out regions which cannot be traversed using the equipped tool
                    .Where(x => Tools[x.type].Contains(region.Tool))
                    .Select(x => new Region(x.pos, x.type, region.Tool))
                    .Concat(
                        // Include the same region, but with the tool switched
                        Tools[region.Type]
                            .Where(x => x != region.Tool)
                            .Select(x => new Region(region.Pos, region.Type, x))
                    ),
                (reg1, reg2) => // Manhattan heuristics
                    Math.Abs(reg1.Pos.X - reg2.Pos.X) + Math.Abs(reg1.Pos.Y - reg2.Pos.Y),
                (reg1, reg2) => // Graph cost, 1 minute for same tool, 7 minutes to switch
                    reg1.Tool == reg2.Tool
                        ? 1
                        : 7
            );

            return path.Sum(x => x.Cost);
        }

        private static (int Depth, (int X, int Y) Target) ParseInput(IReadOnlyList<string> input)
        {
            var depth = int.Parse(input[0].Split(" ")[1]);
            var targetParts = input[1].Split(" ")[1].SplitAsInt(",").ToArray();
            return (depth, (targetParts[0], targetParts[1]));
        }

        private static int GeoIndex(Dictionary<(int X, int Y), int> geoIndices, (int x, int y) pos, (int, int) target, int depth)
        {
            if (geoIndices.ContainsKey(pos))
                return geoIndices[pos];
            var geoIndex = pos.x == 0 && pos.y == 0 || pos.x == target.Item1 && pos.y == target.Item2
                ? 0
                : pos.y == 0
                    ? pos.x * 16807
                    : pos.x == 0
                        ? pos.y * 48271
                        : SumAdjacentErosion(geoIndices, pos, target, depth);
            geoIndices.TryAdd(pos, geoIndex);
            return geoIndex;
        }

        private static int SumAdjacentErosion(Dictionary<(int X, int Y), int> geoIndices, (int x, int y) pos, (int, int) target, int depth)
        {
            var pos1 = (pos.x - 1, pos.y);
            var pos2 = (pos.x, pos.y - 1);

            var geoIdx1 = GeoIndex(geoIndices, pos1, target, depth);
            var geoIdx2 = GeoIndex(geoIndices, pos2, target, depth);

            var erosion1 = Erosion(geoIdx1, depth);
            var erosion2 = Erosion(geoIdx2, depth);

            return erosion1 * erosion2;
        }

        private static int Erosion(int geoIndex, int depth) =>
            (geoIndex + depth) % 20183;

        private enum Tool
        {
            Neither = 0,
            Torch = 1,
            ClimbingGear = 2,
        }

        private static readonly Dictionary<int, Tool[]> Tools = new Dictionary<int, Tool[]>
        {
            {0, new[] {Tool.ClimbingGear, Tool.Torch}},
            {1, new[] {Tool.ClimbingGear, Tool.Neither}},
            {2, new[] {Tool.Torch, Tool.Neither}}
        };

        private class Region : IComparable<Region>, IEquatable<Region>
        {
            public Region((int X, int Y) pos, int type, Tool tool)
            {
                Pos = pos;
                Type = type;
                Tool = tool;
            }

            public (int X, int Y) Pos { get; }
            public int Type { get; }
            public Tool Tool { get; }

            public int CompareTo(Region other)
            {
                if (ReferenceEquals(this, other)) return 0;
                if (ReferenceEquals(null, other)) return 1;
                var posComparison = ((IComparable) Pos).CompareTo(other.Pos);
                if (posComparison != 0) return posComparison;
                return Tool.CompareTo(other.Tool);
            }

            public bool Equals(Region other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return Pos.Equals(other.Pos) && Tool == other.Tool;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Region) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (Pos.GetHashCode() * 397) ^ (int) Tool;
                }
            }
        }

        // Custom version of path finder which caches the graph cost along with visited nodes
        private static IEnumerable<(int X, int Y, float Cost)> FindPath(
            Region start,
            Region goal,
            Func<Region, IEnumerable<Region>> neighbours,
            Func<Region, Region, float> heuristics,
            Func<Region, Region, float> graphCost)
        {
            var queue = new PriorityQueue<Region>();
            queue.Enqueue(start, 0f);
            var visited = new Dictionary<Region, (Region Region, float Cost)> {{start, default}};
            var costs = new Dictionary<Region, float> {{start, 0f}};
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                if (current.Equals(goal))
                    break;

                foreach (var next in neighbours(current))
                {
                    var gc = graphCost(current, next);
                    var newCost = costs[current] + gc;
                    if (costs.ContainsKey(next) && !(newCost < costs[next]))
                        continue;

                    costs[next] = newCost;
                    var priority = newCost + heuristics(goal, next);
                    queue.Enqueue(next, priority);
                    visited[next] = (current, gc);
                }
            }

            return !visited.ContainsKey(goal)
                ? new (int X, int Y, float Cost)[] { }
                : BuildPath(visited, start, goal);
        }

        private static (int X, int Y, float Cost)[] BuildPath(IReadOnlyDictionary<Region, (Region Region, float Cost)> cameFrom, Region start, Region current)
        {
            var path = new List<(int X, int Y, float Cost)>();
            while (!start.Equals(current))
            {
                var next = cameFrom[current];
                path.Add((current.Pos.X, current.Pos.Y, next.Cost));
                current = next.Region;
            }

            path.Add((start.Pos.X, start.Pos.Y, cameFrom[current].Cost));

            path.Reverse();

            return path.ToArray();
        }
    }
}
