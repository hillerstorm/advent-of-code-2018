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
                new Day01()
            };
            var sw = new Stopwatch();
            foreach (var day in days)
            {
                sw.Start();
                var (part1, part2) = day.Run();
                sw.Stop();
                Console.WriteLine($"{day.ToString().Split(".").LastOrDefault()}: {sw.Elapsed:g}");
                Console.WriteLine($"  Part 1: {part1}");
                Console.WriteLine($"  Part 2: {part2}");
                sw.Reset();
            }

            Console.Read();
        }
    }
}
