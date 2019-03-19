using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebOTronic.WebApp.Models.RemotePlay2
{
    public struct Size
    {
        public Size(double width, double height)
        {
            this.Width = width;
            this.Height = height;
        }

        public double Width { get; private set; }
        public double Height { get; private set; }
    }
}
