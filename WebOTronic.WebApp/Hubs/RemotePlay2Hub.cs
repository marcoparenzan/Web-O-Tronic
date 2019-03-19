using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebOTronic.WebApp.Models.RemotePlay2;
using WebOTronic.WebApp.Services;

namespace WebOTronic.WebApp.Hubs
{
    [Authorize]
    public class RemotePlay2Hub : Hub
    {
        private IRemotePlay2Service _service;

        public RemotePlay2Hub(IRemotePlay2Service service)
        {
            this._service = service;
        }

        public void Register(string userId)
        {
            var world = _service.Register(userId, Context.UserIdentifier);
            Clients.Caller.SendAsync("begin", new { world });
            Clients.User(userId).SendAsync("begin", new { world });
        }

        public async Task Run()
        {
            _service.Run();
        }

        public void NotifyLeftPaddle(string direction)
        {
            _service.LeftPaddle(direction);
        }

        public void NotifyRightPaddle(string direction)
        {
            _service.RightPaddle(direction);
        }

        public override Task OnConnectedAsync()
        {
            Clients.Caller.SendAsync("profile", new {
                userId = this.Context.UserIdentifier
            }).Wait();
            return base.OnConnectedAsync();
        }
    }
}
