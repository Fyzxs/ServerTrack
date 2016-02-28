using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerTrack.Data.Record;
using ServerTrack.Models;

namespace ServerTrackNUnitTests.Data.Record
{
    [TestFixture]
    public class TimeRecordTests
    {
        [Test]
        public void CtorShouldThrowArgumentExceptionGivenLowIdent()
        {
            try
            {
                // ReSharper disable once ObjectCreationAsStatement
                new ConcereteTimeRecord(1, 2, 0);
                Assert.Fail("No Excpetion Thrown");
            }
            catch (ArgumentException)
            {
            }

            Assert.Pass();
        }
        [Test]
        public void CtorShouldThrowArgumentExceptionGivenHighIdent()
        {
            try
            {
                // ReSharper disable once ObjectCreationAsStatement
                new ConcereteTimeRecord(1, 2, 3);
                Assert.Fail("No Excpetion Thrown");
            }
            catch (ArgumentException)
            {
            }

            Assert.Pass();
        }

        [Test]
        public void CtorShouldNotThrowArgumentExceptionGivenEqualMaxIdent()
        {
            // ReSharper disable once ObjectCreationAsStatement
            new ConcereteTimeRecord(1, 2, 2);
            Assert.Pass();
        }

        [Test]
        public void CtorShouldNotThrowArgumentExceptionGivenEqualMinIdent()
        {
            // ReSharper disable once ObjectCreationAsStatement
            new ConcereteTimeRecord(1, 2, 1);
            Assert.Pass();
        }

        [Test]
        public void CompleteAddingShouldStopUpdates()
        {
            var timeRecord = new ConcereteTimeRecord(1, 1, 1);
            timeRecord.CompleteAdding();
            Assert.That(timeRecord.GetCpuLoad(), Is.EqualTo(0));

            timeRecord.Update(new IncomingRecordData() {CpuLoad = 100});
            Assert.That(timeRecord.GetCpuLoad(), Is.EqualTo(0));
        }

        [Test]
        public void UpdateShouldNotUpdateWhenCompleteAdding()
        {
            var timeRecord = new ConcereteTimeRecord(1, 1, 1);
            timeRecord.CompleteAdding();
            Assert.That(timeRecord.GetCpuLoad(), Is.EqualTo(0));

            timeRecord.Update(new IncomingRecordData() { CpuLoad = 100 });
            Assert.That(timeRecord.GetCpuLoad(), Is.EqualTo(0));
        }

        [Test]
        public void UpdateEmptyShouldAffectAverage()
        {
            var timeRecord = new ConcereteTimeRecord(1, 1, 1);
            timeRecord.Update(new IncomingRecordData() { CpuLoad = 100 });
            timeRecord.UpdateEmpty();
            Assert.That(timeRecord.GetCpuLoad(), Is.EqualTo(50));
        }

        [Test]
        public void UpdateEmptyShouldNotUpdateWhenCompleteAdding()
        {
            var timeRecord = new ConcereteTimeRecord(1, 1, 1);
            timeRecord.Update(new IncomingRecordData() { CpuLoad = 100 });
            timeRecord.CompleteAdding();
            timeRecord.UpdateEmpty();
            Assert.That(timeRecord.GetCpuLoad(), Is.EqualTo(100));
        }

        [Test]
        public void GetCpuLoadShouldRoundToTwo()
        {
            var timeRecord = new ConcereteTimeRecord(1, 1, 1);
            timeRecord.Update(new IncomingRecordData() { CpuLoad = 100 });
            timeRecord.UpdateEmpty();
            timeRecord.UpdateEmpty();
            Assert.That(timeRecord.GetCpuLoad(), Is.EqualTo(33.33));
        }

        [Test]
        public void GetRamLoadShouldRoundToTwo()
        {
            var timeRecord = new ConcereteTimeRecord(1, 1, 1);
            timeRecord.Update(new IncomingRecordData() { RamLoad = 100 });
            timeRecord.UpdateEmpty();
            timeRecord.UpdateEmpty();
            Assert.That(timeRecord.GetRamLoad(), Is.EqualTo(33.33));
        }
    }

    internal class ConcereteTimeRecord : TimeRecord
    {
        public ConcereteTimeRecord(int minIdentifier, int maxIdentifier, int identifier) : base(minIdentifier, maxIdentifier, identifier)
        {
        }
    }
}
