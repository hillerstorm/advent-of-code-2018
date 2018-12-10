using AdventOfCode.Days;
using Xunit;

namespace AdventOfCode.Tests
{
    public class Day2Tests
    {
        [Fact]
        public void TestPart1()
        {
            var assertions = new[]
            {
                ("abcdef, bababc, abbcde, abcccd, aabcdd, abcdee, ababab", 12),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day02.Part1(x.Input.Split(", "))));
        }

        [Fact]
        public void TestPart2()
        {
            var assertions = new[]
            {
                ("abcde, fghij, klmno, pqrst, fguij, axcye, wvxyz", "fgij")
            };
            assertions.ForEach(((string Input, string Expected) x) =>
                Assert.Equal(x.Expected, Day02.Part2(x.Input.Split(", "))));
        }
    }
}
