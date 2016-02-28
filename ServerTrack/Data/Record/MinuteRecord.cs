/*  
 Licensed under the MIT License. See LICENSE file in the project root for full license information.  
*/
namespace ServerTrack.Data.Record
{
    /// <summary>
    /// This class provides a container to track the data over the span of a minute
    /// </summary>
    public class MinuteRecord : TimeRecord
    {
        /// <summary>
        /// Constructor for the MinuteRecord
        /// </summary>
        /// <param name="identifier">The Minute that this object will represent</param>
        public MinuteRecord(int identifier) : base(0, 59, identifier){}
    }

}