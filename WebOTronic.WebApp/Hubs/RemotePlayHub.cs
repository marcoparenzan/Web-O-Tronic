using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebOTronic.WebApp.Hubs
{
    [Authorize]
    public class RemotePlayHub : Hub
    {
        public async Task PlayWith(string userId)
        {
            var args = new {
                leftpaddle = userId,
                rightpaddle = Context.UserIdentifier
            };
            await Clients.Caller.SendAsync("begin", args);
            await Clients.User(userId).SendAsync("begin", args);
        }

        public async Task NotifyLeftPaddle(string userId, object args)
        {
            await Clients.User(userId).SendAsync("leftpaddleupdate", args);
        }

        public async Task NotifyRightPaddle(string userId, object args)
        {
            await Clients.User(userId).SendAsync("rightpaddleupdate", args);
        }

        public async Task NotifyBall(string userId, object args)
        {
            await Clients.User(userId).SendAsync("ballupdate", args);
        }

        public override Task OnConnectedAsync()
        {
            Clients.Caller.SendAsync("profile", new {
                userId = this.Context.UserIdentifier
            }).Wait();
            return base.OnConnectedAsync();
        }
    }

    public class Paddle
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class Ball
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int SpeedX { get; set; }
        public int SpeedY { get; set; }
    }
}
