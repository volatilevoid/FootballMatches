using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballMatches.Models
{
    public class Team
    {
        /**
         * Football team
         */
        public int Id { get; set; }
        public string Name { get; set; }
        // Note: byte[] format is apropriate for storing images in SQLite
        public byte[] Logo { get; set; }
        public string Description { get; set; }
        public List<Player> Players { get; set; }
        public List<Match> HostMatches { get; set; }
        public List<Match> GuestMatches { get; set; }
    }
}
