using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballMatches.Data;
using FootballMatches.Models;
using Microsoft.AspNetCore.Mvc;
using FootballMatches.ViewModels;

namespace FootballMatches.Controllers
{
    public class StatisticsController : Controller
    {
        private IStatisticsRepository _statisticsRepository;
        public StatisticsController(IStatisticsRepository statisticsRepository)
        {
            _statisticsRepository = statisticsRepository;
        }
        public IActionResult Index()
        {
            StatisticsViewModel model = new StatisticsViewModel()
            {
                PlayerStatistics = ParsePlayerStatistics(_statisticsRepository.Players()),
                TeamStatistics = ParseTeamStatistics(_statisticsRepository.Teams())
            };

            return View(model);
        }

        private Dictionary<int, PlayerStatistics> ParsePlayerStatistics(List<Player> rawPlayersData)
        {
            Dictionary<int, PlayerStatistics> allPlayersStatistics = new Dictionary<int, PlayerStatistics>();

            foreach(Player player in rawPlayersData)
            {
                if(player.MatchPlayers != null && player.MatchPlayers.Count() != 0)
                {
                    // Player played a match
                    if (!allPlayersStatistics.ContainsKey(player.Id))
                    {
                        PlayerStatistics newstatsToTrack = new PlayerStatistics()
                        {
                            PlayerId = player.Id,
                            PlayerName = player.Name,
                            PlayerTeam = player.Team.Name,
                            MatchesPlayed = player.MatchPlayers.Count(),
                            GoalsScored = CountGoals(player.MatchPlayers)
                        };
                        allPlayersStatistics.Add(player.Id, newstatsToTrack);
                    }
                }
            }
            return allPlayersStatistics.OrderByDescending(ps => ps.Value.GoalsScored).ToDictionary(ps => ps.Key, ps => ps.Value);
        }

        private int CountGoals(List<MatchPlayer> playedMatches)
        {
            int goalsScored = 0;
            foreach(MatchPlayer playedMatch in playedMatches)
            {
                goalsScored += playedMatch.Goals.Count();
            }
            return goalsScored;
        }

        /**
        * Team statistics view model
        * WinDrawLossViewModel[0] - Host matches
        * WinDrawLossViewModel[1] - Guest matches
        */
        private List<TeamStatistics> ParseTeamStatistics(List<Team> rawTeamsData) 
        {
            List<TeamStatistics> allTeamsStatistics = new List<TeamStatistics>();

            foreach(Team team in rawTeamsData)
            {
                WinDrawLossViewModel hostScores = ParseHostScores(team.HostMatches);
                WinDrawLossViewModel guestScores = ParseGuestScores(team.GuestMatches);
                //WinDrawLossViewModel totalScores = new WinDrawLossViewModel()
                //{
                //    Win = hostScores.Win + guestScores.Win,
                //    Draw = hostScores.Draw + guestScores.Draw,
                //    Loss = hostScores.Loss + guestScores.Loss
                //};

                TeamStatistics teamStats = new TeamStatistics()
                {
                    TeamId = team.Id,
                    TeamName = team.Name,
                    Statistics = new WinDrawLossViewModel 
                    {
                        Win = hostScores.Win + guestScores.Win,
                        Draw = hostScores.Draw + guestScores.Draw,
                        Loss = hostScores.Loss + guestScores.Loss
                    }
                };
                allTeamsStatistics.Add(teamStats);
            }
            return allTeamsStatistics;
        }

        private WinDrawLossViewModel ParseHostScores(List<Match> matches)
        {
            WinDrawLossViewModel stats = new WinDrawLossViewModel()
            {
                Win = 0,
                Draw = 0,
                Loss = 0
            };
            foreach(Match match in matches)
            {
                if (IsMatchFinishedRegulary(match))
                {
                    if (match.HostScore == match.GuestScore)
                    {
                        stats.Draw += 1;
                    }
                    else if (match.HostScore > match.GuestScore)
                    {
                        stats.Win += 1;
                    }
                    else
                    {
                        stats.Loss += 1;
                    }
                }
            }
            return stats;
        }
        private WinDrawLossViewModel ParseGuestScores(List<Match> matches)
        {
            WinDrawLossViewModel stats = new WinDrawLossViewModel()
            {
                Win = 0,
                Draw = 0,
                Loss = 0
            };

            foreach (Match match in matches)
            {
                if (IsMatchFinishedRegulary(match))
                {
                    if (match.HostScore == match.GuestScore)
                    {
                        stats.Draw += 1;
                    }
                    else if (match.HostScore < match.GuestScore)
                    {
                        stats.Win += 1;
                    }
                    else
                    {
                        stats.Loss += 1;
                    }
                }
            }
            return stats;
        }
        private bool IsMatchFinishedRegulary(Match match)
        {
            if(match.Status.Default || (!match.Status.IsMatchStateChangeable && match.Status.AreTeamsAvailable))
            {
                // Not started or canceled
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
