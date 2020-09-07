using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FootballMatches.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballMatches.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            // Test
            var team = _context.Teams.Find(1);
            var player = _context.Players.Find(1);
            var match = new Match()
            {
                Id = 1,
                HostTeamId = 1,
                GuestTeamId = 2,
            };
            _context.Matches.Add(match);
            var playersInMatch = new MatchPlayer()
            {
                MatchId = match.Id,
                PlayerId = player.Id
            };
            var goal = new Goal()
            {
                Id = 1,
                MatchPlayer = playersInMatch
            };
            _context.Goals.Add(goal);
            var test2 = _context.Goals.Find(1);
            var test = _context.Matches.Find(1);
            return View(player);
        }

        public string Test()
        {
            var team = _context.Teams.Find(1).ToString();
            return team;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
