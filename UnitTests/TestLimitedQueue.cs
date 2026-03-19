using Microsoft.VisualStudio.TestTools.UnitTesting;
using Battleship.Model;

namespace Battleship
{
    [TestClass]
    public class TestLimitedQueue
    {
        /// <summary>Queue stays at capacity N; oldest item is evicted on overflow.</summary>
        [TestMethod]
        public void EnqueAddsUpToNelementsToQueueWhereNisProvidedInConstructor()
        {
            var queue = new LimitedQueue<int>(3);

            queue.Enqueue(1); Assert.AreEqual(1, queue.Count);
            queue.Enqueue(2); Assert.AreEqual(2, queue.Count);
            queue.Enqueue(3); Assert.AreEqual(3, queue.Count);
            queue.Enqueue(4); Assert.AreEqual(3, queue.Count);

            Assert.IsTrue(queue.Contains(2));
            Assert.IsTrue(queue.Contains(3));
            Assert.IsTrue(queue.Contains(4));

            queue.Enqueue(5); Assert.AreEqual(3, queue.Count);

            Assert.IsTrue(queue.Contains(3));
            Assert.IsTrue(queue.Contains(4));
            Assert.IsTrue(queue.Contains(5));
        }
    }
}