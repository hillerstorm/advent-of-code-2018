using AdventOfCode.Days;
using Xunit;

namespace AdventOfCode.Tests
{
    public class Day25Tests
    {
        [Fact]
        public void TestPart1()
        {
            var assertions = new[]
            {
                ("Inputs/Day25_1.txt", 2),
                ("Inputs/Day25_2.txt", 4),
                ("Inputs/Day25_3.txt", 3),
                ("Inputs/Day25_4.txt", 8),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day25.Part1(x.Input.ReadLines())));
        }
    }
}
