using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace WebApi.Controllers
{
    public class StatsController : ApiController
    {
        // GET api/stats
        public Response Get()
        {
            Response response = new Response();

            response.count_human_dna = 100;
            response.count_mutant_dna = 40;
            response.ratio = response.count_mutant_dna / response.count_human_dna;
            return response;
            
        }
    }

    public class Response
    {
        public int count_mutant_dna;
        public int count_human_dna;
        public double ratio;
}
}