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
    public class Day10 : Day
    {
        public override (Func<string>, Func<string>) GetParts(string path)
        {
            var input = path.ReadLines();
            return (
                () => $"{Environment.NewLine}{Part1(input)}{Environment.NewLine}",
                () => Part2(input).ToString()
            );
        }

        public static string Part1(IEnumerable<string> input, bool createImage = false)
        {
            var pixels = ParsePixels(input);
            var (_, minX, minY, width, height) = GetMinValues(pixels);
            var litPixels = pixels
                .Select(x => NormalizePosition(x.X, x.Y, minX, minY))
                .ToImmutableHashSet();

            if (createImage)
                CreateImage(width, height, litPixels);

            return string.Join(Environment.NewLine,
                Extensions.Square(0, 0, width, height)
                    .OrderBy(x => x.Y)
                    .ThenBy(x => x.X)
                    .Select(i => litPixels.Contains((i.X, i.Y)) ? "#" : " ")
                    .PartitionBy(width)
                    .Select(x => string.Concat(x))
            );
        }

        public static int Part2(IEnumerable<string> input) =>
            GetMinValues(ParsePixels(input)).Iterations;

        private static void CreateImage(int width, int height, IEnumerable<(int X, int Y)> pixels)
        {
            using (var img = new SKBitmap(width, height))
            {
                foreach (var (x, y) in pixels)
                    img.SetPixel(x, y, SKColors.White);
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

        private static (int X, int Y) NormalizePosition(int X, int Y, int minX, int minY)
        {
            var x = X;
            if (minX < 0)
                x += Math.Abs(minX);
            else if (minX > 0)
                x -= minX;

            var y = Y;
            if (minY < 0)
                y += Math.Abs(minY);
            else if (minY > 0)
                y -= minY;

            return (x, y);
        }

        private static (int Iterations, int MinX, int MinY, int Width, int Height) GetMinValues((int X, int Y, int HorizVelocity, int VertVelocity)[] pixels)
        {
            var iterations = 0;
            var minX = int.MaxValue;
            var minY = int.MaxValue;
            var width = int.MaxValue;
            var height = int.MaxValue;
            while (true)
            {
                for (var i = 0; i < pixels.Length; i++)
                {
                    pixels[i].X += pixels[i].HorizVelocity;
                    pixels[i].Y += pixels[i].VertVelocity;
                }
                var curMinX = pixels.Min(x => x.X);
                var maxX = pixels.Max(x => x.X);
                var curMinY = pixels.Min(x => x.Y);
                var maxY = pixels.Max(x => x.Y);
                var minWidth = Math.Abs(maxX - curMinX) + 1;
                var minHeight = Math.Abs(maxY - curMinY) + 1;
                if (minWidth < width && minHeight < height)
                {
                    minX = curMinX;
                    minY = curMinY;
                    width = minWidth;
                    height = minHeight;
                    iterations++;
                }
                else
                {
                    for (var i = 0; i < pixels.Length; i++)
                    {
                        pixels[i].X -= pixels[i].HorizVelocity;
                        pixels[i].Y -= pixels[i].VertVelocity;
                    }

                    break;
                }
            }

            return (iterations, minX, minY, width, height);
        }

        private static (int X, int Y, int HorizVelocity, int VertVelocity)[] ParsePixels(IEnumerable<string> input) =>
            input.Select(line =>
            {
                var parts = line.Split("> velocity=<");
                var pos = parts[0].Substring(10).Split(", ").Select(int.Parse).ToArray();
                var vel = parts[1].Substring(0, parts[1].Length - 1).Split(", ").Select(int.Parse).ToArray();
                return (pos[0], pos[1], vel[0], vel[1]);
            }).ToArray();
    }
}
