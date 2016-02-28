/*  
 Licensed under the MIT License. See LICENSE file in the project root for full license information.  
*/
using System.Net.Http.Headers;
using System.Web.Http;

namespace ServerTrack
{
    /*
        Assuming the API will evolve over time
    */
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

        }
    }
}
