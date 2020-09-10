using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballMatches.ViewModels
{
    public class PlayerStatistics
    {
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public string PlayerTeam { get; set; }
        public int MatchesPlayed { get; set; }
        public int GoalsScored { get; set; }
    }
}
