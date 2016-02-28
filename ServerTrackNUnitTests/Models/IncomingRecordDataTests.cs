using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerTrack.Models;

namespace ServerTrackNUnitTests.Models
{
    [TestFixture]
    public class IncomingRecordDataTests
    {
        [Test]
        public void HasNow()
        {
            var target = new IncomingRecordData();
            var now = DateTime.Now;
            var diff = now.Subtract(target.LoadedDateTime);
            Assert.That(diff.Ticks, Is.LessThan(25000));//It's a few ticks
        }
    }
}

