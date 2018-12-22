using AdventOfCode.Days;
using Xunit;

namespace AdventOfCode.Tests
{
    public class Day22Tests
    {
        [Fact]
        public void TestPart1()
        {
            var assertions = new[]
            {
                (new[] {"depth: 510", "target: 10,10"}, 114),
            };
            assertions.ForEach(((string[] Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day22.Part1(x.Input)));
        }

        [Fact]
        public void TestPart2()
        {
            var assertions = new[]
            {
                (new[] {"depth: 510", "target: 10,10"}, 45f),
            };
            assertions.ForEach(((string[] Input, float Expected) x) =>
                Assert.Equal(x.Expected, Day22.Part2(x.Input)));
        }
    }
}
