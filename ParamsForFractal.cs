using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fractals
{
    public class ParamsForFractal
    {
        public Point screenCenter = new Point();
        public PointD offset = new PointD();
        public decimal scaleFactor;
        public int palette;
        public decimal powerOfZ;
        public int appliedFunc;
        public decimal escapeNumber;
        public int maxIterations;
        public decimal cx;
        public decimal cy;

        public int amountOfPoints;
    }
}
