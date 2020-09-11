using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballMatches.Data;
using FootballMatches.Models;
using FootballMatches.ResourceModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FootballMatches.API
{
    public class FootballApiController : ControllerBase
    {
        private readonly IWebApiRepository _webApiRepository;
        private readonly IMatchResponseFormatService _matchFormatter;

        public FootballApiController(IWebApiRepository webApiRepository, IMatchResponseFormatService matchResponseFormatter)
        {
            _webApiRepository = webApiRepository;
            _matchFormatter = matchResponseFormatter;
        }

        [HttpGet]
        public List<MatchResourceModel> Get()
        {
            List<Match> matches = _webApiRepository.Matches();

            List<MatchResourceModel> resource = _matchFormatter.Format(matches);

            return resource;
        }
    }
}
