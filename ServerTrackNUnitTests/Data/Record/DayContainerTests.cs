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
    public class DayContainerTests
    {
        [Test]
        public void HasInitialHour()
        {
            var dc = new DayContainer();
            Assert.That(dc.GetHours().Item1.Count, Is.EqualTo(1));
        }
        [Test]
        public void AddsMissingHours()
        {
            var dc = new DayContainer();
            dc.Update(new TestIncomingRecordData() {LoadedDateTime = DateTime.Now.AddHours(2)});
            Assert.That(dc.GetHours().Item1.Count, Is.EqualTo(4));
        }

        [Test]
        public void AddAtNewHours()
        {
            var dc = new DayContainer();
            dc.Update(new TestIncomingRecordData() { LoadedDateTime = DateTime.Now.AddHours(1) });
            Assert.That(dc.GetHours().Item1.Count, Is.EqualTo(2));
        }
    }
}
