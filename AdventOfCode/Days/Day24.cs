using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Days
{
    public class Day24 : Day
    {
        public override (Func<string>, Func<string>) GetParts(string path)
        {
            var input = path.ReadInput();
            return (
                () => Part1(input).ToString(),
                () => Part2(input).ToString()
            );
        }

        public static int Part1(string input) =>
            RunSimulation(ParseInput(input)).Units;

        public static int Part2(string input)
        {
            var groups = ParseInput(input);
            var toMin = 0;
            var toAdd = 1000;
            var toMax = 2000;
            while (true)
            {
                foreach (var group in groups)
                {
                    group.Reset();
                    if (group.Type == GroupType.Immune)
                        group.Damage += toAdd;
                }
                var (immuneSystemWon, units) = RunSimulation(groups);
                if (immuneSystemWon && (toAdd == toMax || toAdd == toMin))
                    return units;

                if (immuneSystemWon)
                {
                    toMax = toAdd;
                    toAdd = toMin + (toAdd - toMin) / 2;
                }
                else
                {
                    toMin = toAdd + 1;
                    toAdd = toMin + (toMax - toMin) / 2;
                }
            }
        }

        private static (bool ImmuneSystemWon, int Units) RunSimulation(IReadOnlyCollection<Group> groups)
        {
            while (true)
            {
                foreach (var group in groups
                    .OrderByDescending(x => x.Power)
                    .ThenByDescending(x => x.Initiative)
                    .Where(x => x.Alive))
                {
                    if (!group.Alive)
                        continue;

                    if (group.Enemies.All(x => !x.Alive))
                        return (
                            group.Type == GroupType.Immune,
                            groups
                                .Where(x => x.Type == group.Type && x.Alive)
                                .Sum(x => x.Units)
                        );

                    group.NextToAttack =
                        group.Enemies
                            .Where(x => x.Alive && !groups.Any(g => g.Alive && g.NextToAttack == x) &&
                                        !x.Immunities.Contains(group.AttackType))
                            .OrderByDescending(x => x.Weaknesses.Contains(group.AttackType)
                                ? group.Power * 2
                                : group.Power)
                            .ThenByDescending(x => x.Power)
                            .ThenByDescending(x => x.Initiative)
                            .FirstOrDefault();
                }

                var attempted = 0;
                var skipped = 0;

                foreach (var group in groups
                    .OrderByDescending(x => x.Initiative)
                    .Where(x => x.Alive && x.NextToAttack != null && x.NextToAttack.Alive))
                {
                    if (!group.Alive || !group.NextToAttack.Alive)
                        continue;

                    attempted++;
                    var dmgToDo = group.NextToAttack.Weaknesses.Contains(group.AttackType)
                        ? group.Power * 2
                        : group.Power;
                    if (dmgToDo < group.NextToAttack.Health)
                    {
                        skipped++;
                        continue;
                    }
                    group.NextToAttack.Units -= dmgToDo / group.NextToAttack.Health;
                }

                if (skipped == attempted)
                    return (false, -1);

                foreach (var group in groups)
                    group.NextToAttack = null;
            }
        }

        enum GroupType
        {
            Immune,
            Infection
        }

        private class Group
        {
            private readonly int _initialUnits;
            private readonly int _initialDamage;

            public Group(GroupType type, int units, int health, int damage, int initiative, string attackType, List<string> immunities, List<string> weaknesses)
            {
                Type = type;
                _initialUnits = units;
                Units = units;
                Health = health;
                _initialDamage = damage;
                Damage = damage;
                Initiative = initiative;
                AttackType = attackType;
                Immunities = immunities;
                Weaknesses = weaknesses;
            }

            public GroupType Type { get; }
            public int Units { get; set; }
            public int Health { get; }
            public int Damage { get; set; }
            public int Initiative { get; }
            public string AttackType { get; }
            public List<string> Immunities { get; }
            public List<string> Weaknesses { get; }

            public int Power => Units * Damage;
            public bool Alive => Units > 0;

            public List<Group> Enemies { get; set; }
            public Group NextToAttack { get; set; }

            public void Reset()
            {
                Units = _initialUnits;
                Damage = _initialDamage;
                NextToAttack = null;
            }
        }

        private static readonly Regex pattern = new Regex(
                @"^(?<units>\d+) units each with (?<hp>\d+) hit points[ ]{0,1}\({0,1}(?:(?<level1>weak|immune) to (?<types1>[\w,\s]+)){0,1};{0,1}[ ]{0,1}(?:(?<level2>weak|immune) to (?<types2>[\w,\s]+)){0,1}\){0,1} with an attack that does (?<damage>\d+) (?<attackType>\w+) damage at initiative (?<initiative>\d+)",
                RegexOptions.Multiline);
        private static List<Group> ParseInput(string input)
        {
            var result = input
                .Split("\n\nInfection:\n");
            var immuneGroup = result[0].Substring(15);
            var infectionGroup = result[1];

            var immuneMatches = pattern.Matches(immuneGroup);
            var infectionMatches = pattern.Matches(infectionGroup);

            var immunes = immuneMatches
                .Select(MapGroup(GroupType.Immune))
                .ToList();

            var infections = infectionMatches
                .Select(MapGroup(GroupType.Infection))
                .ToList();
            infections.ForEach(x => x.Enemies = immunes);
            immunes.ForEach(x => x.Enemies = infections);

            return immunes.Concat(infections).ToList();
        }

        private static Func<Match, Group> MapGroup(GroupType type) => match =>
        {
            var units = int.Parse(match.Groups["units"].Value);
            var hp = int.Parse(match.Groups["hp"].Value);
            var damage = int.Parse(match.Groups["damage"].Value);
            var initiative = int.Parse(match.Groups["initiative"].Value);
            var attackType = match.Groups["attackType"].Value;
            var immunities = new List<string>();
            var weaknesses = new List<string>();
            var level1 = match.Groups["level1"].Value;
            var types1 = match.Groups["types1"].Value;
            if (!string.IsNullOrEmpty(level1) && !string.IsNullOrEmpty(types1))
            {
                switch (level1)
                {
                    case "weak":
                        weaknesses.AddRange(types1.Split(", "));
                        break;
                    case "immune":
                        immunities.AddRange(types1.Split(", "));
                        break;
                }
            }

            var level2 = match.Groups["level2"].Value;
            var types2 = match.Groups["types2"].Value;
            if (!string.IsNullOrEmpty(level2) && !string.IsNullOrEmpty(types2))
            {
                switch (level2)
                {
                    case "weak":
                        weaknesses.AddRange(types2.Split(", "));
                        break;
                    case "immune":
                        immunities.AddRange(types2.Split(", "));
                        break;
                }
            }

            return new Group(type, units, hp, damage, initiative, attackType, immunities, weaknesses);
        };
    }
}
