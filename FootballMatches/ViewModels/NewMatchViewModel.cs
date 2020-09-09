using FootballMatches.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballMatches.ViewModels
{
    public class NewMatchViewModel
    {
        public List<Status> statuses { get; set; }
        public List<Team> availableTeams { get; set; }
    }
}
