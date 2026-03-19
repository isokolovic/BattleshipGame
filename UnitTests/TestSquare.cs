using Microsoft.VisualStudio.TestTools.UnitTesting;
using Battleship.Model;

namespace Battleship
{
    [TestClass]
    public class TestSquare
    {
        /// <summary>Constructor stores row and column correctly.</summary>
        [TestMethod]
        public void ConstructorCreatesSquareWithGivenCoordinates()
        {
            var s = new Square(1, 5);
            Assert.AreEqual(1, s.Row);
            Assert.AreEqual(5, s.Column);
        }

        /// <summary>State starts at Initial.</summary>
        [TestMethod]
        public void InitialStateIsInitial()
        {
            var s = new Square(0, 0);
            Assert.AreEqual(SquareState.Initial, s.State);
        }

        /// <summary>State advances forward through the lifecycle.</summary>
        [TestMethod]
        public void ChangeState_AllowsForwardProgression()
        {
            var s = new Square(1, 1);
            s.ChangeState(SquareState.Hit);
            Assert.AreEqual(SquareState.Hit, s.State);
            s.ChangeState(SquareState.Sunken);
            Assert.AreEqual(SquareState.Sunken, s.State);
        }

        /// <summary>State does not go backwards.</summary>
        [TestMethod]
        public void ChangeState_DoesNotRegress()
        {
            var s = new Square(1, 1);
            s.ChangeState(SquareState.Hit);
            s.ChangeState(SquareState.Missed);
            Assert.AreEqual(SquareState.Hit, s.State);
        }

        /// <summary>Two squares at the same position are equal regardless of state.</summary>
        [TestMethod]
        public void Equals_BasedOnCoordinatesOnly()
        {
            var a = new Square(2, 3);
            var b = new Square(2, 3);
            b.ChangeState(SquareState.Hit);
            Assert.IsTrue(a.Equals(b));
            Assert.IsTrue(a == b);
            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }

        /// <summary>Different coordinates are not equal.</summary>
        [TestMethod]
        public void NotEquals_DifferentCoordinates()
        {
            var a = new Square(2, 3);
            var b = new Square(3, 2);
            Assert.IsFalse(a.Equals(b));
            Assert.IsTrue(a != b);
        }
    }
}