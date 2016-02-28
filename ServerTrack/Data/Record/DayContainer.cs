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
    /// This class provides a container to track the data over the span of a day
    /// </summary>
    /// <remarks>This does not inherit from TimeRecord like HourRecord and MinuteRecord do as we don't return a list with day resolution.</remarks>
    public class DayContainer
    {
        /// <summary>
        /// Logger for the class
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(DayContainer));
        /// <summary>
        /// The last DateTime that got processed
        /// </summary>
        /// <remarks>This is used to ensure any downtime for a server doesn't result
        /// in incorrect data. The most likely instance would be downtime of 24 hours or more.</remarks>
        private DateTime lastDateTime = DateTime.Now;

        /// <summary>
        /// This allows easy access to the last item put onto the queue
        /// but continues to allow queue behavior
        /// </summary>
        private HourRecord lastHourRecord;

        /// <summary>
        /// The hours of the day
        /// </summary>
        private readonly Queue<HourRecord> hours = new Queue<HourRecord>(24);

        /// <summary>
        /// Constructor for holding a day record
        /// </summary>
        /// <remarks>Currently only 1 day of data is held; but it can be expanded</remarks>
        public DayContainer()
        {
            Initialize(DateTime.Now.Hour);
        }

        /// <summary>
        /// Encapsulates initialization of the class; or resetting.
        /// </summary>
        private void Initialize(int hour)
        {
            hours.Clear();
            lastHourRecord = new HourRecord(hour);
            hours.Enqueue(lastHourRecord);
        }

        #region Update
        /// <summary>
        /// Updates the information tracking the current record
        /// </summary>
        /// <param name="recordData">The data for the record</param>
        public void Update(IIncomingRecordData recordData)
        {
            /* 
                If it's been over 24 hours; reset as we shouldn't have
                data anymore.
            */
            var lastEntryDif = recordData.LoadedDateTime.Subtract(lastDateTime);
            if (lastEntryDif.Hours == 0) //Same Hour
            {
                /*
                    No-oping this. 
                    It should be the only check MOST of the time; so having
                    it first will minimize the cycles; and in a heavy load 
                    this small optimization can have significant improvement
                */
            }
            else if (lastEntryDif.Hours > 24)//It's been more than 24 hours; reset
            {
                Initialize(recordData.LoadedDateTime.Hour); //reset
            } 
            else if (lastEntryDif.Hours > 1)//It's been more than an hour since last Update
            {
                MultipleHourJump(recordData.LoadedDateTime, lastEntryDif);
            }
            else if (lastEntryDif.Hours == 1) //This had better be the last case we hit
            {
                NewHour(recordData.LoadedDateTime);
            }
            else
            {
                /*
                    We should REALLY never get here... but... incase we do; KA-BOOM
                */
                var msg = $"Incoming [time={recordData.LoadedDateTime}] is of an unexpected form resulting in an invalid [timeSpan={lastEntryDif}] from [lastDateTime={lastDateTime}]";
                Log.Error(() => msg);
                throw new InvalidOperationException(msg);
            }

            lastHourRecord.Update(recordData);//Update

            lastDateTime = recordData.LoadedDateTime;
        }

        /// <summary>
        /// Handling for a new hour
        /// </summary>
        /// <param name="time">The incoming data's time</param>
        private void NewHour(DateTime time)
        {
            var dataHourTime = time.Hour;
            var lastHourTime = lastHourRecord.Identifier;

            if (lastHourTime == dataHourTime) { return; }

            /*
                        Looping here is scary; but we need to ensure we trim to
                        limit we want to contain.
                    */
            while (hours.Count >= 24) //We've filled a day
            {
                hours.Dequeue(); //Remove the oldest
            }
            lastHourRecord.CompleteAdding();
            lastHourRecord = new HourRecord(time.Hour); //Set the new 'last hour'
            hours.Enqueue(lastHourRecord); //Add the new HourRecord
        }

        /// <summary>
        /// Method to process when there is a multiple hour jump since last update
        /// </summary>
        /// <param name="time">Time of incoming data</param>
        /// <param name="lastEntryDif">Difference since last time updated</param>
        private void MultipleHourJump(DateTime time, TimeSpan lastEntryDif)
        {
            //Clear out enough items to just insert the empty hours
            while (hours.Count + lastEntryDif.Hours >= 24)
            {
                hours.Dequeue();
            }

            for (var missingHour = lastDateTime.Hour; missingHour < time.Hour; missingHour++)
            {
                hours.Enqueue(new HourRecord(missingHour));
            }
            lastHourRecord = new HourRecord(time.Hour);
            hours.Enqueue(lastHourRecord);
        }
        #endregion

        #region Result

        /// <summary>
        /// Get the minute resolution for the last hour
        /// </summary>
        /// <returns>List of Minute Averages in ascending order of time</returns>
        public Tuple<List<double>, List<double>> GetMinutes()
        {
            return lastHourRecord.GetMinuteList();
        }

        /// <summary>
        /// Gets the hour resolution for the day
        /// </summary>
        /// <returns>List of Hour Averages in ascending order of time</returns>
        public Tuple<List<double>, List<double>> GetHours()
        {

            var cpu = new List<double>();
            var ram = new List<double>();
            foreach (var hourRecord in hours)
            {
                cpu.Add(hourRecord.GetCpuLoad());
                ram.Add(hourRecord.GetRamLoad());
            }
            return Tuple.Create(cpu, ram);
        }

        #endregion
    }
}