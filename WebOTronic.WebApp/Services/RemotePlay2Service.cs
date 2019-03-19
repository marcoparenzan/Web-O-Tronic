using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebOTronic.WebApp.Hubs;
using WebOTronic.WebApp.Models.RemotePlay2;

namespace WebOTronic.WebApp.Services
{
    public class RemotePlay2Service : IRemotePlay2Service
    {
        private IHubContext<RemotePlay2Hub> _hubContext;


        public RemotePlay2Service(IHubContext<RemotePlay2Hub> hubContext)
        {
            this._hubContext = hubContext;
        }

        private Task _running;
        private CancellationToken _ct;
        private Game _game;

        public World World { get; private set; }
        public string LeftPaddleId { get; private set; }
        public string RightPaddleId { get; private set; }

        public World Register(string leftPaddleId, string rightPaddleId)
        {
            LeftPaddleId = leftPaddleId;
            RightPaddleId = rightPaddleId;
            World = new World(new World.Options
            {
                X1 = 0,
                Y1 = 0,
                X2 = 1,
                Y2 = 0.75
            });

            _game = new Game(World);
            _ct = new CancellationToken();
            _running = Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    var start = DateTime.Now;

                    if (Monitor.TryEnter(_game, 25))
                    {
                        _game.Update();
                        _hubContext.Clients.User(LeftPaddleId).SendAsync("gameupdate", _game.currentEvent);
                        _hubContext.Clients.User(RightPaddleId).SendAsync("gameupdate", _game.currentEvent);
                        Monitor.Exit(_game);
                    }

                    var elapsed = (DateTime.Now - start).TotalMilliseconds;
                    var delay = 30 - elapsed;
                    if (delay > 0)
                    {
                        await Task.Delay((int)delay);
                    }
                }
            }, _ct);

            return World;
        }

        public void Run()
        {
            _game.Run();
        }

        public void LeftPaddle(string direction)
        {
            _game.LeftPaddle(direction);
        }

        public void RightPaddle(string direction)
        {
            _game.RightPaddle(direction);
        }
    }
}
