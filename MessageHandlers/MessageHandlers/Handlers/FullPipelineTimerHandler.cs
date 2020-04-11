using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Handlers
{
    /// <summary>
    /// Generic template for a DelegatingHandler
    /// </summary>
    /// <remarks>
    /// If you don't have any response processing to do for step 3, you can replace the entire
    /// block of steps 2 through 4 with a single <code>return base.SendAsync(request, cancellationToken);</code>
    /// and remove the async keyword from the method definition (since you don't need the 
    /// continuation behavior of await in that case).
    /// </remarks>
    public class FullPipelineTimerHandler : DelegatingHandler
    {
        const string _header = "X-API-Timer";

        //calculate the amount of time used by the rwquest pipeline and sending to the caller in http header
        protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // STEP 1: Global message-level logic that must be executed BEFORE the request
            //          is sent on to the action method

            var timer = Stopwatch.StartNew();

            // STEP 2: Call the rest of the pipeline, all the way to a response message
            var response = await base.SendAsync(request, cancellationToken);

            // STEP 3: Any global message-level logic that must be executed AFTER the request
            //          has executed, before the final HTTP response message

            var elapsed = timer.ElapsedMilliseconds;
            Trace.WriteLine("Pipeline + Action timer :" + elapsed);

            response.Headers.Add(_header,elapsed + "ms"); 

            // STEP 4:  Return the final HTTP response
            return response;

        }
    }
}