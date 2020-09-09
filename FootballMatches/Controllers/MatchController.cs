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
                availableTeams = AvailableTeams(DateTime.Today),
                statuses = _matchRepository.Statuses()
            };

            return View(viewModel);
        }
        [HttpGet]
        public List<Team> AvailableTeams(DateTime matchDate)
        {
            return _matchRepository.AvailableTeams(matchDate);
        }
        [HttpGet]
        public List<Player> AvailablePlayers(int teamId)
        {
            return _matchRepository.AvailablePlayers(teamId);
        }
    }
}
