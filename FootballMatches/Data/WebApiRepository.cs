using FootballMatches.Models;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballMatches.Data
{
    public interface IWebApiRepository
    {
        public List<Match> Matches();
    }

    public class WebApiRepository : IWebApiRepository
    {
        private readonly ApplicationDbContext _context;

        public WebApiRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Match> Matches()
        {
            return _context.Matches
                .Include(m => m.Status)
                .Include(m => m.HostTeam)
                    .ThenInclude(ht => ht.Players)
                        .ThenInclude(p => p.MatchPlayers)
                            .ThenInclude(mp => mp.Goals)
                .Include(m => m.GuestTeam)               
                    .ThenInclude(gt => gt.Players)
                        .ThenInclude(p => p.MatchPlayers)
                            .ThenInclude(mp => mp.Goals)
                .ToList();
        }
    }
}
