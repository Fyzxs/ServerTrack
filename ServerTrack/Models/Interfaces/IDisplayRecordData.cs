/*  
 Licensed under the MIT License. See LICENSE file in the project root for full license information.  
*/
namespace ServerTrack.Models.Interfaces
{
    /// <summary>
    /// The interface for the Displayed Record Data
    /// </summary>
    public interface IDisplayRecordData
    {
        /// <summary>
        /// The Server Name
        /// </summary>
        string ServerName { get; set; }

        /// <summary>
        /// The averages per minute
        /// </summary>
        ResolutionLoads MinuteResolutionLoads { get; set; }

        /// <summary>
        /// The averages per hour
        /// </summary>
        ResolutionLoads HourResolutionLoads { get; set; }
    }
}