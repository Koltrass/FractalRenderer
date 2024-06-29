using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fractals
{
    public class IFS
    {
        public List<decimal> a = new List<decimal>();
        public List<decimal> b = new List<decimal>();
        public List<decimal> c = new List<decimal>();
        public List<decimal> d = new List<decimal>();
        public List<decimal> e = new List<decimal>();
        public List<decimal> f = new List<decimal>();
        public List<decimal> p = new List<decimal>();
        public List<Color> colors = new List<Color>();
        public Color backColor;
        public IFS(List<decimal> a, List<decimal> b, List<decimal> c, List<decimal> d, List<decimal> e, List<decimal> f, List<decimal> p, List<Color> colors, Color backColor)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
            this.e = e;
            this.f = f;
            this.p.Add(p[0]);
            for (int i = 1; i < p.Count - 1; i++)
            {
                this.p.Add(this.p[i - 1] + p[i]);
            }
            this.colors = colors;
            this.backColor = backColor; 
        }
    }
}