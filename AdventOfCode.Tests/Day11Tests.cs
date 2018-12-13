using AdventOfCode.Days;
using Xunit;

namespace AdventOfCode.Tests
{
    public class Day11Tests
    {
        [Fact]
        public void TestPart1()
        {
            var assertions = new[]
            {
                (18, (33, 45)),
                (42, (21, 61)),
            };
            assertions.ForEach(((int SerialNumber, (int X, int Y) Expected) x) =>
                Assert.Equal(x.Expected, Day11.Part1(x.SerialNumber)));
        }

        [Fact(Skip = "Slow")]
        public void TestPart2()
        {
            var assertions = new[]
            {
                (18, (90, 269, 16)),
                (42, (232, 251, 12)),
            };
            assertions.ForEach(((int SerialNumber, (int X, int Y, int Size) Expected) x) =>
                Assert.Equal(x.Expected, Day11.Part2(x.SerialNumber)));
        }
    }
}
