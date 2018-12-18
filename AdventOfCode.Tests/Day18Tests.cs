using AdventOfCode.Days;
using Xunit;

namespace AdventOfCode.Tests
{
    public class Day18Tests
    {
        [Fact]
        public void TestPart1()
        {
            var assertions = new[]
            {
                ("Inputs/Day18.txt", 1147),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day18.Part1(x.Input.ReadLines())));
        }
    }
}
