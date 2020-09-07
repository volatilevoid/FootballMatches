using FootballMatches.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballMatches.Data
{
    public interface ITeamRepository
    {
        List<Team> All();
        void Add(Team team);
        Team Get(int teamId);
        //void Update(Team team);
        void AddPlayer(Player player);
        void RemovePlayer(Player player);
        void Save();
    }
    public class TeamRepository : ITeamRepository
    {
        private readonly ApplicationDbContext _context;

        public TeamRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        /**
         * Load all teams with related players
         */
        public List<Team> All()
        {
            return _context.Teams
                .Include(team => team.Players)
                .ToList();
        }
        /**
         * Create new team
         */
        public void Add(Team team)
        {
            _context.Teams.Add(team);
        }
        /**
         * Load Team with related players
         */
        public Team Get(int teamId)
        {
            return _context.Teams
                .Where(team => team.Id == teamId)
                .Include(team => team.Players)
                .FirstOrDefault();
        }
        /**
         * Add new player to team
         */
        public void AddPlayer(Player player)
        {
            var team = _context.Teams
                .Find(player.TeamId);

            team.Players.Add(player);
        }
        /*
         * Remove player from team
         */
        public void RemovePlayer(Player player)
        {
            var team = _context.Teams. Find(player.Id);
            team.Players.Remove(player);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
