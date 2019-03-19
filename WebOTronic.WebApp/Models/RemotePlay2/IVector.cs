using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebOTronic.WebApp.Models.RemotePlay2
{
    public interface IVector<TVector>
        where TVector: IVector<TVector>
    {
        double X { get; }
        double Y { get; }
        TVector Clone();
    }
}
