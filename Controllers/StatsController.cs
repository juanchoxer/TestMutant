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

            response = new Database().GetStats();

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
