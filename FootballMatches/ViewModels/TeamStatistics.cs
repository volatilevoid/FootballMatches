using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballMatches.ViewModels
{
    /**
     * Team statistics view model
     * WinDrawLossViewModel[0] - Home matches
     * WinDrawLossViewModel[1] - Away matches
     * WinDrawLossViewModel[2] - Total matches
     */
    public class TeamStatistics
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public WinDrawLossViewModel[] Statistics { get; set; }
    }
}
