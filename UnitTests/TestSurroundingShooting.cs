using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Battleship.Model;

namespace Battleship
{
    [TestClass]
    public class TestSurroundingShooting
    {
        /// <summary>On an empty grid, any of the four cardinal neighbours is a valid target.</summary>
        [TestMethod]
        public void ForEmptyGridSurroundingShootingTargetsOneOfSquaresArroundHitOne()
        {
            var ss = new SurroundingShooting(new EnemyGrid(10, 10), new Square(3, 3));
            var candidates = new List<Square> { new Square(3, 2), new Square(3, 4), new Square(2, 3), new Square(4, 3) };
            CollectionAssert.Contains(candidates, ss.NextTarget());
        }

        /// <summary>Already-marked squares are excluded from candidates.</summary>
        [TestMethod]
        public void SurroundingShootingTargetsOnlySquaresThatAreNotMarked1()
        {
            var grid = new EnemyGrid(10, 10);
            grid.ChangeSquareState(3, 2, SquareState.Hit);
            var ss = new SurroundingShooting(grid, new Square(3, 3));
            var candidates = new List<Square> { new Square(3, 4), new Square(2, 3), new Square(4, 3) };
            CollectionAssert.Contains(candidates, ss.NextTarget());
        }

        /// <summary>With two neighbours marked, only the remaining two are candidates.</summary>
        [TestMethod]
        public void SurroundingShootingTargetsOnlySquaresThatAreNotMarked2()
        {
            var grid = new EnemyGrid(10, 10);
            grid.ChangeSquareState(3, 2, SquareState.Hit);
            grid.ChangeSquareState(3, 4, SquareState.Hit);
            var ss = new SurroundingShooting(grid, new Square(3, 3));
            var candidates = new List<Square> { new Square(2, 3), new Square(4, 3) };
            CollectionAssert.Contains(candidates, ss.NextTarget());
        }

        /// <summary>With three neighbours marked, the single remaining candidate is returned.</summary>
        [TestMethod]
        public void SurroundingShootingTargetsOnlySquaresThatAreNotMarked3()
        {
            var grid = new EnemyGrid(10, 10);
            grid.ChangeSquareState(3, 2, SquareState.Hit);
            grid.ChangeSquareState(3, 4, SquareState.Hit);
            grid.ChangeSquareState(4, 3, SquareState.Hit);
            Assert.AreEqual(new Square(2, 3), new SurroundingShooting(grid, new Square(3, 3)).NextTarget());
        }
    }
}