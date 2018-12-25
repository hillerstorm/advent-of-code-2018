using AdventOfCode.Days;
using Xunit;

namespace AdventOfCode.Tests
{
    public class Day06Tests
    {
        [Fact]
        public void TestPart1()
        {
            var assertions = new[]
            {
                ("1, 1; 1, 6; 8, 3; 3, 4; 5, 5; 8, 9", 17),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day06.Part1(x.Input.Split("; "))));
        }

        [Fact]
        public void TestPart2()
        {
            var assertions = new[]
            {
                ("1, 1; 1, 6; 8, 3; 3, 4; 5, 5; 8, 9", 16),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day06.Part2(x.Input.Split("; "), maxDistance: 32)));
        }
    }
}
