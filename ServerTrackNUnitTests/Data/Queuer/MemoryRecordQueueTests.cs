using NUnit.Framework;
using ServerTrack.Data.Queuer;
using ServerTrack.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerTrackNUnitTests.Data.Queuer
{
    [TestFixture]
    public class MemoryRecordQueueTests
    {
        private static void ConsumerShouldConsumeWithSpecifiedConsumer(int consumers)
        {
            var queuer = new MemoryRecordQueue(consumers);
            const int times = 10000000;
            var collection = new IncomingRecordData[times];
            //Things get consumed so fast that the time to construct objects was making validation spotty
            Console.Out.WriteLine($"[time={DateTime.Now}] Creating {times} IncomingRecords");
            for (var i = 0; i < times; i++)
            {
                collection[i] = new IncomingRecordData()
                {
                    CpuLoad = new Random(i).NextDouble() * 1000
                };
            }
            Console.Out.WriteLine($"[time={DateTime.Now}] Starting Producer");
            var ticks = DateTime.Now.Ticks;
            var producer = Task.Factory.StartNew(() =>
            {
                for (var i = 0; i < times; i++)
                {
                    //Creating the objects above as the creation took too long and the consumer consumed
                    queuer.AddRecordData(collection[i]);
                }
            });

            producer.Wait();
            var queueSize = queuer.QueueSize();
            do
            {
                //Console.Out.WriteLine($"[millisecond={DateTime.Now.Millisecond}] Queue still active [QueueCount={queuer.QueueSize()}]");
                Thread.Sleep(1); //Console output gets overwhelmed if not given a slight pause
            } while (queuer.QueueSize() > 0);
            var endTicks = DateTime.Now.Ticks;

            Console.Out.WriteLine($"[time={DateTime.Now}] Queue Is Zero");
            Console.Out.WriteLine($"[time={DateTime.Now}] Took an average of [ticks={(endTicks - ticks) / times}] per item");

            Assert.That(queueSize, Is.GreaterThan(0), "Did not detect any consuming happening! Did you set the number of records too small?");

        }

        [Test]
        public void ConsumerShouldConsumeWithSingleConsumer()
        {
            ConsumerShouldConsumeWithSpecifiedConsumer(1);
        }

        [Test]
        public void ConsumerShouldConsumeWithTwoConsumers()
        {
            ConsumerShouldConsumeWithSpecifiedConsumer(2);
        }

        [Test]
        public void ConsumerShouldConsumeWithTenConsumers()
        {
            ConsumerShouldConsumeWithSpecifiedConsumer(10);
        }
    }
}
