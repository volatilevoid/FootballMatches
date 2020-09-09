using FootballMatches.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballMatches.ViewModels
{
    public class AllMatchesViewModel
    {
        public List<Match> Matches { get; set; }
        public List<Status> Statuses { get; set; }

    }
}
