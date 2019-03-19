using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebOTronic.Core;

namespace WebOTronic.Core
{
    public interface IGameFabric: IGameContext
    {
        World NewGame(string left, string right);
    }
}
