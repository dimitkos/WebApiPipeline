using Filters.Filters;
using Filters.Models;
using Swashbuckle.Swagger.Annotations;
using System.Net;
using System.Web.Http;

namespace Filters.Controllers
{
    [RoutePrefix("models")]
    public class ModelsController : ApiController
    {
        [HttpPost, Route("object")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(string))]
        [ValidateModelState(BodyRequired = true)]    // METHOD 2: Use an Action Filter
        public IHttpActionResult Post([FromBody]ComplexTypeDto dto)
        {
            // METHOD 1: Inline model validation
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            return Ok("Posted data valid");
        }

    }
}