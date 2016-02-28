/*  
 Licensed under the MIT License. See LICENSE file in the project root for full license information.  
*/
using System.Collections.Generic;

namespace ServerTrack.Models
{
    /// <summary>
    /// Object to hold the lists for a given resolution
    /// </summary>
    public class ResolutionLoads
    {
        /// <summary>
        /// The CPU utilization percentage
        /// </summary>
        public List<double> CpuLoad { get; set; }

        /// <summary>
        /// The RAM utilization percentage
        /// </summary>
        public List<double> RamLoad { get; set; }
    }
}
