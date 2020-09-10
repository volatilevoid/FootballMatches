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
            Match model = _matchRepository.Get(id);
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
        public IActionResult Create(int hostId, int guestId, string matchDate, string matchPlace, int[] hostPlayerIDs, int[] guestPlayerIDs)
        {
            int minPlayers = 6;
            Status defaultStatus = _matchRepository.DefaultStatus();
            // Validate date
            DateTime date;
            if ( !DateTime.TryParse(matchDate, out date) || DateTime.Compare(DateTime.Today.Date, date.Date) > 0)
            {
                return Json(false);
            }
            // Validate other fields
            if (hostId == guestId || hostPlayerIDs.Count() < minPlayers || guestPlayerIDs.Count() < minPlayers || matchPlace== null || matchPlace.Length == 0)
            {
                return Json(false);
            }
            // Create new match
            Match newMatch = new Match()
            {
                HostTeamId = hostId,
                GuestTeamId = guestId,
                HomeScore = 0,
                GuestScore = 0,
                Date = date,
                Place = matchPlace,
                StatusId = defaultStatus.Id
            };
            _matchRepository.Create(newMatch);
            _matchRepository.Save();
            // Add host players
            foreach (int hostPlayerId in hostPlayerIDs)
            {
                MatchPlayer playerOnMatch = new MatchPlayer()
                {
                    MatchId = newMatch.Id,
                    PlayerId = hostPlayerId
                };
                _matchRepository.AddMatchPlayer(playerOnMatch);
            }
            // Add guest players
            foreach (int guestPlayerId in guestPlayerIDs)
            {
                MatchPlayer playerOnMatch = new MatchPlayer()
                {
                    MatchId = newMatch.Id,
                    PlayerId = guestPlayerId
                };
                _matchRepository.AddMatchPlayer(playerOnMatch);
            }
            _matchRepository.Save();

            return Json(true);
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
