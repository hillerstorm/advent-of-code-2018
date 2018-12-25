using AdventOfCode.Days;
using Xunit;

namespace AdventOfCode.Tests
{
    public class Day08Tests
    {
        [Fact]
        public void TestPart1()
        {
            var assertions = new[]
            {
                ("2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2", 138),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day08.Part1(x.Input.SplitAsInt(" "))));
        }

        [Fact]
        public void TestPart2()
        {
            var assertions = new[]
            {
                ("2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2", 66),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day08.Part2(x.Input.SplitAsInt(" "))));
        }
    }
}
