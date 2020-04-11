using Handlers;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace MessageHandlers.Controllers
{
    [RoutePrefix("values")]
    public class ValuesController : ApiController
    {
        // GET <controller>
        [HttpGet, Route("")]
        public IEnumerable<string> Get()
        {
            var getByIdUrl = Url.Link("GetById", new { id = 123 });

            //return new string[] { "value1", "value2" };
            return new string[] {
                getByIdUrl,//original url
                Request.GetSelfReferenceBaseUrl().ToString(),//the client base url if i have headers x-forwarded-host:mycompany.com:1234, x-forwarded-proto :https
                Request.RebaseUrlForClient(new Uri(getByIdUrl)).ToString()//rebase url from client prespective
            };
        }

        // GET <controller>/5
        [HttpGet, Route("{id:int}", Name = "GetById")]
        public string Get(int id)
        {
            return "value";
        }

        // POST <controller>
        [HttpPost, Route("")]
        public void Post([FromBody]string value)
        {
        }

        // PUT <controller>/5
        [HttpPut, Route("{id:int}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE <controller>/5
        [HttpDelete, Route("{id:int}")]
        public void Delete(int id)
        {
        }
    }
}

