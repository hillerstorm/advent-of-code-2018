using AdventOfCode.Days;
using Xunit;

namespace AdventOfCode.Tests
{
    public class Day19Tests
    {
        [Fact]
        public void TestPart1()
        {
            var assertions = new[]
            {
                ("Inputs/Day19.txt", 6),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day19.Part1(x.Input.ReadLines())));
        }
    }
}
