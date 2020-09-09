using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballMatches.Data;
using FootballMatches.Models;
using FootballMatches.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FootballMatches.Controllers
{
    public class MatchController : Controller
    {
        private IMatchRepository _matchRepository;

        public MatchController(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }
        public IActionResult Index()
        {
            var viewModel = new AllMatchesViewModel()
            {
                Matches = _matchRepository.Matches(),
                Statuses = _matchRepository.Statuses()
            };

            return View(viewModel);
        }
        public IActionResult Details(int id)
        {

            return View();
        }
        public IActionResult New(DateTime matchDate)
        {
            var viewModel = new NewMatchViewModel()
            {
                availableTeams = _matchRepository.AvailableTeams(DateTime.Today),
                statuses = _matchRepository.Statuses()
            };

            return View(viewModel);
        }
        [HttpPost]
        public JsonResult Create(int hostId, int guestId, string matchDate, string matchPlace, int[] hostPlayers, int[] guestPlayers)
        {
            // TODO: backend validation + store new match
            Team hostTeam = _matchRepository.Team(hostId);
            Team guestTeam = _matchRepository.Team(guestId);
            Status defaultStatus = _matchRepository.DefaultStatus();


            return Json(new { matchCreated = true });
        }
        /**
         * Get teams that don't have match on matchDate
         */
        [HttpGet]
        public List<Team> AvailableTeams(string matchDate)
        {
            return _matchRepository.AvailableTeams(DateTime.Parse(matchDate));
        }
        /**
         * Get available team players for match
         */
        [HttpGet]
        public List<Player> AvailablePlayers(int teamId)
        {
            return _matchRepository.AvailablePlayers(teamId);
        }

    }
}
