using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballMatches.ResourceModels;
using FootballMatches.Models;

namespace FootballMatches.API
{
    public interface IMatchResponseFormatService
    {
        public List<MatchResourceModel> Format(List<Match> matches);
    }
    /**
     * Format matches data for API response
     */
    public class MatchResponseFormatService : IMatchResponseFormatService
    {
        public List<MatchResourceModel> Format(List<Match> matches)
        {
            List<MatchResourceModel> formattedResult = new List<MatchResourceModel>();

            foreach(Match match in matches)
            {
                (Dictionary<string, List<PlayerResourceModel>>, List<GoalScorerResourceModel>) playersAndScorers = GetPlayerDetails(match.HostTeamId, match.GuestTeamId, match.MatchPlayers);
                MatchResourceModel formattedMatch = new MatchResourceModel()
                {
                    Id = match.Id,
                    Place = match.Place,
                    Date = match.Date.Date,
                    HostId = match.HostTeamId,
                    HostName = match.HostTeam.Name,
                    GuestId = match.GuestTeamId,
                    GuestName = match.GuestTeam.Name,
                    Result = String.Format("{0} : {1}", match.HostScore, match.GuestScore),
                    HostTeam = playersAndScorers.Item1["host"],
                    GuestTeam = playersAndScorers.Item1["guest"],
                    GoalScorers = playersAndScorers.Item2
                };
                formattedResult.Add(formattedMatch);
            }

            return formattedResult;
        }
        /**
         * Get match players and scorers
         */
        private (Dictionary<string, List<PlayerResourceModel>>, List<GoalScorerResourceModel>) GetPlayerDetails(int hostId, int guestId, List<MatchPlayer> matchPlayers)
        {
            List<PlayerResourceModel> hostPlayers = new List<PlayerResourceModel>();
            List<PlayerResourceModel> guestPlayers = new List<PlayerResourceModel>();
            List<GoalScorerResourceModel> goalScorers = new List<GoalScorerResourceModel>();

            Dictionary<string, List<PlayerResourceModel>> players = new Dictionary<string, List<PlayerResourceModel>>()
            {
                { "host", hostPlayers },
                {"guest", guestPlayers }
            };

            foreach(MatchPlayer matchPlayer in matchPlayers)
            {
                PlayerResourceModel player = new PlayerResourceModel()
                {
                    Id = matchPlayer.PlayerId,
                    Name = matchPlayer.Player.Name
                };
                // Host player
                if(matchPlayer.Player.TeamId == hostId)
                {
                    players["host"].Add(player);
                }
                // Guest player
                else if (matchPlayer.Player.TeamId == guestId)
                {
                    players["guest"].Add(player);
                }
                // Player scored
                if(matchPlayer.Goals.Count() != 0)
                {
                    GoalScorerResourceModel scorer = new GoalScorerResourceModel()
                    {
                        PlayerId = matchPlayer.PlayerId,
                        PlayerName = matchPlayer.Player.Name,
                        GoalsScored = matchPlayer.Goals.Count()
                    };
                    goalScorers.Add(scorer);
                }
            }
            return (players, goalScorers);
        }
    }
}
