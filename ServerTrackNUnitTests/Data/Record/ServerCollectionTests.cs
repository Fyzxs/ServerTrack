using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using ServerTrack.Data.Record;
using ServerTrack.Models;
using ServerTrack.Models.Interfaces;

namespace ServerTrackNUnitTests.Data.Record
{
    [TestFixture]
    public class ServerCollectionTests
    {
        [Test]
        public void InstanceShouldNotBeNull()
        {
            Assert.That(ServerCollection.Instance, Is.Not.Null);
        }
        [Test]
        public void SetServerCollectionShouldSet()
        {
            var target = new ServerCollection();
            ServerCollection.SetServerCollection(target);

            Assert.That(ServerCollection.Instance, Is.EqualTo(target));
        }

        [Test]
        public void AddRecordLoadShouldAddRecord()
        {
            var incomingData = new IncomingRecordData(){ServerName = "testServer"};

            ServerCollection.Instance.AddRecordLoad(incomingData);

            var display = ServerCollection.Instance.GetDisplayData("testServer");

            Assert.That(display, Is.Not.Null);
        }

        [Test]
        public void GetDisplayDataShouldReturnNullGivenNonExistingServerName()
        {
            var target = ServerCollection.Instance.GetDisplayData("testServer");
            Assert.That(target, Is.Null);
        }

        [Test]
        public void RemoveDisplayDataShouldRemoveDisplayData()
        {
            //Verify it adds
            AddRecordLoadShouldAddRecord();

            ServerCollection.Instance.RemoveDisplayData("testServer");

            //Verify it doesn't exist
            GetDisplayDataShouldReturnNullGivenNonExistingServerName();
        }
    }
}
