using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Fractals
{
    internal static class CoordinateShenanigans
    {
        public static Point GetScreenCoords(PointD p, Point center, PointD offset, decimal scaleFactor)
        {
            return new Point((int)((center.X+offset.X) + p.X * scaleFactor),
                (int)((center.Y+offset.Y) - p.Y * scaleFactor));
        }
        public static (decimal x, decimal y) GetWorldCoords(Point p, Point center, PointD offset, decimal scaleFactor)
        {
            return ((p.X - (center.X + offset.X)) / scaleFactor, -((p.Y - (center.Y + offset.Y)) / scaleFactor));
        }
        public static (decimal x, decimal y) GetWorldCoords(int x, int y, Point center, PointD offset, decimal scaleFactor)
        {
            return ((x - (center.X + offset.X)) / scaleFactor, -(y -(center.Y+ offset.Y)) / scaleFactor);
        }
        public static bool PointFits(Point point, Point center)
        {
            if (point.X > center.X*2 || point.X < 0)
                return false;
            if (point.Y > center.Y * 2 || point.Y < 0)
                return false;
            return true;
        }
        public static (decimal x, decimal y) GetDistanceFromZero(PointD newCenter, decimal scaleFactor)
        {
            return (-newCenter.X * scaleFactor, newCenter.Y * scaleFactor);
        }
    }
}
