using System;
using System.Diagnostics;
using System.Threading;

namespace WebOTronic.WebApp.Models.RemotePlay2
{
    public class Game
    {
        public World world { get; private set; }
        public Ball ball { get; private set; }
        public Paddle leftPaddle { get; private set; }
        public Paddle rightPaddle { get; private set; }

        public Event currentEvent { get; private set; }

        public Game(World world)
        {
            this.world = world;
            ball = new Ball(new Ball.BallOptions
            {
                Size = new Size(0.03125, 0.16),
                Boundary = world,
                Movement = 0.008
            });
            leftPaddle = new Paddle(new Paddle.PaddleOptions
            {
                Size = new Size(0.03125, 0.16),
                Boundary = new Boundary(new Boundary.Options
                {
                    X1 = world.X1,
                    Y1 = world.Y1,
                    X2 = world.X2 / 2,
                    Y2 = world.Y2
                }),
                Movement = 0.01,
                RestartPosition = new XYVector(world.X1, world.Y2 / 2)
            });
            rightPaddle = new Paddle(new Paddle.PaddleOptions
            {
                Size = new Size(0.03125, 0.16),
                Boundary = new Boundary(new Boundary.Options
                {
                    X1 = world.X2 / 2,
                    Y1 = world.Y1,
                    X2 = world.X2,
                    Y2 = world.Y2
                }),
                Movement = 0.01,
                RestartPosition = new XYVector(world.X2, world.Y2 / 2)
            });
        }

        public void Run()
        {
            ball.State.Running();
        }

        public void Update()
        {
            if (ball.State.State == "running")
            {
                if (ball.CollidesWith(leftPaddle) && (ball.State.Speed.X < 0))
                {
                    ball.BounceX(leftPaddle);
                }
                else if (ball.CollidesWith(rightPaddle) && (ball.State.Speed.X > 0))
                {
                    ball.BounceX(rightPaddle);
                }
                else if (ball.State.Position.X >= ball.Boundary.X2 && (ball.State.Speed.X > 0))
                {
                    ball.Score(leftPaddle, rightPaddle);
                    if (leftPaddle.State.Score == 15)
                    {
                        currentEvent = NewEvent("leftpaddlewin");
                        return;
                    }
                }
                else if (ball.State.Position.X <= ball.Boundary.X1 && (ball.State.Speed.X < 0))
                {
                    ball.Score(rightPaddle, leftPaddle);
                    if (rightPaddle.State.Score == 15)
                    {
                        currentEvent = NewEvent("rightpaddlewin");
                        return;
                    }
                }
                else if ((ball.State.Position.Y >= ball.Boundary.Y2) && ball.State.Speed.Y > 0)
                {
                    ball.BounceY();
                }
                else if ((ball.State.Position.Y <= ball.Boundary.Y1) && ball.State.Speed.Y < 0)
                {
                    ball.BounceY();
                }
                else
                {
                    ball.State.Position.Add(ball.State.Speed);
                }
                currentEvent = NewEvent("gameupdate");
            }
            else // 
            {
                currentEvent = NewEvent("gameupdate");
            }
        }

        public void LeftPaddle(string direction)
        {
            Paddle(leftPaddle, direction);
        }

        public void RightPaddle(string direction)
        {
            Paddle(rightPaddle, direction);
        }

        private void Paddle(Paddle paddle, string direction)
        {
            switch (direction)
            {
                case "left":
                    paddle.Left();
                    break;
                case "right":
                    paddle.Right();
                    break;
                case "up":
                    paddle.Up();
                    break;
                case "down":
                    paddle.Down();
                    break;
            }
        }

        private Event NewEvent(string type)
        {
            return new Event(type, leftPaddle.State, rightPaddle.State, ball.State);
        }

        private Event NewEvent<TArgs>(string type, TArgs args = null)
            where TArgs : class
        {
            return new Event(type, leftPaddle.State, rightPaddle.State, ball.State, args);
        }
    }
}
