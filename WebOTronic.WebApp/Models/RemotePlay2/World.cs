﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebOTronic.WebApp.Models.RemotePlay2
{
    public class World: Boundary
    {
        public World(Options options): base(options)
        {
        }

        public new class Options: Boundary.Options
        {

        }
    }
}
