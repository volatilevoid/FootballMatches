﻿using System;
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
        /**
         * Match details
         */
        public IActionResult Details(int id)
        {
            Match matchDetails = _matchRepository.Get(id);

            List<Player> hostSquad = new List<Player>();
            List<Player> guestSquad = new List<Player>();

            foreach(MatchPlayer matchPlayer in matchDetails.MatchPlayers)
            {
                if(matchPlayer.Player.TeamId == matchDetails.HostTeamId)
                {
                    hostSquad.Add(matchPlayer.Player);
                }
                else if(matchPlayer.Player.TeamId == matchDetails.GuestTeamId)
                {
                    guestSquad.Add(matchPlayer.Player);
                }
            }

            MatchDetailsViewModel model = new MatchDetailsViewModel()
            {
                Id = matchDetails.Id,
                HostTeamId = matchDetails.HostTeamId,
                HostTeamName = matchDetails.HostTeam.Name,
                GuestTeamId = matchDetails.GuestTeamId,
                GuestTeamName = matchDetails.GuestTeam.Name,
                HostScore = matchDetails.HostScore,
                GuestScore = matchDetails.GuestScore,
                Place = matchDetails.Place,
                Date = matchDetails.Date,
                Status = matchDetails.Status,
                HostSquad = hostSquad,
                GuestSquad = guestSquad
            };


            return View(model);
        }
        /**
         * Available teams and players for new match creation
         */
        public IActionResult New(DateTime matchDate)
        {
            var viewModel = new NewMatchViewModel()
            {
                Teams = _matchRepository.AvailableTeams(DateTime.Today),
                Statuses = _matchRepository.Statuses()
            };

            return View(viewModel);
        }
        /**
         * Create new match
         */
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
            if (hostId == guestId || hostPlayerIDs.Count() != guestPlayerIDs.Count() || hostPlayerIDs.Count() < minPlayers || guestPlayerIDs.Count() < minPlayers || matchPlace== null || matchPlace.Length == 0)
            {
                return Json(false);
            }
            // Create new match
            Match newMatch = new Match()
            {
                HostTeamId = hostId,
                GuestTeamId = guestId,
                HostScore = 0,
                GuestScore = 0,
                Date = date,
                Place = matchPlace,
                StatusId = defaultStatus.Id
            };
            _matchRepository.Add(newMatch);
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
         * Get available team players for the match
         */
        [HttpGet]
        public List<Player> AvailablePlayers(int teamId)
        {
            return _matchRepository.AvailablePlayers(teamId);
        }
        /**
         * Update match status
         */
        public IActionResult UpdateStatus(int matchId, int statusId)
        {
            Status cancelStatus = _matchRepository.Statuses().Find(s => s.AreTeamsAvailable == true);
            
            Match matchToUpdate = _matchRepository.Get(matchId);
            matchToUpdate.StatusId = statusId;
            if(statusId == cancelStatus.Id)
            {
                // Match canceled 
                // -> players made no appearance
                // -> Teams can play some other match on that day
                matchToUpdate.MatchPlayers = new List<MatchPlayer>();
                _matchRepository.Update(matchToUpdate);
                _matchRepository.Save();
                return RedirectToAction("Index", "Match");
            }
            _matchRepository.Update(matchToUpdate);
            _matchRepository.Save();

            return RedirectToAction("Details", new { id = matchId});
        }
        [HttpPost]
        public IActionResult UpdateScore(int matchId, int teamId, int playerId)
        {
            // If playerId == 0 fix!!!!
            if(playerId == 0)
            {
                return RedirectToAction("Details", new { id = matchId });
            }
            Match match = _matchRepository.Get(matchId);
            Goal newGoal = new Goal()
            {
                MatchId = matchId,
                PlayerId = playerId
            };
            int len = match.MatchPlayers.Count();
            for (int i = 0; i < len; i++)
            {
                if(match.MatchPlayers[i].PlayerId == playerId)
                {
                    match.MatchPlayers[i].Goals.Add(newGoal);
                }
            }
            // Update score
            if(match.HostTeamId == teamId)
            {
                match.HostScore += 1;
            }
            else if(match.GuestTeamId == teamId)
            {
                match.GuestScore += 1;
            }
            _matchRepository.Update(match);
            _matchRepository.Save();
            return RedirectToAction("Details", new { id = matchId });
        }
    }
}
