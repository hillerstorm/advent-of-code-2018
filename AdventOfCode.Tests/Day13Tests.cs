using AdventOfCode.Days;
using Xunit;

namespace AdventOfCode.Tests
{
    public class Day13Tests
    {
        [Fact]
        public void TestPart1()
        {
            var assertions = new[]
            {
                ("Inputs/Day13_straight.txt", (0, 3)),
                ("Inputs/Day13_complex.txt", (7, 3)),
            };
            assertions.ForEach(((string Input, (int X, int Y) Expected) x) =>
                Assert.Equal(x.Expected, Day13.Part1(x.Input.ReadLines())));
        }

        [Fact]
        public void TestPart2()
        {
            var assertions = new[]
            {
                ("Inputs/Day13_part2.txt", (6, 4)),
            };
            assertions.ForEach(((string Input, (int X, int Y) Expected) x) =>
                Assert.Equal(x.Expected, Day13.Part2(x.Input.ReadLines())));
        }
    }
}
