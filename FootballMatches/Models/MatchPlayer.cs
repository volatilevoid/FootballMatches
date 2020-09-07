using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballMatches.Models
{
    /**
     * Players in match
     * 
     * Entity class represnting join table
     */
    public class MatchPlayer
    {
        public int MatchId { get; set; }
        public Match Match { get; set; }
        public int PlayerId { get; set; }
        public Player Player { get; set; }
        public List<Goal> Goals { get; set; }
    }
}
