using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using WebOTronic.Core;

namespace WebOTronic.GameApp.Hubs
{
    [Authorize]
    public class GameHub : Hub
    {
        private IGameFabric _fabric;

        public GameHub(IGameFabric fabric)
        {
            this._fabric = fabric;
        }

        public void PlayWith(string userId)
        {
            var world = _fabric.NewGame(userId, Context.UserIdentifier);
            Clients.User(userId).SendAsync("begin", new { leftPaddleId = userId, rightPaddleId = Context.UserIdentifier, world });
            Clients.Caller.SendAsync("begin", new { world });
        }

        public void Run()
        {
            _fabric[Context.UserIdentifier]?.Run();
        }

        public void NotifyLeftPaddle(string direction)
        {
            _fabric[Context.UserIdentifier]?.LeftPaddle(direction);
        }

        public void NotifyRightPaddle(string direction)
        {
            _fabric[Context.UserIdentifier]?.RightPaddle(direction);
        }
    }
}
