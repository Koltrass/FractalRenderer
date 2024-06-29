using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using System.Windows.Forms;
using System.Numerics;
using System.Drawing;

namespace Fractals.FractalPlotters
{
    public class MultibrotPlotter : IFractalPlotter
    {
        public void DrawFractal(Graphics graphics, ParamsForFractal parameters)
        {
            try
            {
                int height = (int)graphics.VisibleClipBounds.Height;
                int width = (int)graphics.VisibleClipBounds.Width;
                SolidBrush drawingBrush = new SolidBrush(Color.Yellow);
                for (int pixY = 0; pixY < height; pixY++)
                {
                    for (int pixX = 0; pixX < width; pixX++)
                    {
                        (decimal cx, decimal cy) = CoordinateShenanigans.GetWorldCoords(pixX, pixY, parameters.screenCenter,
                            parameters.offset, parameters.scaleFactor);
                        decimal x = 0;
                        decimal y = 0;
                        decimal x2 = x * x;
                        decimal y2 = y * y;
                        int n = 0;
                        while (x2 + y2 <= parameters.escapeNumber && n < parameters.maxIterations)
                        {
                            (x, y) = ApplyFunction(parameters.appliedFunc, x, y, parameters.powerOfZ, x2, y2, cx, cy);
                            x2 = x * x;
                            y2 = y * y;
                            n++;
                        }
                        if (n == parameters.maxIterations)
                        {
                            drawingBrush.Color = Color.Black;
                        }
                        else
                            drawingBrush.Color = FractalColoring.GetColor(n, parameters.maxIterations, parameters.palette);
                        graphics.FillRectangle(drawingBrush, pixX, pixY, 1, 1);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Calculations failed (due to overflow or precision error)", "Critical error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void DrawFractalMultithread(Graphics graphics, ParamsForFractal parameters)
        {
            try
            {


                int height = (int)graphics.VisibleClipBounds.Height;
                int width = (int)graphics.VisibleClipBounds.Width;
                SolidBrush drawingBrush = new SolidBrush(Color.Yellow);
                Bitmap map = new Bitmap(width, height);
                object locko = new object();
                Parallel.For(0, height, (i) =>
                {
                    for (int pixX = 0; pixX < width; pixX++)
                    {
                        (decimal cx, decimal cy) = CoordinateShenanigans.GetWorldCoords(pixX, i, parameters.screenCenter,
                            parameters.offset, parameters.scaleFactor);
                        decimal x = 0;
                        decimal y = 0;
                        decimal x2 = x * x;
                        decimal y2 = y * y;
                        int n = 0;
                        while (x2 + y2 <= parameters.escapeNumber && n < parameters.maxIterations)
                        {
                            (x, y) = ApplyFunction(parameters.appliedFunc, x, y, parameters.powerOfZ, x2, y2, cx, cy);
                            x2 = x * x;
                            y2 = y * y;
                            n++;
                        }
                        lock (locko)
                        {
                            if (n == parameters.maxIterations)
                            {
                                map.SetPixel(pixX, i, Color.Black);
                            }
                            map.SetPixel(pixX, i, FractalColoring.GetColor(n, parameters.maxIterations, parameters.palette));
                        }
                    }
                });
                graphics.DrawImage(map, 0, 0);
            }
            catch
            {
                MessageBox.Show("Calculations failed (due to overflow or precision error)", "Critical error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private (decimal, decimal) ApplyFunction(int func, decimal zr, decimal zi, decimal zpow, decimal zrzr, decimal zizi, decimal cr, decimal ci)
        {
            if (func == 0)
                return ((decimal)Math.Pow((double)(zrzr + zizi), (double)(zpow / 2.0M)) * (decimal)Math.Cos((double)(zpow * (decimal)Math.Atan2((double)zi, (double)zr))) + cr,
                        (decimal)Math.Pow((double)(zrzr + zizi), (double)(zpow / 2.0M)) * (decimal)Math.Sin((double)(zpow * (decimal)Math.Atan2((double)zi, (double)zr))) + ci);
            else if (func == 1)
            {
                Complex z = new Complex(Math.Abs((double)zr), -Math.Abs((double)zi));
                Complex res = Complex.Pow(z, (double)zpow) + new Complex((double)cr, (double)ci);
                return ((decimal)res.Real, (decimal)res.Imaginary);
            }
            return (0, 0);
        }
    }
}
