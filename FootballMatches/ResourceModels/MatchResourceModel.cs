using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballMatches.ResourceModels
{
    public class MatchResourceModel
    {
        public int Id { get; set; }
        public string Place { get; set; }
        public DateTime Date { get; set; }
        public int HostId { get; set; }
        public string HostName { get; set; }
        public int GuestId { get; set; }
        public string GuestName { get; set; }
        public string Result { get; set; }
        public List<PlayerResourceModel> HostTeam { get; set; }
        public List<PlayerResourceModel> GuestTeam { get; set; }
        public List<GoalScorerResourceModel> GoalScorers { get; set; }
    }
}
