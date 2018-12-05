using System;

namespace AdventOfCode
{
    public abstract class Day
    {
        public abstract (Func<string>, Func<string>) GetParts(string path);
    }
}
