/*  
 Licensed under the MIT License. See LICENSE file in the project root for full license information.  
*/
using System;
using log4net;
using ServerTrack.Log;
using ServerTrack.Models.Interfaces;

namespace ServerTrack.Data.Record
{
    /// <summary>
    /// Base class for time bases records.
    /// </summary>
    public abstract class TimeRecord
    {
        /// <summary>
        /// Logger for the class
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(TimeRecord));

        /// <summary>
        /// Average Calculator for the CPU utilization
        /// </summary>
        private readonly AverageCalc cpuAverageCalc = new AverageCalc();
        /// <summary>
        /// Average Calculator for the RAM utilization
        /// </summary>
        private readonly AverageCalc ramAverageCalc = new AverageCalc();

        /// <summary>
        /// Flag for when adding new values is completed
        /// </summary>
        protected bool IsCompletedAdding;
        

        /// <summary>
        /// This provides an identifier for each record. There's no enforcement of uniqueness
        /// </summary>
        internal readonly int Identifier;

        /// <summary>
        /// Creates a record and verifies the identifier is in the implementors expected range
        /// </summary>
        /// <param name="minIdentifier"></param>
        /// <param name="maxIdentifier"></param>
        /// <param name="identifier"></param>
        protected TimeRecord(int minIdentifier, int maxIdentifier, int identifier)
        {
            if (identifier < minIdentifier || identifier > maxIdentifier)
            {
                /*
                    Coming from a few years on mobile; I'm going to call out my use of `GetType().Name`. 
                    This is server code which will not be getting obfuscated; so utilizing the name in this fashion will work.
                */
                var msg = string.Format($"{GetType().Name} requries an identifier inclusive between {minIdentifier} and {maxIdentifier}");
                Log.Debug(() => msg);
                throw new ArgumentException(msg);
            }
            Identifier = identifier;
        }

        /// <summary>
        /// Marks the <see cref="TimeRecord"/> instance as not accepting any more additions.
        /// </summary>
        /// <remarks> It may be noticed that there is no locking around the <see cref="IsCompletedAdding"/> being set. There is a small
        /// potential that some values may get stuffed in after the method is called; or not included into the final
        /// <see cref="IDisplayRecordData"/> stored as <see cref="CompletedDisplayRecordData"/>. Given the expected high volume of values; a small number
        /// will have minimal impact.
        /// The largest factor to the decision to not use locking is - Locking is expensive. That cost can be removed with insignificant impact on the data.
        /// 
        /// The small expectation is due to that there should only be a single thread adding elements; and that thread should be doing all
        /// the calling to both <see cref="Update"/> and <see cref="CompleteAdding"/>; the expected single threading prevents collisions
        /// on that front.
        /// </remarks>
        public void CompleteAdding()
        {
            IsCompletedAdding = true;
        }

        /// <summary>
        /// Updates the information tracking the current record
        /// </summary>
        /// <param name="recordData">The data for the record</param>
        public virtual void Update(IIncomingRecordData recordData)
        {
            if (IsCompletedAdding)
            {
                Log.Info(() => "Attempted to add after CompletedAdding");
                return;
            }
            cpuAverageCalc.Update(recordData.CpuLoad);
            ramAverageCalc.Update(recordData.RamLoad);
        }

        public void UpdateEmpty()
        {
            if (IsCompletedAdding)
            {
                Log.Info(() => "Attempted to update empty after CompletedAdding");
                return;
            }
            cpuAverageCalc.Update(0);
            ramAverageCalc.Update(0);
        }

        /// <summary>
        /// Get the result set of the data for the cpu 
        /// </summary>
        /// <returns>CPU average</returns>
        public double GetCpuLoad()
        {
            return Math.Round(cpuAverageCalc.Average, 2);
        }
        /// <summary>
        /// Get the result set of the data for the ram 
        /// </summary>
        /// <returns>RAM average</returns>
        public double GetRamLoad()
        {
            return Math.Round(ramAverageCalc.Average, 2);
        }
    }
}