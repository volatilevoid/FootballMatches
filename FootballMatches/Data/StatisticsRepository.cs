using FootballMatches.Models;
using FootballMatches.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballMatches.Data
{
    public interface IStatisticsRepository
    {
        public List<Team> Teams();
        public List<Player> Players();
    }
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly ApplicationDbContext _context;

        public StatisticsRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Team> Teams()
        {
            return _context.Teams
                .Include(t => t.HostMatches)
                    .ThenInclude(hm => hm.Status)
                .Include(t => t.GuestMatches)
                    .ThenInclude(gm => gm.Status)
                .ToList();
        }
        public List<Player> Players()
        {
            return _context.Players
                .Include(p => p.Team)
                .Include(p => p.MatchPlayers)
                    .ThenInclude(mp => mp.Goals)
                .ToList();
        }
    }
}
