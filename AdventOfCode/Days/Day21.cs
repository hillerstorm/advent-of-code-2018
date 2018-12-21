using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day21 : Day
    {
        public override (Func<string>, Func<string>) GetParts(string path)
        {
            var input = path.ReadLines();
            return (
                () => Part1(input).ToString(),
                () => Part2(input).ToString()
            );
        }

        public static int Part1(IEnumerable<string> input) =>
            Run(input);

        public static int Part2(IEnumerable<string> input) =>
            Run(input, part2: true);

        private static int Run(IEnumerable<string> input, bool part2 = false)
        {
            var program = input.ToArray();
            var ip = int.Parse(program[0].Substring(4));
            var commands = program.Skip(1).Select(Day19.ParseCommand).ToList();
            var registers = new int[6];
            var seenValues = new HashSet<int>();
            var lastValue = 0;
            for (var i = 0; i < commands.Count; i++)
            {
                registers[ip] = i;
                var cmd = commands[i];
                if (cmd.Op == "eqrr" && (cmd.A == 0 || cmd.B == 0))
                {
                    var valueToSet = cmd.A == 0 ? registers[cmd.B] : registers[cmd.A];

                    if (part2)
                    {
                        // Find first duplicate, that's our answer
                        if (!seenValues.Add(valueToSet))
                            return lastValue;

                        lastValue = valueToSet;
                    }
                    else // Set the first result, this is the quickest answer!
                        registers[0] = valueToSet;
                }
                cmd.Run(registers);
                i = registers[ip];
            }

            return registers[0];
        }
    }
}
