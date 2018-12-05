using AdventOfCode.Days;
using Xunit;

namespace AdventOfCode.Tests
{
    public class Day5Tests
    {
        [Fact]
        public void TestPart1()
        {
            var assertions = new[]
            {
                ("aA", 0),
                ("abBA", 0),
                ("abAB", 4),
                ("aabAAB", 6),
                ("dabAcCaCBAcCcaDA", 10)
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day05.Part1(x.Input)));
        }

        [Fact]
        public void TestPart2()
        {
            var assertions = new[]
            {
                ("dabAcCaCBAcCcaDA", 4)
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day05.Part2(x.Input)));
        }
    }
}
