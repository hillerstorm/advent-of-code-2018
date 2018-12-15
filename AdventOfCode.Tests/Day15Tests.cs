using AdventOfCode.Days;
using Xunit;

namespace AdventOfCode.Tests
{
    public class Day15Tests
    {
        [Fact]
        public void TestPart1()
        {
            var assertions = new[]
            {
                ("Inputs/Day15_1.txt", 36334),
                ("Inputs/Day15_2.txt", 39514),
                ("Inputs/Day15_3.txt", 27755),
                ("Inputs/Day15_4.txt", 28944),
                ("Inputs/Day15_5.txt", 18740),
                ("Inputs/Day15_6.txt", 27730),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day15.Part1(x.Input.ReadLines())));
        }

        [Fact]
        public void TestPart2()
        {
            var assertions = new[]
            {
                ("Inputs/Day15_2.txt", 31284),
                ("Inputs/Day15_3.txt", 3478),
                ("Inputs/Day15_4.txt", 6474),
                ("Inputs/Day15_5.txt", 1140),
                ("Inputs/Day15_6.txt", 4988),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day15.Part2(x.Input.ReadLines())));
        }
    }
}
