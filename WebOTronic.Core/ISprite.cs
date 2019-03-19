using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebOTronic.Core
{
    public interface ISprite
    {
        XYVector Position { get; }
        Size Size { get; }
        bool CollidesWith(ISprite sprite);
    }
}
