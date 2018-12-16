using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Days
{
    public class Day16 : Day
    {
        public override (Func<string>, Func<string>) GetParts(string path)
        {
            var input = path.ReadInput();
            return (
                () => Part1(input).ToString(),
                () => Part2(input).ToString()
            );
        }

        private static readonly Dictionary<string, Action<int[], int, int, int>> Instructions = new Dictionary<string, Action<int[], int, int, int>>
        {
            // (add register) stores into register C the result of adding register A and register B.
            {"addr", (reg, a, b, c) =>
                reg[c] = reg[a] + reg[b]},

            // (add immediate) stores into register C the result of adding register A and value B.
            {"addi", (reg, a, b, c) =>
                reg[c] = reg[a] + b},

            // (multiply register) stores into register C the result of multiplying register A and register B.
            {"mulr", (reg, a, b, c) =>
                reg[c] = reg[a] * reg[b]},

            // (multiply immediate) stores into register C the result of multiplying register A and value B.
            {"muli", (reg, a, b, c) =>
                reg[c] = reg[a] * b},

            // (bitwise AND register) stores into register C the result of the bitwise AND of register A and register B.
            {"banr", (reg, a, b, c) =>
                reg[c] = reg[a] & reg[b]},

            // (bitwise AND immediate) stores into register C the result of the bitwise AND of register A and value B.
            {"bani", (reg, a, b, c) =>
                reg[c] = reg[a] & b},

            // (bitwise OR register) stores into register C the result of the bitwise OR of register A and register B.
            {"borr", (reg, a, b, c) =>
                reg[c] = reg[a] | reg[b]},

            // (bitwise OR immediate) stores into register C the result of the bitwise OR of register A and value B.
            {"bori", (reg, a, b, c) =>
                reg[c] = reg[a] | b},

            // (set register) copies the contents of register A into register C. (Input B is ignored.)
            {"setr", (reg, a, b, c) =>
                reg[c] = reg[a]},

            // (set immediate) stores value A into register C. (Input B is ignored.)
            {"seti", (reg, a, b, c) =>
                reg[c] = a},

            // (greater-than immediate/register) sets register C to 1 if value A is greater than register B. Otherwise, register C is set to 0.
            {"gtir", (reg, a, b, c) =>
                reg[c] = a > reg[b] ? 1 : 0},

            // (greater-than register/immediate) sets register C to 1 if register A is greater than value B. Otherwise, register C is set to 0.
            {"gtri", (reg, a, b, c) =>
                reg[c] = reg[a] > b ? 1 : 0},

            // (greater-than register/register) sets register C to 1 if register A is greater than register B. Otherwise, register C is set to 0.
            {"gtrr", (reg, a, b, c) =>
                reg[c] = reg[a] > reg[b] ? 1 : 0},

            // (equal immediate/register) sets register C to 1 if value A is equal to register B. Otherwise, register C is set to 0.
            {"eqir", (reg, a, b, c) =>
                reg[c] = a == reg[b] ? 1 : 0},

            // (equal register/immediate) sets register C to 1 if register A is equal to value B. Otherwise, register C is set to 0.
            {"eqri", (reg, a, b, c) =>
                reg[c] = reg[a] == b ? 1 : 0},

            // (equal register/register) sets register C to 1 if register A is equal to register B. Otherwise, register C is set to 0.
            {"eqrr", (reg, a, b, c) =>
                reg[c] = reg[a] == reg[b] ? 1 : 0},
        };

        private static readonly Regex TestPattern = new Regex(@"Before:\s*\[([\d, ]+)\]\s*([\d ]+)\s*After:\s*\[([\d, ]+)\]",
            RegexOptions.Multiline);

        public static int Part1(string input)
        {
            var matches = TestPattern.Matches(input);
            var count = 0;
            foreach (Match match in matches)
            {
                var before = match.Groups[1].Value.SplitAsInt().ToArray();
                var op = match.Groups[2].Value.SplitAsInt(" ").ToArray();
                var after = match.Groups[3].Value.SplitAsInt().ToArray();
                var matchingOps = 0;
                foreach (var instruction in Instructions)
                {
                    var reg = new[] {before[0], before[1], before[2], before[3]};
                    instruction.Value(reg, op[1], op[2], op[3]);
                    if (reg[0] == after[0] &&
                        reg[1] == after[1] &&
                        reg[2] == after[2] &&
                        reg[3] == after[3] &&
                        ++matchingOps == 3)
                    {
                        count++;
                        break;
                    }
                }
            }

            return count;
        }

        public static int Part2(string input)
        {
            var matches = TestPattern.Matches(input);
            var possibleOpCodes = new Dictionary<string, HashSet<int>>();
            foreach (Match match in matches)
            {
                var before = match.Groups[1].Value.SplitAsInt().ToArray();
                var op = match.Groups[2].Value.SplitAsInt(" ").ToArray();
                var after = match.Groups[3].Value.SplitAsInt().ToArray();
                foreach (var instruction in Instructions)
                {
                    var reg = new[] {before[0], before[1], before[2], before[3]};
                    instruction.Value(reg, op[1], op[2], op[3]);

                    if (reg[0] != after[0] || reg[1] != after[1] || reg[2] != after[2] || reg[3] != after[3])
                        continue;

                    if (possibleOpCodes.ContainsKey(instruction.Key))
                        possibleOpCodes[instruction.Key].Add(op[0]);
                    else
                        possibleOpCodes.Add(instruction.Key, new HashSet<int>(op[0]));
                }
            }

            var allOpCodes = new Dictionary<int, string>();
            while (true)
            {
                if (allOpCodes.Count == 16)
                    break;

                var singles = possibleOpCodes.Where(x => x.Value.Count == 1).ToArray();
                if (singles.Length == 0)
                    break;

                var opsToRemoveFrom = possibleOpCodes.Where(x =>
                    x.Value.Count > 1 && x.Value.Overlaps(singles.SelectMany(y => y.Value)));

                foreach (var ops in opsToRemoveFrom)
                    ops.Value.RemoveWhere(x => singles.Any(y => y.Value.Contains(x)));

                foreach (var op in singles)
                {
                    possibleOpCodes.Remove(op.Key);
                    allOpCodes.Add(op.Value.First(), op.Key);
                }
            }

            var program = input.Split("\n\n\n\n")[1]
                .Split("\n")
                .Select(x => x.SplitAsInt(" ").ToArray());
            var register = new[] {0, 0, 0, 0};
            foreach (var op in program)
                Instructions[allOpCodes[op[0]]](register, op[1], op[2], op[3]);

            return register[0];
        }
    }
}
