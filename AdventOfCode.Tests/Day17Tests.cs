using AdventOfCode.Days;
using Xunit;

namespace AdventOfCode.Tests
{
    public class Day17Tests
    {
        [Fact]
        public void TestPart1()
        {
            var assertions = new[]
            {
                ("Inputs/Day17.txt", 57),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day17.Part1(x.Input.ReadLines())));
        }

        [Fact]
        public void TestPart2()
        {
            var assertions = new[]
            {
                ("Inputs/Day17.txt", 29),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day17.Part2(x.Input.ReadLines())));
        }
    }
}
