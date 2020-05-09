using System.Collections.Generic;
using System.Web.Http;

namespace Authentication.Controllers
{
    [RoutePrefix("values")]
    public class ValuesController : ApiController
    {
        // GET: api/Values
        [HttpGet, Route("")]
        [Authorize]
        public IEnumerable<string> Get()
        {
            return new string[] { User.Identity.Name, User.Identity.AuthenticationType };
        }

        // GET: api/Values/5
        [HttpGet, Route("{id:int}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Values
        public void Post([FromBody]string value)
        {
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
