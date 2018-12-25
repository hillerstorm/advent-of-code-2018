using AdventOfCode.Days;
using Xunit;

namespace AdventOfCode.Tests
{
    public class Day04Tests
    {
        [Fact]
        public void TestPart1()
        {
            var assertions = new[]
            {
                ("Inputs/Day04.txt", 240),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day04.Part1(x.Input.ReadLines())));
        }

        [Fact]
        public void TestPart2()
        {
            var assertions = new[]
            {
                ("Inputs/Day04.txt", 4455),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day04.Part2(x.Input.ReadLines())));
        }
    }
}
