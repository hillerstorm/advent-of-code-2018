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
                //new Day05(), // Really slow...
                new Day06(),
                new Day07(),
                new Day08(),
                new Day09(),
                new Day10(),
                new Day11(),
                new Day12(),
                new Day13(),
                new Day14(),
                //new Day15(), // Slow, for now
                new Day16(),
                new Day17(),
                new Day18(),
                new Day19(),
                //new Day20(), // Really slow...
                //new Day21(), // Part 2 slow :<
                new Day22(),
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
        }
    }
}
