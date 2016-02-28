/*  
 Licensed under the MIT License. See LICENSE file in the project root for full license information.  
*/
using System;

namespace ServerTrack.Data.Queuer
{
    /// <summary>
    /// The static access to the RecordQueue
    /// </summary>
    public class RecordQueue
    {
        #region Singleton
        /// <summary>
        /// Lazy create singleton
        /// </summary>
        /// <remarks>This could be configured to be any other type; hard coded for simplicity</remarks>
        private static readonly Lazy<MemoryRecordQueue> InstanceHolder = new Lazy<MemoryRecordQueue>();


        /// <summary>
        /// Get the Instance of the day record
        /// </summary>
        /// <remarks>Not a huge fan of a singleton; but it's a clean interface instead of doing a global</remarks>
        public static IRecordQueue Instance => InstanceHolder.Value;

        #endregion
    }
}