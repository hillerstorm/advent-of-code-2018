using AdventOfCode.Days;
using Xunit;

namespace AdventOfCode.Tests
{
    public class Day16Tests
    {
        [Fact]
        public void TestPart1()
        {
            var assertions = new[]
            {
                ("Before: [3, 2, 1, 1] 9 2 1 2 After:  [3, 2, 2, 1]", 1),
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(x.Expected, Day16.Part1(x.Input)));
        }
    }
}
