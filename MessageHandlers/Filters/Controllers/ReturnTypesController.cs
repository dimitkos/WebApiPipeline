using Filters.ActionResults;
using Filters.Filters;
using Filters.Models;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Filters.Controllers
{
    [RoutePrefix("returntypes")]
    [ClientCacheControlFilter(ClientCacheControl.NoCache,10)]
    public class ReturnTypesController : ApiController
    {        
        /// <summary>
        /// Returns an HttpResponseMessage
        /// </summary>
        [HttpGet, Route("httpresponse")]
        [ResponseType(typeof(ComplexTypeDto))]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ComplexTypeDto))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(HttpError))]
        public HttpResponseMessage GetAnHttpResponse()
        {
            var dto = new ComplexTypeDto()
            {
                String1 = "This is string 1",
                String2 = "This is string 2",
                Int1 = 55,
                Date1 = DateTime.Now
            };

            //var response = new HttpResponseMessage(HttpStatusCode.OK)
            //{
            //    // note I am responsible for my own content negotiation!
            //    // this content will confuse a caller wanting XML or other media type
            //    Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json")
            //};

            // or alternatively
            var response = Request.CreateResponse(dto);

            response.Headers.Add("X-MyCustomHeader", "MyHeaderValue");
            response.ReasonPhrase = "Most Excellent!";

            // error response
            response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid something or other");

            return response;
        }

        /// <summary>
        /// Returns an IHttpActionResult
        /// </summary>
        [HttpGet, Route("actionresult")]
        [ResponseType(typeof(ComplexTypeDto))]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ComplexTypeDto))]
        public IHttpActionResult GetAnActionResult()
        {
            var dto = new ComplexTypeDto()
            {
                String1 = "This is string 1",
                String2 = "This is string 2",
                Int1 = 55,
                Date1 = DateTime.Now
            };

            return Ok(dto).AddHeader("X-MyCustomHeader", "test value");
        }

    }
}