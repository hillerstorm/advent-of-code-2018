using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day15 : Day
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
            GetOutcome(input, failOnElfDeath: false);

        public static int Part2(IEnumerable<string> input) =>
            GetOutcome(input, failOnElfDeath: true);

        private static int GetOutcome(IEnumerable<string> input, bool failOnElfDeath)
        {
            var arr = input.ToArray();
            var gridArr = arr.SelectMany(x => x.ToCharArray()).Select((y, idx) => (y, idx)).Where(y => y.y != '.').ToArray();
            var height = arr.Length;
            var width = arr[0].Length;
            var manhattan = Manhattan(width, height);
            var walls = gridArr
                .Where(x => x.Item1 == '#')
                .Select(x => x.idx)
                .ToHashSet();
            walls = walls
                .Where(x => x % width > 0 && !walls.Contains(x - 1) ||
                            x % width < width - 1 && !walls.Contains(x + 1) ||
                            x / height > 0 && !walls.Contains(x - width) ||
                            x / height < height - 1 && !walls.Contains(x + width))
                .ToHashSet();
            var elfAttackPower = failOnElfDeath ? 4 : 3;
            while (true)
            {
                var units = gridArr
                    .Where(x => x.Item1 == 'G' || x.Item1 == 'E')
                    .Select(x =>
                        x.Item1 == 'G'
                            ? (Unit) new Goblin(x.idx, width, height)
                            : (Unit) new Elf(x.idx, width, height, elfAttackPower))
                    .ToArray();
                var (success, outcome) = TryGetOutcome(units, walls, width, manhattan, failOnElfDeath);
                if (failOnElfDeath && !success)
                    elfAttackPower++;
                else
                    return outcome;
            }
        }

        private static Func<int, IEnumerable<int>> GetNeighbours(int width, ICollection<int> blockers) => x =>
            new[]
            {
                x - 1,
                x + 1,
                x - width,
                x + width
            }.Where(y => !blockers.Contains(y));

        private static Func<int, int, float> Manhattan(int width, int height) => (x, y) =>
            Math.Abs(y % width - x % width) + Math.Abs(y / height - x / height);

        private static (bool Success, int Outcome) TryGetOutcome(
            Unit[] units,
            HashSet<int> walls,
            int width,
            Func<int, int, float> manhattan,
            bool failOnElfDeath)
        {
            var goblins = units.OfType<Goblin>().ToArray<Unit>();
            var elves = units.OfType<Elf>().ToArray<Unit>();
            var rounds = 0;
            while (true)
            {
                foreach (var unit in units.OrderBy(x => x.Index).Where(x => x.Alive))
                {
                    var enemies = unit is Goblin
                        ? elves.Where(x => x.Alive).ToArray()
                        : goblins.Where(x => x.Alive).ToArray();

                    if (enemies.Length == 0)
                        return (true, rounds * units.Sum(x => x.Alive ? x.HitPoints : 0));

                    var enemy = unit.GetClosestEnemy(enemies);
                    if (enemy != null)
                    {
                        enemy.HitPoints -= unit.AttackPower;
                        if (failOnElfDeath && enemy is Elf && !enemy.Alive)
                            return (false, -1);

                        continue;
                    }

                    var neighbours = GetNeighbours(width, walls.Union(units.Where(x => x.Alive).Select(x => x.Index)).ToHashSet());
                    var path = neighbours(unit.Index)
                        .Pairs(enemies.SelectMany(x => neighbours(x.Index)))
                        .Select(x =>
                            x.A == x.B
                                ? new[] {x.A}
                                : PathFinder.FindPath(x.A, x.B, neighbours, manhattan))
                        .Where(x => x.Length > 0)
                        .OrderBy(x => x.Length)
                        .ThenBy(x => x[x.Length - 1])
                        .ThenBy(x => x[0])
                        .FirstOrDefault();

                    if (path == null)
                        continue;

                    unit.Index = path[0];

                    enemy = unit.GetClosestEnemy(enemies);
                    if (enemy == null)
                        continue;

                    enemy.HitPoints -= unit.AttackPower;
                    if (failOnElfDeath && enemy is Elf && !enemy.Alive)
                        return (false, -1);
                }

                rounds++;
            }
        }

        private static void PrintGrid(int width, int height, IReadOnlyCollection<Unit> units, int[] walls)
        {
            Debug.WriteLine(string.Empty);
            var size = width * height;
            for (var i = 0; i < size; i++)
            {
                Debug.Write(
                    walls.Any(w => w == i)
                        ? '#'
                        : units.SingleOrDefault(u => u.Index == i)?.Type ?? '.'
                );
                if ((i + 1) % width != 0)
                    continue;
                var y = i / height;
                Debug.Write($"  {string.Join(", ", units.Where(x => x.Y == y).OrderBy(x => x.X).Select(x => x.ToString()))}{Environment.NewLine}");
            }
        }

        private abstract class Unit
        {
            private readonly int _width;
            private readonly int _height;

            protected Unit(char type, int index, int width, int height)
            {
                _width = width;
                _height = height;
                Type = type;
                Index = index;
                HitPoints = 200;
            }

            public bool Alive => HitPoints > 0;
            public int HitPoints { get; set; }
            public int X => Index % _width;
            public int Y => Index / _height;
            public char Type { get; }
            public int Index { get; set; }
            public abstract int AttackPower { get; }

            private bool IsAdjacent(Unit enemy) =>
                enemy.Index == Index - 1 ||
                enemy.Index == Index + 1 ||
                enemy.Index == Index - _width ||
                enemy.Index == Index + _width;

            public Unit GetClosestEnemy(IEnumerable<Unit> enemies) =>
                enemies
                    .Where(IsAdjacent)
                    .OrderBy(x => x.HitPoints)
                    .ThenBy(x => x.Index)
                    .FirstOrDefault();

            public override string ToString() =>
                $"({X}, {Y}) {Type}({HitPoints:000})";
        }

        private class Goblin : Unit
        {
            public Goblin(int index, int width, int height)
                : base('G', index, width, height)
            {
            }

            public override int AttackPower => 3;
        }

        private sealed class Elf : Unit
        {
            public Elf(int index, int width, int height, int attackPower)
                : base('E', index, width, height)
            {
                AttackPower = attackPower;
            }

            public override int AttackPower { get; }
        }
    }
}
