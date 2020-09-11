using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballMatches.ResourceModels
{
    public class GoalScorerResourceModel
    {
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public int GoalsScored { get; set; }
    }
}
