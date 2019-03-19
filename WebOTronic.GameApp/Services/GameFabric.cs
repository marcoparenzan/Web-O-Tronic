using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebOTronic.Core;
using WebOTronic.GameApp.Hubs;

namespace WebOTronic.GameApp.Services
{
    public class GameFabric : IGameFabric
    {
        private IHubContext<GameHub> _hub;
        private Dictionary<string, IGame> _userServiceMap = new Dictionary<string, IGame>();

        public GameFabric(IHubContext<GameHub> hub)
        {
            _hub = hub;
        }

        IGame IGameContext.this[string user]
        {
            get {
                if (_userServiceMap.ContainsKey(user))
                {
                    return _userServiceMap[user];
                }
                else
                {
                    return null;
                }
            }
        }

        World IGameFabric.NewGame(string left, string right)
        {
            var service = new Game(this);

            void Add(string userId)
            {
                if (_userServiceMap.ContainsKey(userId))
                    _userServiceMap.Remove(userId);
                _userServiceMap.Add(userId, service);
            }

            Add(left);
            Add(right);
            var world = service.Init(left, right);
            return world;
        }

        async Task IGameContext.GameUpdate(string userId, Event ev)
        {
            await _hub.Clients.User(userId).SendAsync("gameupdate", ev);
        }
    }
}
