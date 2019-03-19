using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebOTronic.WebApp.Models.RemotePlay2
{
    public class XYVector : IVector<XYVector>
    {
        public XYVector(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public double X { get; set; }
        public double Y { get; set; }

        public XYVector Clone()
        {
            return new XYVector(this.X, this.Y);
        }

        public XYVector Add<TVector>(IVector<TVector> v)
            where TVector: IVector<TVector>
        {
            this.X += v.X;
            this.Y += v.Y;
            return this;
        }

        public XYVector Add(double? x = null, double? y = null)
        {
            this.X += x ?? 0;
            this.Y += y ?? 0;
            return this;
        }

        public override string ToString()
        {
            return $"X={X} Y={Y}";
        }
    }
}
