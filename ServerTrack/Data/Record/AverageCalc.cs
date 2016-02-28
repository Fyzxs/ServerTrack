/*  
 Licensed under the MIT License. See LICENSE file in the project root for full license information.  
*/
using System;
using log4net;
using ServerTrack.Log;

namespace ServerTrack.Data.Record
{

    /// <summary>
    /// Class to encapsulate the average calculations for a measure
    /// </summary>
    public class AverageCalc
    {   /// <summary>
        /// Logger for the class
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(AverageCalc));

        /// <summary>
        /// The number of values that have been added
        /// </summary>
        private double count;

        /// <summary>
        /// The sum of all measures
        /// </summary>
        private double sum;

        /// <summary>
        /// Update the record with a new value
        /// </summary>
        /// <param name="measure"></param>
        public void Update(double measure)
        {
            if (measure < 0)
            {
                var msg = string.Format($"{measure} must be above 0 (zero)");
                Log.Error(() => msg);
                throw new ArgumentException(msg);
            }
            sum += measure;
            count++;
        }

        /// <summary>
        /// The Average
        /// </summary>
        public double Average => count > 0 ? sum / count : 0;
    }
}