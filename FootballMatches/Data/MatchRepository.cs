using FootballMatches.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballMatches.Data
{
    public interface IMatchRepository
    {
        // Get all matches
        List<Match> Matches();
        // Add new Match
        void Add(Match match);
        // Get match details
        Match Get(int id);
        // Get all match statuses
        public List<Status> Statuses();
        // Status on match creation
        public Status DefaultStatus();
        // Add selected team players to the match
        public void AddMatchPlayer(MatchPlayer playerOnMatch);
        // Update match state
        void Update(Match match);
        // Get all available teams for provided match date
        public List<Team> AvailableTeams(DateTime matchDate);
        // Get all team players available for match
        public List<Player> AvailablePlayers(int teamId);
        //public MatchPlayer GetMatchPlayer(int matchId, int playerId);
        void Save();
    }
    public class MatchRepository : IMatchRepository
    {
        private readonly ApplicationDbContext _context;

        public MatchRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Match> Matches()
        {
            return _context.Matches
                .Include(match => match.Status)
                .Include(match => match.HostTeam)
                .Include(match => match.GuestTeam)
                .Include(match => match.MatchPlayers)
                .ToList();
        }
        public List<Status> Statuses() 
        {
            return _context.Statuses
                .Include(s => s.PermittedActions)
                .ToList();
        }
        public Status DefaultStatus()
        {
            return _context.Statuses
                .Where(s => s.Default == true)
                .FirstOrDefault();
        }
        public void Add(Match match)
        {
            _context.Matches.Add(match);
        }

        public void Update(Match match) 
        {
            _context.Update(match);
        }

        public Match Get(int id)
        {
            return _context.Matches
                .Where(match => match.Id == id)
                .Include(match => match.Status)
                    .ThenInclude(s => s.PermittedActions)
                .Include(match => match.HostTeam)
                    .ThenInclude(ht => ht.Players)
                        .ThenInclude(p => p.MatchPlayers)
                            .ThenInclude(mp => mp.Goals)
                .Include(match => match.GuestTeam)
                    .ThenInclude(gt => gt.Players)
                        .ThenInclude(p => p.MatchPlayers)
                            .ThenInclude(mp => mp.Goals)
                .FirstOrDefault();
        }

        /**
         * Teams with no other fixtures on matchDate
         */
        public List<Team> AvailableTeams(DateTime matchDate)
        {
            /**
             * Not efficient solution, but it was best I can find so far.
             */
            var test = matchDate.Date;
            var homeTeamsWithFixtures = _context.Matches
                .Where(m => DateTime.Compare(m.Date.Date, matchDate.Date) == 0 && !m.Status.AreTeamsAvailable)
                .Select(m => m.HostTeam);
            var teamsWithFixtures = _context.Matches
                .Where(m => DateTime.Compare(m.Date.Date, matchDate.Date) == 0 && !m.Status.AreTeamsAvailable)
                .Select(m => m.GuestTeam)
                .Union(homeTeamsWithFixtures)
                .OrderBy(t => t.Name)
                .ToList();
            var allTeams = _context.Teams.OrderBy(t => t.Name).ToList();
            List<Team> availableTeams = new List<Team>();
            foreach(Team team in allTeams)
            {
                if (!teamsWithFixtures.Contains(team))
                {
                    availableTeams.Add(team);
                }
            }
            return availableTeams;
                
        }
        public List<Player> AvailablePlayers(int teamId)
        {
            return _context.Players
                .Where(p => p.TeamId == teamId)
                .ToList();
        }
        public void AddMatchPlayer(MatchPlayer playerOnMatch)
        {
            _context.MatchPlayers.Add(playerOnMatch);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
