using System;
using AdventOfCode.Days;
using Xunit;

namespace AdventOfCode.Tests
{
    public class Day10Tests
    {
        [Fact]
        public void TestPart1()
        {
            var assertions = new[]
            {
                ("Inputs/Day10.txt", "Inputs/Day10_expected.txt"),
            };
            assertions.ForEach(((string Input, string ExpectedOutput) x) =>
            {
                var expected = string.Join(Environment.NewLine, x.ExpectedOutput.ReadLines());
                var result = Day10.Part1(x.Input.ReadLines());
                Assert.Equal(expected, result);
            });
        }

        [Fact]
        public void TestPart2()
        {
            var assertions = new[]
            {
                ("Inputs/Day10.txt", 3),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day10.Part2(x.Input.ReadLines())));
        }
    }
}
