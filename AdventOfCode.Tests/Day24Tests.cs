using AdventOfCode.Days;
using Xunit;

namespace AdventOfCode.Tests
{
    public class Day24Tests
    {
        [Fact]
        public void TestPart1()
        {
            var assertions = new[]
            {
                ("Inputs/Day24.txt", 5216),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day24.Part1(x.Input.ReadInput())));
        }

        [Fact]
        public void TestPart2()
        {
            var assertions = new[]
            {
                ("Inputs/Day24.txt", 51),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day24.Part2(x.Input.ReadInput())));
        }
    }
}
