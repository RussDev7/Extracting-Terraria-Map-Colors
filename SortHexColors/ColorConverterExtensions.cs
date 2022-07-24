using System;
using System.Drawing;

namespace SortHexColors
{
    public static class ColorConverterExtensions
    {
        // Hex To String
        // https://stackoverflow.com/a/37821008/8667430

        // #RRGGBB
        public static string ToHexString(this Color c) => $"#{c.R:X2}{c.G:X2}{c.B:X2}";

        // RGB(R, G, B)
        public static string ToRgbString(this Color c) => $"RGB({c.R}, {c.G}, {c.B})";

        // #RRGGBBAA
        public static string ToHexaString(this Color c) => $"#{c.R:X2}{c.G:X2}{c.B:X2}{c.A:X2}";

        public static double ToProportion(byte b) => b / (double)Byte.MaxValue;

        // RGBA(R, G, B, A)
        public static string ToRgbaString(this Color c) => $"RGBA({c.R}, {c.G}, {c.B}, {ToProportion(c.A):N2})";
    }
}
