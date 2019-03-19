using System;

namespace WebOTronic.Core
{
    public class Ball: ISprite
    {
        public Size Size { get; private set; }
        public Boundary Boundary { get; private set; }
        public double Movement { get; private set; }

        public BallState State { get; } = new BallState();

        XYVector ISprite.Position
        {
            get => State.Position;
        }

        private static Random __random__ = new Random();

        public Ball(BallOptions options)
        {
            this.Size = options.Size;
            this.Boundary = options.Boundary;
            this.Movement = options.Movement ?? 0.00785;
            this.State = new BallState
            {
            };
            this.State.ResetPositionTo(new XYVector(this.Boundary.X1+this.Boundary.Width/2, this.Boundary.Y1 + this.Boundary.Height / 2));
            var r = __random__.NextDouble();
            if (r > 0.5)
            {
                this.State.ResetSpeedTo(new PolarVector(this.Movement, Angles.P135));
            }
            else
            {
                this.State.ResetSpeedTo(new PolarVector(this.Movement, Angles.P45));
            }
            this.State.Stopped();
        }

        public bool CollidesWith(ISprite sprite)
        {
            return this.State.Position.X < sprite.Position.X + sprite.Size.Width &&
              this.State.Position.X + this.Size.Width > sprite.Position.X &&
              this.State.Position.Y < sprite.Position.Y + sprite.Size.Height &&
              this.State.Position.Y + this.Size.Height > sprite.Position.Y;
        }

        public Ball BounceX(Paddle paddle)
        {
            var index = (this.State.Position.Y - paddle.State.Position.Y) / (paddle.Size.Height / 2);
            if (index < -0.98) index = -0.98;
            if (index > +0.98) index = +0.98;

            var alpha = 0.0;
            if (this.State.Speed.X < 0)
            { // left
                alpha = index * Math.PI / 3;
            }
            else if (this.State.Speed.X > 0)
            { // right
                alpha = Math.PI - index * Math.PI / 3;
            }
            this.State.Speed.UpdateAngle(alpha);
            return this;
        }

        public Ball BounceY()
        {
            State.Speed.UpdateY(-State.Speed.Y);
            return this;
        }

        public Ball Score(Paddle paddle, Paddle other)
        {
            this.State.Stopped();

            paddle.WonPoint();
            other.LostPoint();

            this.State.ResetPositionTo(other.State.Position.Clone());
            if (other.State.Position.X > (Boundary.X1+Boundary.Width/2))
            {
                // randomize angle
                this.State.ResetSpeedTo(new PolarVector(this.Movement, Angles.P135));
            }
            else
            {
                // randomize angle
                this.State.ResetSpeedTo(new PolarVector(this.Movement, Angles.P45));
            }

            return this;
        }

        public class BallOptions
        {
            public Size Size { get; set; }
            public Boundary Boundary { get; set; }
            public double? Movement { get; set; }
        }

        public class BallState
        {
            public XYVector Position { get; private set; }
            public PolarVector Speed { get; private set; }
            public string State { get; private set; }

            public void ResetPositionTo(XYVector v)
            {
                Position = v;
            }

            public void ResetSpeedTo(PolarVector v)
            {
                Speed = v;
            }

            public void Stopped()
            {
                State = "stopped";
            }

            public void Running()
            {
                State = "running";
            }
        }
    }
}