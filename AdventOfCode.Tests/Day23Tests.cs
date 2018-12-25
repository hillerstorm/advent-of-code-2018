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
                ("Inputs/Day23.txt", 7),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day23.Part1(x.Input.ReadLines())));
        }
    }
}
