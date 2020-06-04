using ExceptionsErrorHandling.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ExceptionsErrorHandling.Controllers
{
    [RoutePrefix("values")]
    public class ValuesController : ApiController
    {
        // GET api/<controller>
        [HttpGet, Route("")]
        public IEnumerable<string> Get()
        {
            throw new InvalidOperationException();
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet, Route("{id:int}")]
        [InvalidAccountIdExceptionFilter]
        public string Get(int id)
        {
            throw new ArgumentOutOfRangeException("id", "IDs must be in the range 1 to 50");
            return "value";
        }

        [HttpPost, Route("")]
        public void Post([FromBody]string value)
        {
            throw new NotImplementedException();
        }

        // PUT: api/Values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Values/5
        public void Delete(int id)
        {
        }
    }
}
