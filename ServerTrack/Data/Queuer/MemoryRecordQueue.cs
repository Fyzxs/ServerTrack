/*  
 Licensed under the MIT License. See LICENSE file in the project root for full license information.  
*/
using log4net;
using ServerTrack.Data.Record;
using ServerTrack.Log;
using ServerTrack.Models.Interfaces;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace ServerTrack.Data.Queuer
{
    /// <summary>
    /// Queues Records in Memory
    /// </summary>
    public class MemoryRecordQueue : IRecordQueue
    {
        /// <summary>
        /// Collection of consumers
        /// </summary>
        private Task[] consumerWorkers;

        /// <summary>
        /// Default constructor; uses default number of consumers
        /// </summary>
        public MemoryRecordQueue()
        {
            int numberOfConsumers;
            if (!int.TryParse(WebConfigurationManager.AppSettings["MemoryRecordQueueConsumerCount"], out numberOfConsumers))
            {
                numberOfConsumers = 1;
            }
            InitializeConsumers(numberOfConsumers);
        }

        /// <summary>
        /// Constructor with settable number of consumers
        /// </summary>
        /// <param name="numberOfConsumers"></param>
        public MemoryRecordQueue(int numberOfConsumers)
        {
            InitializeConsumers(numberOfConsumers);
        }

        /// <summary>
        /// Initialize the collection of consumers
        /// </summary>
        /// <param name="numberOfConsumers">The number of consumers to create</param>
        private void InitializeConsumers(int numberOfConsumers)
        {
            consumerWorkers = new Task[numberOfConsumers];

            for (var i = 0; i < consumerWorkers.Length; i++)
            {
                consumerWorkers[i] = Task.Factory.StartNew(ConsumerWorker);
            }
        }

        /// <summary>
        /// Logger for the class
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(MemoryRecordQueue));
        
        /// <summary>
        /// The BlockingQueue to support the Producer/Consumer pattern.
        /// </summary>
        /// <remarks>This is implementing a Producer/Consumer model to ensure the Record POST requests are not
        /// unduly delayed or large amounts of threads consumed.</remarks>
        private static readonly BlockingCollection<IIncomingRecordData> QueuedRecords = new BlockingCollection<IIncomingRecordData>();

        /// <summary>
        /// <see cref="IRecordQueue.AddRecordData"/>
        /// </summary>
        public void AddRecordData(IIncomingRecordData data)
        {
            if (data == null)
            {
                const string msg = "RecordData cannot be null.";
                Log.Error(() => msg);
                
                return;//We'll play nice and not crash the service
            }
            QueuedRecords.Add(data);
        }

        /// <summary>
        /// <see cref="IRecordQueue.RemoveData"/>
        /// </summary>
        public void RemoveData(string serverName) => ServerCollection.Instance.RemoveDisplayData(serverName);

        /// <summary>
        /// Get the size of the queue. This won't be 100% accurate; but provides an estimation
        /// </summary>
        /// <returns>Size of the Queue</returns>
        /// <remarks>Mostly for unit testing</remarks>
        public int QueueSize() =>  QueuedRecords.Count;

        /// <summary>
        /// Process Record Data
        /// </summary>
        /// <returns>A record to process</returns>
        /// <remarks>The GetConsumingEnumerable will block until an item is available to be removed.
        /// Which means that the queue worker may block if it manages to outpace the incoming data.
        /// This should be OK; just something to be aware of</remarks>
        private static void ConsumerWorker()
        {
            foreach (var recordData in QueuedRecords.GetConsumingEnumerable())
            {
                ServerCollection.Instance.AddRecordLoad(recordData);
            }
        }

    }
}