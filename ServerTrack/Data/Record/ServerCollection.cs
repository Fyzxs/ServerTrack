/*  
 Licensed under the MIT License. See LICENSE file in the project root for full license information.  
*/
using System;
using System.Collections;
using log4net;
using ServerTrack.Log;
using ServerTrack.Models.Interfaces;

namespace ServerTrack.Data.Record
{
    /// <summary>
    /// Collection of servers.
    /// This is the interface for updating and getting the display for a given server.
    /// </summary>
    public class ServerCollection
    {
        /// <summary>
        /// Logger for the class
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(ServerCollection));

        #region Singleton
        /// <summary>
        /// Lazy create singleton
        /// </summary>
        private static readonly Lazy<ServerCollection> InstanceHolder = new Lazy<ServerCollection>();

        /// <summary>
        /// Settable instance of the servercollection singleton
        /// </summary>
        private static ServerCollection instanceHolder;
        /// <summary>
        /// Mostly unit test setter to customize the servercollection
        /// </summary>
        /// <param name="newInstance"></param>
        public static void SetServerCollection(ServerCollection newInstance)
        {
            instanceHolder = newInstance;
        }

        /// <summary>
        /// Get the Instance of the day record
        /// </summary>
        /// <remarks>Not a huge fan of a singleton; but it's a clean interface instead of doing a global</remarks>
        public static ServerCollection Instance => instanceHolder ?? InstanceHolder.Value;

        #endregion

        /// <summary>
        /// The collection
        /// </summary>
        private Hashtable ServerSet { get; } = new Hashtable();

        /// <summary>
        /// This adds a RecordLoad data set into the correct server
        /// </summary>
        /// <param name="incomingRecordData"></param>
        public virtual void AddRecordLoad(IIncomingRecordData incomingRecordData)
        {
            if (string.IsNullOrEmpty(incomingRecordData?.ServerName))
            {
                Log.Error(() => "Invalid incomingRecordData");
                return;
            }
            var serverData = ServerSet[incomingRecordData.ServerName] as ServerData;
            if (serverData == null)
            {
                serverData = new ServerData(incomingRecordData.ServerName);
                
                ServerSet.Add(incomingRecordData.ServerName, serverData);
            }

            serverData.Update(incomingRecordData);
        }

        /// <summary>
        /// Get's the data load information
        /// </summary>
        /// <param name="serverName">Server to get the information for</param>
        /// <returns>Set of information to be returned</returns>
        public virtual IDisplayRecordData GetDisplayData(string serverName)
        {
            return (ServerSet[serverName] as ServerData)?.GetResults();
        }

        /// <summary>
        /// Removes the specified server from the collection
        /// </summary>
        /// <param name="serverName"></param>
        public virtual void RemoveDisplayData(string serverName) => ServerSet.Remove(serverName);
    }
}