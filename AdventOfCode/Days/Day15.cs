using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RoyT.AStar;

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
            var gridArr = arr.SelectMany(x => x.ToCharArray()).ToArray();
            var height = arr.Length;
            var width = arr[0].Length;
            var walls = gridArr
                .Select((x, i) => (x, i))
                .Where(x => x.x == '#')
                .Select(x => x.i).ToArray();
            var unitArr = gridArr
                .Select((x, i) => (x, i))
                .Where(x => x.x == 'G' || x.x == 'E')
                .ToArray();
            var elfAttackPower = failOnElfDeath ? 4 : 3;
            while (true)
            {
                var grid = new Grid(width, height);
                foreach (var idx in walls)
                    grid.BlockCell(new Position(idx % width, idx / height));
                var units = unitArr
                    .Select(x =>
                        x.x == 'G'
                            ? (Unit) new Goblin(x.i, width, height)
                            : (Unit) new Elf(x.i, width, height, elfAttackPower))
                    .ToArray();
                var (success, outcome) = TryGetOutcome(grid, units, walls, width, height, failOnElfDeath);
                if (failOnElfDeath && !success)
                    elfAttackPower++;
                else
                    return outcome;
            }
        }

        private static (bool Success, int Outcome) TryGetOutcome(Grid grid, Unit[] units, int[] walls, int width, int height, bool failOnElfDeath)
        {
            var rounds = 0;
            while (true)
            {
                foreach (var unit in units.OrderBy(x => x.Index).Where(x => x.Alive))
                {
                    var enemies = unit is Goblin
                        ? units.OfType<Elf>().Where(x => x.Alive).ToArray<Unit>()
                        : units.OfType<Goblin>().Where(x => x.Alive).ToArray<Unit>();

                    if (enemies.Length == 0)
                        return (true, rounds * units.Sum(x => x.Alive ? x.HitPoints : 0));

                    var enemy = unit.GetClosestEnemy(enemies);
                    if (enemy != null)
                    {
                        unit.Attack(enemy);
                        if (!enemy.Alive && failOnElfDeath && enemy is Elf)
                            return (false, -1);

                        continue;
                    }

                    var toBlock = unit is Goblin
                        ? units.OfType<Goblin>()
                            .Where(x => x.Alive)
                            .Except(new[] {unit})
                            .ToArray()
                        : units.OfType<Elf>()
                            .Where(x => x.Alive)
                            .Except(new[] {unit})
                            .ToArray();

                    foreach (var ally in toBlock)
                        grid.BlockCell(new Position(ally.X, ally.Y));

                    var path = new[]
                        {
                            unit.Index - 1,
                            unit.Index + 1,
                            unit.Index - width,
                            unit.Index + width
                        }
                        .Where(z => walls.All(w => w != z) && units.Where(u => u.Alive).All(u => u.Index != z))
                        .SelectMany(x => enemies
                            .SelectMany(y => new[]
                                {
                                    y.Index - 1,
                                    y.Index + 1,
                                    y.Index - width,
                                    y.Index + width
                                }
                                .Where(z => walls.All(w => w != z) && units.Where(u => u.Alive).All(u => u.Index != z))
                                .Select(z => grid.GetPath(
                                    new Position(x % width, x / height),
                                    new Position(z % width, z / height),
                                    MovementPatterns.LateralOnly
                                ))
                            ))
                        .Where(x => x.Length > 0)
                        .OrderBy(x => x.Length)
                        .ThenBy(x => x[x.Length - 1].Y)
                        .ThenBy(x => x[x.Length - 1].X)
                        .ThenBy(y => y[0].Y)
                        .ThenBy(y => y[0].X)
                        .FirstOrDefault();

                    if (path != null)
                    {
                        unit.X = path[0].X;
                        unit.Y = path[0].Y;

                        enemy = unit.GetClosestEnemy(enemies);
                        if (enemy != null)
                        {
                            unit.Attack(enemy);
                            if (!enemy.Alive && failOnElfDeath && enemy is Elf)
                                return (false, -1);
                        }
                    }

                    foreach (var ally in toBlock)
                        grid.UnblockCell(new Position(ally.X, ally.Y));
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
            private int _index;

            protected Unit(char type, int index, int width, int height)
            {
                _width = width;
                _height = height;
                Type = type;
                Index = index;
                HitPoints = 200;
            }

            public bool Alive => HitPoints > 0;
            public int HitPoints { get; private set; }

            public int Index
            {
                get => _index;
                private set
                {
                    if (value < 0 || value >= _width * _height)
                        throw new IndexOutOfRangeException();

                    _index = value;
                }
            }

            public int X
            {
                get => Index % _width;
                set
                {
                    if (value < 0 || value >= _width)
                        throw new IndexOutOfRangeException();

                    Index = Index - X + value;
                }
            }

            public int Y
            {
                get => Index / _height;
                set
                {
                    if (value < 0 || value >= _height)
                        throw new IndexOutOfRangeException();

                    Index = value * _height + X;
                }
            }

            public char Type { get; }

            protected abstract int AttackPower { get; }

            public void Attack(Unit enemy)
            {
                enemy.HitPoints -= AttackPower;
            }

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

            protected override int AttackPower => 3;
        }

        private class Elf : Unit
        {
            public Elf(int index, int width, int height, int attackPower)
                : base('E', index, width, height)
            {
                AttackPower = attackPower;
            }

            protected override int AttackPower { get; }
        }
    }
}
