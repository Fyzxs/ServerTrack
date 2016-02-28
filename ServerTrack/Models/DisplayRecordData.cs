/*  
 Licensed under the MIT License. See LICENSE file in the project root for full license information.  
*/
using ServerTrack.Models.Interfaces;

namespace ServerTrack.Models
{
    /// <summary>
    /// A concrete instance of the Display Record Data
    /// </summary>
    public class DisplayRecordData : IDisplayRecordData
    {
        /// <summary>
        /// The Server Name
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// The averages per minute
        /// </summary>
        public ResolutionLoads MinuteResolutionLoads { get; set; }
        /// <summary>
        /// The averages per hour
        /// </summary>
        public ResolutionLoads HourResolutionLoads { get; set; }
       
    }
}