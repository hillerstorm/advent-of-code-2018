using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day08 : Day
    {
        public override (Func<string>, Func<string>) GetParts(string path)
        {
            var input = path.ReadInput().SplitAsInt(" ");
            return (
                () => Part1(input).ToString(),
                () => Part2(input).ToString()
            );
        }


        public static int Part1(IEnumerable<int> input) =>
            SumNode(ParseNode(input.GetEnumerator()));

        public static int Part2(IEnumerable<int> input) =>
            GetValue(ParseNode(input.GetEnumerator()));

        private struct Node
        {
            public Node[] Children;
            public int[] Metadata;
        }

        private static Node ParseNode(IEnumerator<int> enumerator)
        {
            if (!enumerator.MoveNext())
                throw new IndexOutOfRangeException();
            var numChildren = enumerator.Current;
            if (!enumerator.MoveNext())
                throw new IndexOutOfRangeException();
            var numMetadata = enumerator.Current;
            return new Node
            {
                Children = 0.To(numChildren)
                    .Select(_ => ParseNode(enumerator))
                    .ToArray(),
                Metadata = 0.To(numMetadata)
                    .TakeWhile(_ => enumerator.MoveNext())
                    .Select(_ => enumerator.Current)
                    .ToArray()
            };
        }

        private static int SumNode(Node node) =>
            node.Metadata.Sum() + node.Children.Sum(SumNode);

        private static int GetValue(Node node) =>
            node.Metadata
                .Sum(x =>
                    node.Children.Length == 0
                        ? x
                        : x > 0 && x <= node.Children.Length
                            ? GetValue(node.Children[x - 1])
                            : 0
                );
    }
}
