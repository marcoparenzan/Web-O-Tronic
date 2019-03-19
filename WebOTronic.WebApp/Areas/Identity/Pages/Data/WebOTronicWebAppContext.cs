using ElCamino.AspNetCore.Identity.AzureTable;
using ElCamino.AspNetCore.Identity.AzureTable.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebOTronic.WebApp.Areas.Identity.Data
{
    public class WebOTronicWebAppContext : IdentityCloudContext
    {
        public WebOTronicWebAppContext()
        {
        }

        public WebOTronicWebAppContext(IdentityConfiguration config): base(config)
        {
        }
    }
}
