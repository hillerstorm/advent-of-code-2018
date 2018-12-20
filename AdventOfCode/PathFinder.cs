using System;
using System.Collections.Generic;
using Priority_Queue;

namespace AdventOfCode
{
    public static class PathFinder
    {
        public static T[] FindPath<T>(
            T start,
            T goal,
            Func<T, IEnumerable<T>> neighbours)
            where T : IEquatable<T> =>
            FindPath(start, goal, neighbours, (_, __) => 0f);

        public static T[] FindPath<T>(
            T start,
            T goal,
            Func<T, IEnumerable<T>> neighbours,
            Func<T, T, float> heuristics)
            where T : IEquatable<T> =>
            FindPath(start, goal, neighbours, heuristics, (_, __) => 1f);

        public static T[] FindPath<T>(
            T start,
            T goal,
            Func<T, IEnumerable<T>> neighbours,
            Func<T, T, float> heuristics,
            Func<T, T, float> graphCost)
            where T : IEquatable<T>
        {
            var queue = new FastPriorityQueue<PathNode<T>>(short.MaxValue);
            queue.Enqueue(new PathNode<T>(start), 0f);
            var visited = new Dictionary<T, T> {{start, default}};
            var costs = new Dictionary<T, float> {{start, 0f}};
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                if (current.Value.Equals(goal))
                    break;

                foreach (var next in neighbours(current.Value))
                {
                    var newCost = costs[current.Value] + graphCost(current.Value, next);
                    if (costs.ContainsKey(next) && !(newCost < costs[next]))
                        continue;

                    costs[next] = newCost;
                    var priority = newCost + heuristics(goal, next);
                    queue.Enqueue(new PathNode<T>(next), priority);
                    visited[next] = current.Value;
                }
            }

            return !visited.ContainsKey(goal)
                ? new T[] { }
                : BuildPath(visited, start, goal);
        }

        private class PathNode<T> : FastPriorityQueueNode
        {
            public PathNode(T value)
            {
                Value = value;
            }

            public T Value { get; }
        }

        private static T[] BuildPath<T>(IReadOnlyDictionary<T, T> cameFrom, T start, T current)
            where T : IEquatable<T>
        {
            var path = new List<T>();
            while (!current.Equals(start))
            {
                path.Add(current);
                current = cameFrom[current];
            }

            path.Add(start);

            path.Reverse();

            return path.ToArray();
        }
    }
}
