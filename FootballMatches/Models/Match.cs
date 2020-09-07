using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballMatches.Models
{
    /**
     * Football match
     */
    public class Match
    {
        // PK
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Place { get; set; }
        public int HomeScore { get; set; }
        public int GuestScore { get; set; }

        public int StatusId { get; set; }
        public Status Status { get; set; }

        public int HostTeamId { get; set; }
        public Team HostTeam { get; set; }
        public int GuestTeamId { get; set; }
        public Team GuestTeam { get; set; }

        public List<MatchPlayer> MatchPlayers { get; set; }
    }
}
