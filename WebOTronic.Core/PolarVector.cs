using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebOTronic.Core
{
    public class PolarVector : IVector<PolarVector>
    {
        public double Mag { get; private set; }
        public double Angle { get; private set; }

        public double X { get; private set; }
        public double Y { get; private set; }

        public PolarVector(double mag, double angle)
        {
            this.Mag = mag;
            this.Angle = angle;
            this.X = 0;
            this.Y = 0;
            Cartesian();
        }

        public PolarVector Clone()
        {
            return new PolarVector(this.Mag, this.Angle);
        }

        public PolarVector UpdateAngle(double angle)
        {
            this.Angle = angle;
            Cartesian();
            return this;
        }

        public PolarVector UpdateX(double x)
        {
            this.X = x;
            Polar();
            return this;
        }

        public PolarVector UpdateY(double y)
        {
            this.Y = y;
            Polar();
            return this;
        }

        private void Cartesian()
        {
            this.X = this.Mag * Math.Cos(this.Angle);
            this.Y = this.Mag * Math.Sin(this.Angle);
        }

        private void Polar()
        {
            this.Mag = Math.Sqrt(this.X * this.X + this.Y * this.Y);
            this.Angle = Math.Atan2(this.Y, this.X);
        }

        public override string ToString()
        {
            return $"Mag={Mag} Angle={Angle}";
        }
    }
}