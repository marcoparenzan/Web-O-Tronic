using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebOTronic.WebApp.Controllers
{
    [Authorize]
    public class RemotePlay2Controller : Controller
    {
        public IActionResult Game(string leftpaddle = null)
        {
            ViewBag.leftpaddle = leftpaddle;
            return View();
        }
    }
}