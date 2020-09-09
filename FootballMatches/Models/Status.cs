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
        public bool IsMatchChangeable { get; set; }
        public List<Match> Matches { get; set; }
    }
}
