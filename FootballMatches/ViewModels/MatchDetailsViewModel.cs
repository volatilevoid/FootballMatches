using System;
using System.Collections.Generic;
using FootballMatches.Models;

namespace FootballMatches.ViewModels
{
    public class MatchDetailsViewModel
    {
        public int Id { get; set; }
        public int HostTeamId { get; set; }
        public string HostTeamName { get; set; }
        public int GuestTeamId { get; set; }
        public string GuestTeamName { get; set; }
        public int HostScore { get; set; }
        public int GuestScore { get; set; }
        public string Place { get; set; }
        public DateTime Date { get; set; }
        public Status Status { get; set; }
        public List<Player> HostSquad { get; set; }
        public List<Player> GuestSquad { get; set; }
    }
}
