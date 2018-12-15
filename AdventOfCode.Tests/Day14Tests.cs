using AdventOfCode.Days;
using Xunit;

namespace AdventOfCode.Tests
{
    public class Day14Tests
    {
        [Fact]
        public void TestPart1()
        {
            var assertions = new[]
            {
                ("9", "5158916779"),
                ("5", "0124515891"),
                ("18", "9251071085"),
                ("2018", "5941429882"),
            };
            assertions.ForEach(((string Input, string Expected) x) =>
                Assert.Equal(x.Expected, Day14.Part1(x.Input)));
        }

        [Fact]
        public void TestPart2()
        {
            var assertions = new[]
            {
                ("51589", 9),
                ("01245", 5),
                ("92510", 18),
                ("59414", 2018),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day14.Part2(x.Input)));
        }
    }
}
