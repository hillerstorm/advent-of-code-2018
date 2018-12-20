using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day20 : Day
    {
        public override (Func<string>, Func<string>) GetParts(string path)
        {
            var input = path.ReadInput();
            return (
                () => Part1(input).ToString(),
                () => Part2(input).ToString()
            );
        }

        public static int Part1(string input) =>
            GetPaths(input)
                .Where(x => x.Length > 0)
                .OrderBy(x => x.Length)
                .LastOrDefault()?.Length - 1 ?? -1;

        public static int Part2(string input) =>
            GetPaths(input)
                .Count(x => x.Length > 1000);

        private static IEnumerable<Node[]> GetPaths(string input)
        {
            var root = new Node('x', 0, 0);
            var allNodes = new HashSet<Node>(new[] {root});
            var currentNodes = new Stack<IStep>(new[] {root});
            for (var i = 1; i < input.Length; i++)
            {
                var currentStep = currentNodes.Peek();
                var dir = input[i];
                switch (dir)
                {
                    case 'W':
                    {
                        currentNodes.Push(currentStep.Add(dir, -1, 0, allNodes));
                        break;
                    }
                    case 'E':
                    {
                        currentNodes.Push(currentStep.Add(dir, 1, 0, allNodes));
                        break;
                    }
                    case 'N':
                    {
                        currentNodes.Push(currentStep.Add(dir, 0, -1, allNodes));
                        break;
                    }
                    case 'S':
                    {
                        currentNodes.Push(currentStep.Add(dir, 0, 1, allNodes));
                        break;
                    }
                    case '|' when currentStep is Node:
                    {
                        while (currentNodes.Peek() is Node)
                            currentNodes.Pop();
                        break;
                    }
                    case '(' when currentStep is Node node:
                    {
                        currentNodes.Push(new Group(node));
                        break;
                    }
                    case ')' when currentStep is Node:
                    {
                        while (currentNodes.Peek() is Node)
                            currentNodes.Pop();
                        currentNodes.Pop();
                        break;
                    }
                    case ')' when currentStep is Group:
                        currentNodes.Pop();
                        break;
                }
            }

            return allNodes
                .Select(goal =>
                    PathFinder.FindPath(
                        root,
                        goal,
                        node => node.Next,
                        Node.Distance
                    )
                );
        }

        private interface IStep
        {
            Node Add(char dir, int dx, int dy, HashSet<Node> allNodes);
        }

        private class Group : IStep
        {
            private readonly Node _baseNode;

            public Group(Node baseNode)
            {
                _baseNode = baseNode;
            }

            public Node Add(char dir, int dx, int dy, HashSet<Node> allNodes) =>
                _baseNode.Add(dir, dx, dy, allNodes);

            public override string ToString() =>
                $"({_baseNode})";
        }

        private class Node : IEquatable<Node>, IStep
        {
            private readonly char _dir;
            private readonly int _x;
            private readonly int _y;

            public Node(char dir, int x, int y)
            {
                _dir = dir;
                _x = x;
                _y = y;
                Next = new List<Node>();
            }

            public List<Node> Next { get; }

            public override string ToString() =>
                $"{_dir} ({_x}, {_y})";

            public Node Add(char dir, int dx, int dy, HashSet<Node> allNodes)
            {
                var nextNode = new Node(dir, _x + dx, _y + dy);
                if (allNodes.Add(nextNode))
                    Next.Add(nextNode);
                return nextNode;
            }

            public static float Distance(Node n1, Node n2) =>
                Math.Abs(n1._x - n2._x) + Math.Abs(n1._y - n2._y);

            public bool Equals(Node other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return _x == other._x && _y == other._y;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((Node) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (_x * 397) ^ _y;
                }
            }
        }
    }
}
