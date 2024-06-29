using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fractals.FractalPlotters
{
    public class IFSPlotter : IFractalPlotter
    {
        private IFS ifs;
        private Random rand = new Random();
        public IFSPlotter(IFS ifs)
        {
            this.ifs = ifs;
        }
        public void DrawFractal(Graphics screen, ParamsForFractal parameters)
        {
            screen.Clear(ifs.backColor);
            SolidBrush drawingBrush = new SolidBrush(ifs.colors[0]);
            PointD start = new PointD(0.0M, 0.0M);
            try
            {
                for (int i = 0; i < parameters.amountOfPoints; i++)
                {
                    Point realP = CoordinateShenanigans.GetScreenCoords(start, parameters.screenCenter, parameters.offset, parameters.scaleFactor);
                    screen.FillRectangle(drawingBrush, realP.X, realP.Y, 1, 1);
                    drawingBrush.Color = NextPoint(ref start);
                }
            }
            catch
            {
                MessageBox.Show("Coordinates too big to store. Check affine transformation value", "Critical error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            drawingBrush.Dispose();
        }

        public void DrawFractalMultithread(Graphics screen, ParamsForFractal parameters)
        {
            screen.Clear(ifs.backColor);
            SolidBrush drawingBrush = new SolidBrush(ifs.colors[0]);
            PointD start = new PointD(0.0M, 0.0M);
            try
            {
                for (int i = 0; i < parameters.amountOfPoints; i++)
                {
                    Point realP = CoordinateShenanigans.GetScreenCoords(start, parameters.screenCenter, parameters.offset, parameters.scaleFactor);
                    screen.FillRectangle(drawingBrush, realP.X, realP.Y, 1, 1);
                    drawingBrush.Color = NextPoint(ref start);
                }
            }
            catch
            {
                MessageBox.Show("Coordinates too big to store. Check affine transformation value", "Critical error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            drawingBrush.Dispose();
        }

        private Color NextPoint(ref PointD current)
        {
            decimal r = (decimal)rand.NextDouble();
            decimal xb = current.X;
            int i = 0;
            for (; i < ifs.p.Count; i++)
            {
                if (r < ifs.p[i])
                {
                    current.X = ifs.a[i] * current.X + ifs.b[i] * current.Y + ifs.e[i];
                    current.Y = ifs.c[i] * xb + ifs.d[i] * current.Y + ifs.f[i];
                    return ifs.colors[i];
                }
            }
            current.X = ifs.a[i] * current.X + ifs.b[i] * current.Y + ifs.e[i];
            current.Y = ifs.c[i] * xb + ifs.d[i] * current.Y + ifs.f[i];
            return ifs.colors[i];
        }
    }
}
