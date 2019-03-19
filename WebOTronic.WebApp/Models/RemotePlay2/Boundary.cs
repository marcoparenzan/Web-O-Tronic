using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebOTronic.WebApp.Models
{
    public class Boundary
    {
        public double X1 { get; private set; }
        public double Y1 { get; private set; }
        public double X2 { get; private set; }
        public double Y2 { get; private set; }
        public double Width { get; private set; }
        public double Height { get; private set; }

        public Boundary(Options options)
        {
            this.X1 = options.X1;
            this.Y1 = options.Y1;
            this.X2 = options.X2;
            this.Y2 = options.Y2;

            this.Width = this.X2 - this.X1;
            this.Height = this.Y2 - this.Y1;
        }

        public class Options
        {
            public double X1 { get; set; }
            public double Y1 { get; set; }
            public double X2 { get; set; }
            public double Y2 { get; set; }
        }
    }
}
