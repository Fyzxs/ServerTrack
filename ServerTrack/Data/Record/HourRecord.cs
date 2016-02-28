/*  
 Licensed under the MIT License. See LICENSE file in the project root for full license information.  
*/
using System;
using System.Collections.Generic;
using log4net;
using ServerTrack.Log;
using ServerTrack.Models.Interfaces;

namespace ServerTrack.Data.Record
{
    /// <summary>
    /// This class provides a container to track the data over the span of an hour
    /// </summary>
    public class HourRecord : TimeRecord
    {
        /// <summary>
        /// Logger for the class
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(HourRecord));
        /// <summary>
        /// Logger for the class
        /// </summary>
        private readonly List<MinuteRecord> minutes = new List<MinuteRecord>(60);
        
        /// <summary>
        /// Cosntructor for an Hour Record
        /// </summary>
        /// <param name="identifier">The identifier representing the hour the record represents</param>
        public HourRecord(int identifier) : base(0, 23, identifier)
        {
            /*
                Adding an initial record to simplify the Update Code
                The Update method should be as minimal as can be done as 
                it will be executed HEAVILY.
            */
            minutes.Add(new MinuteRecord(DateTime.Now.Minute));
        }

        /// <summary>
        /// Updates the information tracking the current record
        /// </summary>
        /// <param name="recordData">The data for the record</param>
        public override void Update(IIncomingRecordData recordData)
        {
            //Minutes go first because if there's gaps; it needs to 
            //add empty records to base
            UpdateMinute(recordData);
            base.Update(recordData);//Updates the hour average information
        }

        /// <summary>
        /// Update the Minutes
        /// </summary>
        /// <param name="recordData">The Data</param>
        private void UpdateMinute(IIncomingRecordData recordData)
        {
            //Get the minute the data came in
            var dataMinuteTime = recordData.LoadedDateTime.Minute;
            var lastMinute = minutes[minutes.Count - 1];
            //Ctor adds a MinuteRecord; so this won't be empty
            var lastMinuteTime = lastMinute.Identifier;

            if (lastMinuteTime != dataMinuteTime)
            {
                /*
                    For a non-busy machine; or one that got restarted there may have been a period of 
                    time that the data wasn't flowing in - this corrects for it
                */
                for (var missingMinute = lastMinuteTime; missingMinute < dataMinuteTime; missingMinute++)
                {
                    //This adds the missing time and values will be defaulted to 0 (zero)
                    minutes.Add(new MinuteRecord(missingMinute));
                    base.UpdateEmpty();
                }

                lastMinute = new MinuteRecord(dataMinuteTime);
                minutes.Add(lastMinute);

                //Fall through to apply the record data
            }

            //It's still the current minute; so we update
            lastMinute.Update(recordData);
        }

        /// <summary>
        /// Gets the list of Minute averages
        /// </summary>
        /// <returns></returns>
        public Tuple<List<double>, List<double>> GetMinuteList()
        {
            if (!IsCompletedAdding)
            {
                var cpu = new List<double>();
                var ram = new List<double>();
                for(var i = 0; i < minutes.Count; i++)
                {
                    var minuteRecord = minutes[i];
                    cpu.Add(minuteRecord.GetCpuLoad());
                    ram.Add(minuteRecord.GetRamLoad());

                    //This isn't susceptible to hours; the hour starts new.
                    if (i != minutes.Count - 1 || minuteRecord.Identifier >= DateTime.Now.Minute) continue;
                    var dif = DateTime.Now.Minute - minuteRecord.Identifier;
                    for (var m = 0; m < dif; m++)
                    {
                        cpu.Add(0);
                        ram.Add(0);
                    }
                }
                return Tuple.Create(cpu, ram);
            }

            //We've completed; minute list is no longer valid
            const string msg = "Attempted to get MinuteList after CompletedAdding";
            Log.Error(() => msg);
            throw new NotSupportedException(msg);
        }
    }
}