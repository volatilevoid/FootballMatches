using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FootballMatches.Data;
using FootballMatches.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FootballMatches.Controllers
{
    public class TeamController : Controller
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public TeamController(ITeamRepository teamRepository, IWebHostEnvironment webHostEnvironment)
        {
            _teamRepository = teamRepository;
            _webHostEnvironment = webHostEnvironment;
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
         * Add new team
         */
        [HttpPost]
        public IActionResult Add(string teamName, string teamDescription, IFormFile logo)
        {
            var newTeam = new Team()
            {
                Name = teamName,
                Description = teamDescription,
                Logo = logo == null ? "default_logo.png" : UploadedImage(logo)
            };
            _teamRepository.Add(newTeam);
            _teamRepository.Save();
            return RedirectToAction("Index");
        }
        /**
         * Remove player from team
         */
        [HttpPost]
        public JsonResult RemovePlayer(int id, int teamId)
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

        /**
         * Proccess uploaded team logo
         * 
         * Generate unique file name
         * Store image in filesystem
         * return new image name 
         */
        private string UploadedImage(IFormFile image)
        {
            string uniqueFileName;
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                image.CopyTo(fileStream);
            }
            return uniqueFileName;
        }
    }
}
