using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebOTronic.Core;

namespace WebOTronic.Core
{
    public interface IGameContext
    {
        IGame this[string userId] { get; }
        Task GameUpdate(string userId, Event ev);
    }
}
