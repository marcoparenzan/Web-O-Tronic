using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebOTronic.WebApp.Models.RemotePlay2
{
    public class Event
    {
        public Paddle.PaddleState LeftPaddle { get; private set; }
        public Paddle.PaddleState RightPaddle { get; private set; }
        public Ball.BallState Ball { get; private set; }
        public object Args { get; private set; }

        public string Type { get; private set; }

        public Event(string type, Paddle.PaddleState leftPaddle = null, Paddle.PaddleState rightPaddle = null, Ball.BallState ball = null, object args = null)
        {
            if (leftPaddle != null) this.LeftPaddle = leftPaddle;
            if (rightPaddle != null) this.RightPaddle = rightPaddle;
            if (ball != null) this.Ball = ball;
            if (args != null) this.Args = args;
            this.Type = type;
        }
    }
}
