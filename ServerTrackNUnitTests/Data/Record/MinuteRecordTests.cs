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
    public class MinuteRecordTests
    {
        [Test]
        public void CtorShouldThrowExceptionGivenNegative()
        {
            try
            {
                // ReSharper disable once ObjectCreationAsStatement
                new MinuteRecord(-1);
                Assert.Fail("Excpetion Expected");
            }
            catch (ArgumentException)
            {
            }
            Assert.Pass();
        }

        [Test]
        public void CtorShouldNotThrowExceptionGivenValidInputs()
        {
            //Test ALL THE VALUES!!!
            for (var i = 0; i <= 59; i++)
            {
                // ReSharper disable once ObjectCreationAsStatement
                new MinuteRecord(i);
            }
        }

        [Test]
        public void CtorShouldThrowExceptionGivenSixty()
        {
            try
            {
                // ReSharper disable once ObjectCreationAsStatement
                new MinuteRecord(60);
                Assert.Fail("Excpetion Expected");
            }
            catch (ArgumentException)
            {
            }
            Assert.Pass();
        }
    }
}
