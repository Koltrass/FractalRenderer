using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fractals.FractalPlotters
{
    public interface IFractalPlotter
    {
        public void DrawFractal(Graphics screen, ParamsForFractal parameters);
        public void DrawFractalMultithread(Graphics screen, ParamsForFractal parameters);
    }
}
