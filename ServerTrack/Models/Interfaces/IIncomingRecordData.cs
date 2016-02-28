/*  
 Licensed under the MIT License. See LICENSE file in the project root for full license information.  
*/
using System;

namespace ServerTrack.Models.Interfaces
{
    /// <summary>
    /// Addtional information required on Incoming Data
    /// </summary>
    public interface IIncomingRecordData
    {
        /// <summary>
        /// The DateTime the data came in
        /// </summary>
        DateTime LoadedDateTime { get; }
        /// <summary>
        /// The Server Name
        /// </summary>
        string ServerName { get; }
        /// <summary>
        /// The CPU utilization percentage
        /// </summary>
        double CpuLoad { get; }
        /// <summary>
        /// The RAM utilization percentage
        /// </summary>
        double RamLoad { get; }
    }
}
