using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebOTronic.WebApp.Models.RemotePlay2
{
    public interface ISprite
    {
        XYVector Position { get; }
        Size Size { get; }
        bool CollidesWith(ISprite sprite);
    }
}
