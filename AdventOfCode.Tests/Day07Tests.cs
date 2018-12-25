using AdventOfCode.Days;
using Xunit;

namespace AdventOfCode.Tests
{
    public class Day07Tests
    {
        [Fact]
        public void TestPart1()
        {
            var assertions = new[]
            {
                ("Inputs/Day07.txt", "CABDFE"),
            };
            assertions.ForEach(((string Input, string Expected) x) =>
                Assert.Equal(x.Expected, Day07.Part1(x.Input.ReadLines())));
        }

        [Fact]
        public void TestPart2()
        {
            var assertions = new[]
            {
                ("Inputs/Day07.txt", 15),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day07.Part2(x.Input.ReadLines(), 2, -64)));
        }
    }
}
