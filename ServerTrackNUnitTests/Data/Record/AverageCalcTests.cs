using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerTrack.Data.Record;

namespace ServerTrackNUnitTests.Data.Record
{
    [TestFixture]
    public class AverageCalcTests
    {
        [Test]
        public void UpdateShouldThrowArgumentExceptionGivenNegativeLoad()
        {
            Assert.That(() => new AverageCalc().Update(-1), Throws.ArgumentException, "Average Update Did Not Throw when given a negative value");
        }

        [Test]
        public void UpdateShouldCalculateCorrectAverageNoValue()
        {
            Assert.That(new AverageCalc().Average, Is.EqualTo(0), "Incorrect Average Calculation for no updates");
        }

        [Test]
        public void UpdateShouldCalculateCorrectAverageSingleValue()
        {
            var host = new AverageCalc();
            host.Update(456);
            Assert.That(host.Average, Is.EqualTo(456), "Incorrect Average Calculation for a single update");
        }


        [Test]
        public void UpdateShouldCalculateCorrectAverageTwoValues()
        {
            var host = new AverageCalc();
            host.Update(456);
            host.Update(195);
            Assert.That(host.Average, Is.EqualTo(325.5), "Incorrect Average Calculation for a two updates");
        }


        [Test]
        public void UpdateShouldCalculateCorrectAverageTenValues()
        {
            var host = new AverageCalc();
            for (var i = 1; i <= 10; i++)
            {
                host.Update(i);
            }
            Assert.That(host.Average, Is.EqualTo(5.5), "Incorrect Average Calculation for a two updates");
        }
        [Test]
        public void UpdateShouldCalculateCorrectAverageTenRandomValues()
        {
            UpdateWithRandoms(new AverageCalc(), 10, 1000);
        }

        [Test]
        public void UpdateShouldCalculateCorrectAverage1000RandomValues()
        {
            UpdateWithRandoms(new AverageCalc(), 1000, 10000);
        }

        [Test]
        public void UpdateShouldCalculateCorrectAverageOneHundredThousandRandomValues()
        {
            UpdateWithRandoms(new AverageCalc(), 100000, int.MaxValue);
        }

        //[Test]
        //This test takes a noticeable amount of time; so commented out to not run by default
        public void UpdateShouldCalculateCorrectAverageMaxValues()
        {
            
            UpdateWithRandoms(new AverageCalc(), int.MaxValue, int.MaxValue);
        }

        private static void UpdateWithRandoms(AverageCalc host, int numberOfElements, int maxRange)
        {
            double sum = 0;
            for (var i = 0; i < numberOfElements; i++)
            {
                var val = GetRandomNumber(0, maxRange);
                host.Update(val);
                sum += val;
            }
            var actual = sum / numberOfElements;

            Assert.That(host.Average, Is.EqualTo(actual), "Incorrect Average Calculation");
        }

        private static readonly Random Rand = new Random();
        private static double GetRandomNumber(double minimum, double maximum)
        {
            return Rand.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}
