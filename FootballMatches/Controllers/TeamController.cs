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
        public IActionResult Index()
        {
            List<Team> teams = _teamRepository.All();

            return View(teams);
        }

        public IActionResult Details(int id)
        {
            var team = _teamRepository.Get(id);

            return View(team);
        }
    }
}
