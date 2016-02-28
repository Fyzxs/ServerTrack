using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerTrack.Data.Record;
using ServerTrack.Models;
using ServerTrack.Models.Interfaces;

namespace ServerTrackNUnitTests.Data.Record
{
    [TestFixture]
    public class HourRecordTests
    {
        [Test]
        public void GetMinuteListShouldThrowNotSupportedExceptionGivenCompleteAdding()
        {
            try
            {
                var hourRecord = new HourRecord(1);
                hourRecord.CompleteAdding();
                hourRecord.GetMinuteList();
                Assert.Fail("Expected Exception");
            }
            catch (NotSupportedException)
            {
            }
            Assert.Pass();
        }

        [Test]
        public void CtorShouldNotThrowExceptionGivenValidInputs()
        {
            //Test ALL THE VALUES!!!
            for (var i = 0; i <= 23; i++)
            {
                // ReSharper disable once ObjectCreationAsStatement
                new HourRecord(i);
            }
        }

        [Test]
        public void UpdateShouldUpdateMinutes()
        {
            var data = new IncomingRecordData() {CpuLoad = 100};
            var testDate = data.LoadedDateTime;

            var hourRecord = new HourRecord(testDate.Hour);
            hourRecord.Update(data);
            Assert.That(hourRecord.GetMinuteList().Item1, Is.EqualTo(100));

            data.CpuLoad = 300;
            hourRecord.Update(data);
            Assert.That(hourRecord.GetMinuteList().Item1, Is.EqualTo(200));
        }


        [Test]
        public void UpdateShouldUpdateMissingMinutes()
        {
            var data = new TestIncomingRecordData() { CpuLoad = 100, LoadedDateTime = DateTime.Now };
            var testDate = data.LoadedDateTime;

            var hourRecord = new HourRecord(testDate.Hour);
            hourRecord.Update(data);
            Assert.That(hourRecord.GetMinuteList().Item1[0], Is.EqualTo(100.0d));

            data.CpuLoad = 300;
            data.LoadedDateTime = data.LoadedDateTime.Subtract(new TimeSpan(0, 0, 4, 0));
            hourRecord.Update(data);
            Assert.That(hourRecord.GetMinuteList().Item1.Count, Is.EqualTo(6));
        }

        [Test]
        public void GetMinuteListShouldUpdateMissingMinutes()
        {
            var data = new TestIncomingRecordData() { CpuLoad = 100, LoadedDateTime = DateTime.Now };
            var testDate = data.LoadedDateTime;

            var hourRecord = new HourRecord(testDate.Hour);
            data.CpuLoad = 300;
            data.LoadedDateTime = data.LoadedDateTime.Subtract(new TimeSpan(0, 0, 4, 0));
            hourRecord.Update(data);
            Assert.That(hourRecord.GetMinuteList().Item1.Count, Is.EqualTo(6));
        }
    }

    internal class TestIncomingRecordData : IIncomingRecordData {
        public DateTime LoadedDateTime { get; set; }
        public string ServerName { get; set; }
        public double CpuLoad { get; set; }
        public double RamLoad { get; set; }
    }
}
