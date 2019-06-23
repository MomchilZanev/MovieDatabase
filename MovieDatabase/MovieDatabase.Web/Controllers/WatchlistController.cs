using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MovieDatabase.Web.Controllers
{
    public class WatchlistController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}