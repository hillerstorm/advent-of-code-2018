using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day19 : Day
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
            var commands = program.Skip(1).Select(ParseCommand).ToList();
            var registers = new int[6];
            if (part2)
            {
                registers[0] = 1;
                var eq = commands.FindIndex(x => x.Op == "eqrr");
                if (eq > 0)
                {
                    var eqOp = commands[eq];
                    commands[eq] = ("magic", 0, 0, 0, reg =>
                    {
                        var max = reg[eqOp.B];
                        for (var i = 1; i <= max; i++)
                        {
                            for (var j = 1; j <= max; j++)
                            {
                                var tmp = i * j;
                                if (tmp == max)
                                    reg[0] += i;
                                else if (tmp > max)
                                    break;
                            }
                        }

                        reg[ip] = commands.Count * 2;
                    });
                }
            }

            for (var i = 0; i < commands.Count; i++)
            {
                registers[ip] = i;
                commands[i].Run(registers);
                i = registers[ip];
            }

            return registers[0];
        }

        private static (string Op, int A, int B, int C, Action<int[]> Run) ParseCommand(string line)
        {
            var parts = line.Split(" ");
            var instruction = Day16.Instructions[parts[0]];
            var a = int.Parse(parts[1]);
            var b = int.Parse(parts[2]);
            var c = int.Parse(parts[3]);
            return (parts[0], a, b, c, reg =>
                instruction(reg, a, b, c));
        }
    }
}
