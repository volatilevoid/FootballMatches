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
        List<Match> Matches();
        //void Add(Match match);
        Match Get(int id);
        List<Status> Statuses();
        Status DefaultStatus();
        void Update(Match match);
        void Save();
        public List<Team> AvailableTeams(DateTime matchDate);
        public Team Team(int teamId);
        public List<Player> AvailablePlayers(int teamId);
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
        public void Add(Match match) { }
        public List<Status> Statuses() 
        {
            return _context.Statuses
                .ToList();
        }
        public Status DefaultStatus()
        {
            return _context.Statuses
                .Where(s => s.Default == true)
                .FirstOrDefault();
        }
        public void Update(Match match) 
        {
            
        }

        public Match Get(int id)
        {
            return _context.Matches
                .Where(match => match.Id == id)
                .Include(match => match.Status)
                .Include(match => match.HostTeam)
                .Include(match => match.GuestTeam)
                .Include(match => match.MatchPlayers)
                .FirstOrDefault();
        }

        /**
         * Teams with 
         * - more than 6 players
         * - no other fixtures on matchDate
         */
        public List<Team> AvailableTeams(DateTime matchDate)
        {
            /**
             * Not efficient solution, but it was best I can find so far.
             */
            var test = matchDate.Date;
            var homeTeamsWithFixtures = _context.Matches
                .Where(m => DateTime.Compare(m.Time.Date, matchDate.Date) == 0 && !m.Status.AreTeamsAvailable)
                .Select(m => m.HostTeam);
            var teamsWithFixtures = _context.Matches
                .Where(m => DateTime.Compare(m.Time.Date, matchDate.Date) == 0 && !m.Status.AreTeamsAvailable)
                .Select(m => m.GuestTeam)
                .Union(homeTeamsWithFixtures)
                .OrderBy(t => t.Name)
                .ToList();
            var allTeams = _context.Teams.OrderBy(t => t.Name).ToList();
            List<Team> availableTeams = new List<Team>();
            foreach(Team team in allTeams)
            {
                //if (!teamsWithFixtures.Contains(team) && team.Players != null && team.Players.Count > 5)
                // DEV condition
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
        public Team Team(int teamId)
        {
           return _context.Teams
                .Include(t => t.Players)
                .Where(t => t.Id == teamId)
                .FirstOrDefault();
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
