/*  
 Licensed under the MIT License. See LICENSE file in the project root for full license information.  
*/
using ServerTrack.Models;
using ServerTrack.Models.Interfaces;

namespace ServerTrack.Data.Record
{
    /// <summary>
    /// Stores the information for a specific server
    /// </summary>
    public class ServerData
    {
        /// <summary>
        /// The name of the server. This serves as the unique id for a server
        /// </summary>
        public string ServerName { get; }

        /// <summary>
        /// The Day collection for the server
        /// </summary>
        private readonly DayContainer dayContainer = new DayContainer();

        /// <summary>
        /// Constructor for the server
        /// </summary>
        /// <param name="serverName"></param>
        public ServerData(string serverName)
        {
            ServerName = serverName;
        }

        /// <summary>
        /// Update the server with new data
        /// </summary>
        /// <param name="incomingRecordData"></param>
        public void Update(IIncomingRecordData incomingRecordData)
        {
            dayContainer.Update(incomingRecordData);
        }

        /// <summary>
        /// Get the results for the server
        /// </summary>
        /// <returns></returns>
        public IDisplayRecordData GetResults()
        {
            var minutes = dayContainer.GetMinutes();
            var hours = dayContainer.GetHours();

            //Reversing the lists so the newest items are first
            minutes.Item1.Reverse();
            minutes.Item2.Reverse();
            hours.Item1.Reverse();
            hours.Item2.Reverse();

            //Build the resulting data set
            return new DisplayRecordData()
            {
                ServerName = ServerName,
                MinuteResolutionLoads = new ResolutionLoads()
                {
                    CpuLoad = minutes.Item1,
                    RamLoad = minutes.Item2
                },
                HourResolutionLoads = new ResolutionLoads()
                {
                    CpuLoad = hours.Item1,
                    RamLoad = hours.Item2
                }
            };
        }

        #region Hashify

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj) => obj is ServerData && Equals((ServerData) obj);

        public bool Equals(ServerData obj) => obj != null && string.Equals(ServerName, obj.ServerName);

        /// <summary>
        /// Serves as the default hash function. 
        /// </summary>
        /// <returns>
        /// A hash code for the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode() => ServerName.GetHashCode();

        #endregion
    }
}