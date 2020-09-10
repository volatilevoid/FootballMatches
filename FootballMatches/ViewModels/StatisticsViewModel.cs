using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballMatches.ViewModels
{
    public class StatisticsViewModel
    {
        public List<TeamStatistics> TeamStatistics { get; set; }
        public Dictionary<int, PlayerStatistics> PlayerStatistics { get; set; }
    }
}
