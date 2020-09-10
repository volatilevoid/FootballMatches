using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballMatches.Models
{
    public class StatusAction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StatusId { get; set; }
        public Status Status { get; set; }
        public int NewStatusId { get; set; }
        public Status NewStatus { get; set; }
    }
}
