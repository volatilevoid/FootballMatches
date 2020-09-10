using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballMatches.Models
{
    /**
     * Match status
     */
    public class Status
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public bool IsMatchStateChangeable { get; set; }
        // In case of canceled match teams are free to arrange play other match on same date
        public bool AreTeamsAvailable { get; set; }
        public bool Default { get; set; }
        public List<Match> Matches { get; set; }
        public List<StatusAction> PermittedActions { get; set; }
    }
}
