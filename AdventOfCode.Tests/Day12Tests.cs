using AdventOfCode.Days;
using Xunit;

namespace AdventOfCode.Tests
{
    public class Day12Tests
    {
        [Fact]
        public void TestPart1()
        {
            var assertions = new[]
            {
                ("Inputs/Day12_example.txt", 325),
                ("Inputs/Day12_mine.txt", 3890),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day12.Part1(x.Input.ReadLines())));
        }

        [Fact]
        public void TestPart2()
        {
            var assertions = new[]
            {
                ("Inputs/Day12_example.txt", 50000000501),
                ("Inputs/Day12_mine.txt", 4800000001087),
            };
            assertions.ForEach(((string Input, long Expected) x) =>
                Assert.Equal(x.Expected, Day12.Part2(x.Input.ReadLines())));
        }
    }
}
