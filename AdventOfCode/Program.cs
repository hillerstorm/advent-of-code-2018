using AdventOfCode.Days;
using System;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            var days = new Day[]
            {
                new Day01(),
                new Day02(),
                new Day03(),
                new Day04(),
                new Day05(),
                new Day06(),
                new Day07(),
                new Day08(),
            };
            var sw = new Stopwatch();
            var part1Sw = new Stopwatch();
            var part2Sw = new Stopwatch();
            foreach (var day in days)
            {
                var name = day.ToString().Split(".").LastOrDefault();
                var inputPath = $"Inputs/{name}.txt";
                Console.WriteLine(name);
                sw.Start();
                var (part1Func, part2Func) = day.GetParts(inputPath);
                part1Sw.Start();
                var part1 = part1Func();
                part1Sw.Stop();
                Console.WriteLine($"  Part 1: {part1} ({part1Sw.Elapsed:g})");
                part2Sw.Start();
                var part2 = part2Func();
                part2Sw.Stop();
                sw.Stop();
                Console.WriteLine($"  Part 2: {part2} ({part2Sw.Elapsed:g})");
                Console.WriteLine($"---- total: {sw.Elapsed:g}");
                sw.Reset();
                part1Sw.Reset();
                part2Sw.Reset();
            }

            Console.Read();
        }
    }
}
