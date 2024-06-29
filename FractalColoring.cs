using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Fractals
{
    public static class FractalColoring
    {
        public static Color GetColor(int n, int maxIterations, int palette)
        {
            if (n == maxIterations)
                return Color.Black;
            if (palette == 0)
            {
                if (n == maxIterations)
                    return Color.Black;
                if (n % 2 == 0)
                    return Color.Black;
                else
                    return Color.White;
            }
            if (palette == 1)
            {
                float coef = (float)n / (float)maxIterations;
                if (coef > 0.5f)
                    return Color.FromArgb((int)(coef * 255f), (int)(coef * 255f), (int)(coef * 255f));
                else
                    return Color.FromArgb((int)(coef * 128f), (int)(coef * 128f), (int)(coef * 128f));
            }
            if (palette == 2)
            {
                float coef = (float)n / (float)maxIterations;
                if (coef > 0.5f)
                    return Color.FromArgb(255, (int)(coef * 255f), (int)(coef * 255f));
                else
                    return Color.FromArgb((int)(coef * 255f), 0, 0);
            }
            if (palette == 3)
            {
                float coef = (float)n / (float)maxIterations;
                if (coef > 0.5f)
                    return Color.FromArgb((int)(coef * 255f), 255, (int)(coef * 255f));
                else
                    return Color.FromArgb(0, (int)(coef * 255f), 0);
            }
            if (palette == 4)
            {
                float coef = (float)n / (float)maxIterations;
                if (coef > 0.5f)
                    return Color.FromArgb((int)(coef * 255f), (int)(coef * 255f), 255);
                else
                    return Color.FromArgb(0, 0, (int)(coef * 255f));
            }
            if (palette == 5)
            {
                float fr = 1f / 16f;
                if ((float)n / (float)maxIterations < 1 * fr)
                    return Color.FromArgb(66, 30, 15);
                else if ((float)n / (float)maxIterations < 2 * fr)
                    return Color.FromArgb(25, 7, 26);
                else if ((float)n / (float)maxIterations < 3 * fr)
                    return Color.FromArgb(9, 1, 47);
                else if ((float)n / (float)maxIterations < 4 * fr)
                    return Color.FromArgb(4, 4, 73);
                else if ((float)n / (float)maxIterations < 5 * fr)
                    return Color.FromArgb(0, 7, 100);
                else if ((float)n / (float)maxIterations < 6 * fr)
                    return Color.FromArgb(12, 44, 138);
                else if ((float)n / (float)maxIterations < 7 * fr)
                    return Color.FromArgb(24, 82, 177);
                else if ((float)n / (float)maxIterations < 8 * fr)
                    return Color.FromArgb(57, 125, 209);
                else if ((float)n / (float)maxIterations < 9 * fr)
                    return Color.FromArgb(134, 181, 229);
                else if ((float)n / (float)maxIterations < 10 * fr)
                    return Color.FromArgb(211, 236, 248);
                else if ((float)n / (float)maxIterations < 11 * fr)
                    return Color.FromArgb(241, 233, 191);
                else if ((float)n / (float)maxIterations < 12 * fr)
                    return Color.FromArgb(248, 201, 95);
                else if ((float)n / (float)maxIterations < 13 * fr)
                    return Color.FromArgb(255, 170, 0);
                else if ((float)n / (float)maxIterations < 14 * fr)
                    return Color.FromArgb(204, 128, 0);
                else if ((float)n / (float)maxIterations < 15 * fr)
                    return Color.FromArgb(153, 87, 0);
                else
                    return Color.FromArgb(106, 52, 3);
            }
            if (palette == 6)
            {
                float coef = (float)n / (float)maxIterations;
                if (coef > 0.5f)
                    return Color.FromArgb(255, 223, (int)(coef * 255f));
                else
                    return Color.FromArgb((int)(coef * 255f), (int)(coef * 223f), 0);
            }
            if (palette == 7)
            {
                float coef = (float)n / (float)maxIterations;
                if (coef > 0.66f)
                    return Color.FromArgb((int)(coef * 255f), 0, 255 - (int)(coef * 255f));
                else if (coef > 0.33)
                    return Color.FromArgb(0, 255 - (int)(coef * 1.5f * 255f), (int)(coef * 1.5f * 255f));
                else
                    return Color.FromArgb(255 - (int)(coef * 3f * 255f), (int)(coef * 3f * 255f), 0);
            }
            return Color.Red;
        }
    }
}
