using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballMatches.Models
{
    /**
     * Football player
     * 
     * IsActive flag - ability to soft delete in order to preserve statistics for retired players
     */
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public List<MatchPlayer> MatchPlayers { get; set; }

    }
}
