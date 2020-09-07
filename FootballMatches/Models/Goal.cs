using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballMatches.Models
{
    /**
     * Goal scored by player on the match
     */
    public class Goal
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int MatchId { get; set; }
        public MatchPlayer MatchPlayer { get; set; }
    }
}
