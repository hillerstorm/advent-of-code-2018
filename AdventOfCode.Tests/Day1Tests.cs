using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Days;
using Xunit;

namespace AdventOfCode.Tests
{
    public class Day1Tests
    {
        [Fact]
        public void TestPart1()
        {
            var assertions = new[]
            {
                ("+1, -2, +3, +1", 3),
                ("+1, +1, +1", 3),
                ("+1, +1, -2", 0),
                ("-1, -2, -3", -6)
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(Day01.Part1(x.Input.SplitAsInt()), x.Expected));
        }
        
        [Fact]
        public void TestPart2()
        {
            var assertions = new[]
            {
                ("+1, -2, +3, +1", 2),
                ("+1, -1", 0),
                ("+3, +3, +4, -2, -4", 10),
                ("-6, +3, +8, +5, -6", 5),
                ("+7, +7, -2, -7, -4", 14)
            };
            assertions.ForEach(((string Input, int Expected) x) =>
                Assert.Equal(Day01.Part2(x.Input.SplitAsInt()), x.Expected));
        }
    }
}