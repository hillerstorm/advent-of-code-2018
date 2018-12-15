using System;
using System.Linq;
using Nito.Collections;

namespace AdventOfCode.Days
{
    public class Day14 : Day
    {
        public override (Func<string>, Func<string>) GetParts(string path)
        {
            var input = path.ReadInput();
            return (
                () => Part1(input),
                () => Part2(input).ToString()
            );
        }

        public static string Part1(string input)
        {
            var count = int.Parse(input);
            var scores = new Deque<int>(new[] {3, 7});
            var firstElf = 0;
            var secondElf = 1;
            for (var i = 0; i < count + 10; i++)
            {
                var firstScore = scores[firstElf];
                var secondScore = scores[secondElf];
                var sum = (firstScore + secondScore).ToString().Select(x => int.Parse(x.ToString())).ToArray();
                scores.AddToBack(sum[0]);
                if (sum.Length > 1)
                    scores.AddToBack(sum[1]);
                firstElf = (firstElf + firstScore + 1) % scores.Count;
                secondElf = (secondElf + secondScore + 1) % scores.Count;
            }

            return string.Concat(
                scores.ToArray()
                    .AsSpan(count, 10)
                    .ToArray()
                    .Select(x => x.ToString())
            );
        }

        public static int Part2(string input)
        {
            var span = new ReadOnlySpan<int>(input.Select(x => int.Parse(x.ToString())).ToArray());
            var scores = new Deque<int>(new[] {3, 7});
            var firstElf = 0;
            var secondElf = 1;
            var matchedStr = string.Empty;
            while (true)
            {
                var firstScore = scores[firstElf];
                var secondScore = scores[secondElf];
                var sum = (firstScore + secondScore).ToString().Select(x => int.Parse(x.ToString())).ToArray();
                scores.AddToBack(sum[0]);
                matchedStr += sum[0];
                if (!input.StartsWith(matchedStr))
                    matchedStr = matchedStr.Substring(1);
                else if (input == matchedStr)
                    return scores.Count - input.Length;
                if (sum.Length > 1)
                {
                    scores.AddToBack(sum[1]);
                    matchedStr += sum[1];
                    if (!input.StartsWith(matchedStr))
                        matchedStr = matchedStr.Substring(1);
                    else if (input == matchedStr)
                        return scores.Count - input.Length;
                }

                firstElf = (firstElf + firstScore + 1) % scores.Count;
                secondElf = (secondElf + secondScore + 1) % scores.Count;
            }
        }
    }
}
