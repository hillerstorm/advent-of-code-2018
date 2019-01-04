using AdventOfCode.Days;
using Xunit;

namespace AdventOfCode.Tests
{
    public class Day23Tests
    {
        [Fact]
        public void TestPart1()
        {
            var assertions = new[]
            {
                ("Inputs/Day23_1.txt", 7),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day23.Part1(x.Input.ReadLines())));
        }

        [Fact]
        public void TestPart2()
        {
            var assertions = new[]
            {
                ("Inputs/Day23_2.txt", "36"),
            };
            assertions.ForEach(((string Input, string Expected) x) =>
                Assert.Equal(x.Expected, Day23.Part2(x.Input.ReadLines())));
        }
    }
}
