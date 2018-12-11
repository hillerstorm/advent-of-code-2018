using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using SkiaSharp;

namespace AdventOfCode.Days
{
    public class Day11 : Day
    {
        public override (Func<string>, Func<string>) GetParts(string path)
        {
            var input = int.Parse(path.ReadInput());
            return (
                () => Part1(input).ToString(),
                () => Part2(input).ToString()
            );
        }

        public static (int X, int Y) Part1(int serialNumber, bool createImage = false)
        {
            var powerLevels = GetPowerLevels(serialNumber);

            if (createImage)
                CreateImage(powerLevels);

            var level = int.MinValue;
            var pos = (0, 0);
            for (var y = 1; y < 299; y++)
            {
                for (var x = 1; x < 299; x++)
                {
                    var sum = Extensions.Square(-1, -1, 3, 3)
                        .Select(i => powerLevels[300 * (y + i.Y) + x + i.X])
                        .Sum();

                    if (sum <= level)
                        continue;

                    level = sum;
                    pos = (x, y);
                }
            }

            return pos;
        }

        public static (int X, int Y, int Size) Part2(int serialNumber, bool createImage = false)
        {
            var levels = GetPowerLevels(serialNumber).AsSpan();
            var pos = (0, 0);
            var level = int.MinValue;
            var size = 0;
            for (var xy = 0; xy < levels.Length; xy++)
            {
                var x = xy % 300;
                var y = xy / 300;
                var maxSize = Math.Min(300 - x, 300 - y);
                var sum = levels[xy];
                for (var i = 1; i < maxSize; i++)
                {
                    sum += levels
                        .Slice(xy + i * 300, i + 1)
                        .ToArray()
                        .Sum();
                    for (var j = 0; j < i; j++)
                        sum += levels[xy + i + j * 300];

                    if (sum <= level)
                        continue;

                    pos = (x, y);
                    level = sum;
                    size = i + 1;
                }
            }

            if (createImage)
                CreateImage(levels.ToArray(), Extensions.Square(pos.Item1, pos.Item2, size, size).ToImmutableHashSet());

            return (pos.Item1 + 1, pos.Item2 + 1, size);
        }

        private static int[] GetPowerLevels(int serialNumber) =>
            0.To(300 * 300)
                .Select(i => GetPowerLevel(serialNumber, i % 300 + 1, i / 300 + 1))
                .ToArray();

        private static void CreateImage(IEnumerable<int> levels, IImmutableSet<(int, int)> part2 = null)
        {
            using (var img = new SKBitmap(300, 300))
            {
                img.Pixels = levels
                    .Select((level, i) => ToColor(level, part2?.Contains((i % 300 + 1, i / 300 + 1)) == true))
                    .ToArray();
                using (var newImg = SKImage.FromBitmap(img))
                {
                    var data = newImg.Encode(SKEncodedImageFormat.Png, 100);
                    var tempFile = Path.Combine(Path.GetTempPath(), Path.ChangeExtension(Path.GetRandomFileName(), ".png"));
                    using (var file = File.OpenWrite(tempFile))
                        data.SaveTo(file);
                    Process.Start(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "explorer" : "open", tempFile);
                }
            }
        }

        private static SKColor ToColor(int level, bool part2)
        {
            var rb = part2 ? (byte)0x00 : (byte)0xff;
            switch (level)
            {
                case 4:
                    return SKColors.Black;
                case 3:
                    return new SKColor(rb, 0xff, rb, 0x1c);
                case 2:
                    return new SKColor(rb, 0xff, rb, 0x38);
                case 1:
                    return new SKColor(rb, 0xff, rb, 0x54);
                case 0:
                    return new SKColor(rb, 0xff, rb, 0x70);
                case -1:
                    return new SKColor(rb, 0xff, rb, 0x8c);
                case -2:
                    return new SKColor(rb, 0xff, rb, 0xa8);
                case -3:
                    return new SKColor(rb, 0xff, rb, 0xc4);
                case -4:
                    return new SKColor(rb, 0xff, rb, 0xe0);
                case -5:
                    return new SKColor(rb, 0xff, rb, 0xff);
                default:
                    return SKColors.Green;
            }
        }

        private static int GetPowerLevel(int serialNumber, int x, int y)
        {
            var rackId = x + 10;
            return (rackId * y + serialNumber) * rackId / 100 % 10 - 5;
        }
    }
}
