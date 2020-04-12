using Filters.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Http;

namespace Filters.Controllers
{
    [RoutePrefix("values")]
    public class ValuesController : ApiController
    {
        // GET <controller>
        [HttpGet, Route("")]
        [ClientCacheControlFilter(ClientCacheControl.Private,10)]
        public IEnumerable<string> Get()
        {
            Trace.WriteLine(DateTime.Now.ToLongDateString()+ "GetCalled");
            return new string[] { "value1", "value2" };
        }

        // GET <controller>/5
        [HttpGet, Route("{id:int}")]
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
