using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballMatches.Data;
using FootballMatches.Models;
using Microsoft.AspNetCore.Mvc;

namespace FootballMatches.Controllers
{
    public class TeamController : Controller
    {
        private readonly ITeamRepository _teamRepository;
        public TeamController(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }
        /**
         * All teams
         */
        public IActionResult Index()
        {
            List<Team> teams = _teamRepository.All();

            return View(teams);
        }
        /**
         * Team details
         */
        public IActionResult Details(int id)
        {
            var team = _teamRepository.Get(id);
            return View(team);
        }
        /**
         * Remove player from team
         */
        [HttpPost]
        public IActionResult RemovePlayer(int id, int teamId)
        {
            _teamRepository.RemovePlayer(id, teamId);
            _teamRepository.Save();
            return Json(new { playerRemoved = true });
        }
        /**
         * Add new player to team
         */
        [HttpPost]
        public IActionResult AddPlayer(int teamId, string playerName)
        {
            Player newPlayer = new Player()
            {
                Name = playerName,
                TeamId = teamId
            };
            _teamRepository.AddPlayer(newPlayer);
            _teamRepository.Save();

            return RedirectToAction("Details", new { id = teamId });
        }
    }
}
