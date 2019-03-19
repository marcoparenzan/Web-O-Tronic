using System;
using System.Threading;
using System.Threading.Tasks;
using WebOTronic.Core;

namespace WebOTronic.Core
{
    public class Game : IGame
    {
        private IGameContext _context;

        public Game(IGameContext context)
        {
            this._context = context;
        }

        private Task _running;
        private CancellationToken _ct;

        private World _world;

        private Ball _ball;
        private Paddle _leftPaddle;
        private Paddle _rightPaddle;

        private Event CurrentEvent;

        public World Init(string leftPaddleId, string rightPaddleId)
        {
            _world = new World(new World.Options
            {
                X1 = 0,
                Y1 = 0,
                X2 = 1,
                Y2 = 0.75
            });

            #region GameInit

            _ball = new Ball(new Ball.BallOptions
            {
                Size = new Size(0.03125, 0.16),
                Boundary = _world,
                Movement = 0.01
            });
            _leftPaddle = new Paddle(new Paddle.PaddleOptions
            {
                Size = new Size(0.03125, 0.16),
                Boundary = new Boundary(new Boundary.Options
                {
                    X1 = _world.X1,
                    Y1 = _world.Y1,
                    X2 = _world.X2 / 2,
                    Y2 = _world.Y2
                }),
                Movement = 0.015,
                RestartPosition = new XYVector(_world.X1, _world.Y2 / 2)
            });
            _rightPaddle = new Paddle(new Paddle.PaddleOptions
            {
                Size = new Size(0.03125, 0.16),
                Boundary = new Boundary(new Boundary.Options
                {
                    X1 = _world.X2 / 2,
                    Y1 = _world.Y1,
                    X2 = _world.X2,
                    Y2 = _world.Y2
                }),
                Movement = 0.015,
                RestartPosition = new XYVector(_world.X2, _world.Y2 / 2)
            });

            #endregion

            _ct = new CancellationToken();
            _running = Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    var start = DateTime.Now;

                    #region Update

                    if (_ball.State.State == "running")
                    {
                        if (_ball.CollidesWith(_leftPaddle) && (_ball.State.Speed.X < 0))
                        {
                            _ball.BounceX(_leftPaddle);
                        }
                        else if (_ball.CollidesWith(_rightPaddle) && (_ball.State.Speed.X > 0))
                        {
                            _ball.BounceX(_rightPaddle);
                        }
                        else if (_ball.State.Position.X >= _ball.Boundary.X2 && (_ball.State.Speed.X > 0))
                        {
                            _ball.Score(_leftPaddle, _rightPaddle);
                            if (_leftPaddle.State.Score == 15)
                            {
                                CurrentEvent = NewEvent("leftpaddlewin");
                                return;
                            }
                        }
                        else if (_ball.State.Position.X <= _ball.Boundary.X1 && (_ball.State.Speed.X < 0))
                        {
                            _ball.Score(_rightPaddle, _leftPaddle);
                            if (_rightPaddle.State.Score == 15)
                            {
                                CurrentEvent = NewEvent("rightpaddlewin");
                                return;
                            }
                        }
                        else if ((_ball.State.Position.Y >= _ball.Boundary.Y2) && _ball.State.Speed.Y > 0)
                        {
                            _ball.BounceY();
                        }
                        else if ((_ball.State.Position.Y <= _ball.Boundary.Y1) && _ball.State.Speed.Y < 0)
                        {
                            _ball.BounceY();
                        }
                        else
                        {
                            _ball.State.Position.Add(_ball.State.Speed);
                        }
                        CurrentEvent = NewEvent("gameupdate");
                    }
                    else // 
                    {
                        CurrentEvent = NewEvent("gameupdate");
                    }

                    #endregion

                    await _context.GameUpdate(leftPaddleId, CurrentEvent);
                    await _context.GameUpdate(rightPaddleId, CurrentEvent);

                    var elapsed = (DateTime.Now - start).TotalMilliseconds;
                    var delay = 20 - elapsed;
                    if (delay > 0)
                    {
                        await Task.Delay((int)delay);
                    }
                }
            }, _ct);

            return _world;
        }

        void IGame.Run()
        {
            _ball.State.Running();
        }

        void IGame.LeftPaddle(string direction)
        {
            P(_leftPaddle, direction);
        }

        void IGame.RightPaddle(string direction)
        {
            P(_rightPaddle, direction);
        }

        //

        private void P(Paddle paddle, string direction)
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
            return new Event(type, _leftPaddle.State, _rightPaddle.State, _ball.State);
        }

        private Event NewEvent<TArgs>(string type, TArgs args = null)
            where TArgs : class
        {
            return new Event(type, _leftPaddle.State, _rightPaddle.State, _ball.State, args);
        }
    }
}
