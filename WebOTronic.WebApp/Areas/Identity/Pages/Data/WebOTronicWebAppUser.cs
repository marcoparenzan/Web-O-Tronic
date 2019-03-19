using ElCamino.AspNetCore.Identity.AzureTable.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebOTronic.WebApp.Areas.Identity.Data
{
    public class WebOTronicWebAppUser : IdentityUserV2 //or use IdentityUser if your code depends on the Role, Claim and Token collections
    {
    }
}
