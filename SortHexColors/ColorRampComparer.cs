using System;
using System.Collections.Generic;
using System.Drawing;

namespace SortHexColors
{
    class ColorRampComparer : IComparer<Tuple<string, string, Color>>
    {
        public int Compare(Tuple<string, string, Color> a, Tuple<string, string, Color> b)
        {
            var c1 = Step(a.Item3);
            var c2 = Step(b.Item3);

            return ((IComparable)c1).CompareTo(c2);
        }

        private Tuple<int, int, int> Step(Color color)
        {
            int lum = (int)Math.Sqrt(.241 * color.R + .691 * color.G + .068 * color.B);

            float hue = 1 - Rotate(color.GetHue(), 90) / 360;
            float lightness = color.GetBrightness();

            int h2 = (int)(hue * _repetitions);
            int v2 = (int)(lightness * _repetitions);

            // Invert function
            if (_invert)
            {
                if ((h2 % 2) == 0)
                    v2 = _repetitions - v2;
                else
                    lum = _repetitions - lum;
            }

            return Tuple.Create(h2, lum, v2);
        }

        private float Rotate(float angle, float degrees)
        {
            angle = (angle + degrees) % 360;
            if (angle < 0) angle += 360;
            return angle;
        }

        // Declare Invert Varible
        private bool _invert = false;
        public bool Invert
        {
            get
            {
                return _invert;
            }
            set
            {
                _invert = value;
            }
        }

        // Declare Repetitions Varible
        private int _repetitions = 8;
        public int Repetitions
        {
            get
            {
                return _repetitions;
            }
            set
            {
                _repetitions = value;
            }
        }
    }
}
