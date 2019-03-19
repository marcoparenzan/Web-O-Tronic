using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebOTronic.Core
{
    public class Paddle: ISprite
    {
        public XYVector RestartPosition { get; set; }
        public Size Size { get; set; }
        public Boundary Boundary { get; set; }
        public double Movement { get; set; }

        public PaddleState State { get; } = new PaddleState();

        XYVector ISprite.Position
        {
            get => State.Position;
        }

        public Paddle(PaddleOptions options)
        {
            this.RestartPosition = options.RestartPosition;
            this.Size = options.Size;
            this.Boundary = options.Boundary;
            this.Movement = options.Movement ?? 12;

            this.State.ResetPositionTo(options.RestartPosition);
            this.State.ResetScore();
        }

        public bool CollidesWith(ISprite sprite)
        {
            return 
                this.State.Position.X < sprite.Position.X + sprite.Size.Width &&
                this.State.Position.X + this.Size.Width > sprite.Position.X &&
                this.State.Position.Y < sprite.Position.Y + sprite.Size.Height &&
                this.State.Position.Y + this.Size.Height > sprite.Position.Y;
        }

        public Paddle WonPoint()
        {
            this.State.AddScore();
            this.State.ResetPositionTo(this.RestartPosition.Clone());
            return this;
        }

        public Paddle LostPoint()
        {
            this.State.ResetPositionTo(this.RestartPosition.Clone());
            return this;
        }
        
        public Paddle Up()
        {
            if (this.State.Position.Y > this.Boundary.Y1)
            {
                this.State.Position.Add(y: -this.Movement);
            }
            return this;
        }

        public Paddle Down()
        {
            if (this.State.Position.Y < this.Boundary.Y2)
            {
                this.State.Position.Add(y: this.Movement);
            }
            return this;
        }

        public Paddle Left()
        {
            if (this.State.Position.X > this.Boundary.X1)
            {
                this.State.Position.Add(x: -this.Movement);
            }
            return this;
        }

        public Paddle Right()
        {
            if (this.State.Position.X < this.Boundary.X2)
            {
                this.State.Position.Add(x: this.Movement);
            }
            return this;
        }

        public class PaddleOptions
        {
            public XYVector RestartPosition { get; set; }
            public Size Size { get; set; }
            public Boundary Boundary { get; set; }
            public double? Movement { get; set; }
        }

        public class PaddleState
        {
            public XYVector Position { get; private set; }
            public int Score { get; private set; }

            public void AddScore(int value = 1)
            {
                Score += value;
            }

            public void ResetScore()
            {
                Score = 0;
            }

            public void ResetPositionTo(XYVector v)
            {
                Position = v;
            }
        }
    }
}
