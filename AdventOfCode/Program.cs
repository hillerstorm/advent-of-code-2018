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
            };
            var sw = new Stopwatch();
            foreach (var day in days)
            {
                var name = day.ToString().Split(".").LastOrDefault();
                var inputPath = $"Inputs/{name}.txt";
                sw.Start();
                var (part1, part2) = day.Run(inputPath);
                sw.Stop();
                Console.WriteLine($"{name}: {sw.Elapsed:g}");
                Console.WriteLine($"  Part 1: {part1}");
                Console.WriteLine($"  Part 2: {part2}");
                sw.Reset();
            }

            Console.Read();
        }
    }
}
