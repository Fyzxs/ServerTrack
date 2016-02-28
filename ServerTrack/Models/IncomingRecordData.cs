/*  
 Licensed under the MIT License. See LICENSE file in the project root for full license information.  
*/
using System;
using ServerTrack.Models.Interfaces;

namespace ServerTrack.Models
{
    /// <summary>
    /// Default implementation for Incoming Record Data
    /// </summary>
    public class IncomingRecordData : IIncomingRecordData
    {
        /// <summary>
        /// <see cref="IIncomingRecordData.LoadedDateTime"/>
        /// </summary>
        public DateTime LoadedDateTime { get; } = DateTime.Now;

        /// <summary>
        /// <see cref="IIncomingRecordData.ServerName"/>
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// <see cref="IIncomingRecordData.CpuLoad"/>
        /// </summary>
        public double CpuLoad { get; set; }
        /// <summary>
        /// <see cref="IIncomingRecordData.RamLoad"/>
        /// </summary>
        public double RamLoad { get; set; }
    }
}