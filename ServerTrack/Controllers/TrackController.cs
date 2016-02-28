/*  
 Licensed under the MIT License. See LICENSE file in the project root for full license information.  
*/
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ServerTrack.Data.Queuer;
using ServerTrack.Data.Record;
using ServerTrack.Models;

namespace ServerTrack.Controllers
{
    public class TrackController : ApiController
    {

        /// <summary>
        /// This POST will take the information about a server load and track it
        /// for later averaging.
        /// </summary>
        /// <param name="serverName">The unique name of the server</param>
        /// <param name="cpuLoad">The CPU load</param>
        /// <param name="ramLoad">The RAM load</param>
        [Route("api/v1/record/{serverName}/cpu/{cpuLoad}/ram/{ramLoad}/")]
        public HttpResponseMessage GetRecord(string serverName, double cpuLoad, double ramLoad)
        {
            RecordQueue.Instance.AddRecordData(new IncomingRecordData() {ServerName = serverName, CpuLoad = cpuLoad , RamLoad = ramLoad});
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets list of the average load values for the last 60 minutes broken down by minute and the last 24 hours broken down by hour
        /// </summary>
        /// <param name="serverName">The unique server name to retrieve for</param>
        [Route("api/v1/display/{serverName}")]
        public HttpResponseMessage GetDisplay(string serverName)
        {
            var displayData = ServerCollection.Instance.GetDisplayData(serverName);
            return displayData == null ? Request.CreateResponse(HttpStatusCode.NoContent) : Request.CreateResponse(HttpStatusCode.OK, displayData);
        }

        /// <summary>
        /// Simple result to handle the default browser starting url
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public HttpResponseMessage Root()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "ServerTracker Is Active");
        }

        ///// <summary>
        ///// Providing a way to remove the server from the tracker.
        ///// </summary>
        ///// <param name="serverName">The unique name of the server</param>
        //[Route("api/v1/record/{serverName}")]
        //[HttpDelete]
        //public HttpResponseMessage RemoveRecord(string serverName)
        //{
        //    var response = new DisplayController().Get(serverName);
        //    ServerCollection.Instance.RemoveDisplayData(serverName);
        //    return response;
        //}
    }
}