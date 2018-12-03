using System;
using AdventOfCode.Days;
using Xunit;

namespace AdventOfCode.Tests
{
    public class Day3Tests
    {
        [Fact]
        public void TestPart1()
        {
            var assertions = new[]
            {
                ("#1 @ 1,3: 4x4; #2 @ 3,1: 4x4; #3 @ 5,5: 2x2", 4),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected,
                    Day03.Part1(Day03.ParseClaims(x.Input.Split("; ")))));
        }

        [Fact]
        public void TestPart2()
        {
            var assertions = new[]
            {
                ("#1 @ 1,3: 4x4; #2 @ 3,1: 4x4; #3 @ 5,5: 2x2", "#3"),
            };
            assertions.ForEach(((string Input, string Expected) x) =>
                Assert.Equal(x.Expected,
                    Day03.Part2(Day03.ParseClaims(x.Input.Split("; ")))));
        }
    }
}
