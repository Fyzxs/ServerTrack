/*  
 Licensed under the MIT License. See LICENSE file in the project root for full license information.  
*/
using ServerTrack.Models.Interfaces;

namespace ServerTrack.Data.Queuer
{
    /// <summary>
    /// Interface for the Record Queueing
    /// </summary>
    public interface IRecordQueue
    {
        /// <summary>
        /// Adds a new Record
        /// </summary>
        /// <param name="data"></param>
        void AddRecordData(IIncomingRecordData data);

        /// <summary>
        /// Removes the server from the list
        /// </summary>
        /// <param name="serverName">The server data to remvoe</param>
        void RemoveData(string serverName);

        /// <summary>
        /// Get the size of the queue. This won't be 100% accurate; but provides an estimation
        /// </summary>
        /// <returns>Size of the Queue</returns>
        /// <remarks>Mostly for unit testing</remarks>
        int QueueSize();
    }
}