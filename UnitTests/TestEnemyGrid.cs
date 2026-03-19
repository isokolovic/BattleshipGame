using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Battleship.Model;

namespace Battleship
{
    [TestClass]
    public class TestEnemyGrid
    {
        /// <summary>3 clear squares exist left of (8,3) on an empty grid.</summary>
        [TestMethod]
        public void GetAvailableSquaresReturns3SquaresLeftToSquare8_3OnGrid10x10()
        {
            Assert.AreEqual(3, new EnemyGrid(10, 10).GetAvailableSquares(8, 3, Direction.Leftwards).Count());
        }

        /// <summary>A marked square at (8,1) reduces leftward scan from 3 to 1.</summary>
        [TestMethod]
        public void GetAvailableSquaresReturns1SquareLeftToSquare8_3OnGrid10x10IfSquare8_1IsMarkedMissed()
        {
            var grid = new EnemyGrid(10, 10);
            grid.ChangeSquareState(8, 1, SquareState.Missed);
            Assert.AreEqual(1, grid.GetAvailableSquares(8, 3, Direction.Leftwards).Count());
        }

        /// <summary>6 clear squares exist right of (8,3) on an empty grid.</summary>
        [TestMethod]
        public void GetAvailableSquaresReturns6SquaresRightToSquare8_3OnGrid10x10()
        {
            Assert.AreEqual(6, new EnemyGrid(10, 10).GetAvailableSquares(8, 3, Direction.Rightwards).Count());
        }

        /// <summary>A Hit square at (8,8) reduces rightward scan from 6 to 4.</summary>
        [TestMethod]
        public void GetAvailableSquaresReturns4SquaresRightToSquare8_3OnGrid10x10IfSquare8_8IsMarkedHit()
        {
            var grid = new EnemyGrid(10, 10);
            grid.ChangeSquareState(8, 8, SquareState.Hit);
            Assert.AreEqual(4, grid.GetAvailableSquares(8, 3, Direction.Rightwards).Count());
        }

        /// <summary>8 clear squares exist above (8,3) on an empty grid.</summary>
        [TestMethod]
        public void GetAvailableSquaresReturns8SquaresAboveSquare8_3OnGrid10x10()
        {
            Assert.AreEqual(8, new EnemyGrid(10, 10).GetAvailableSquares(8, 3, Direction.Upwards).Count());
        }

        /// <summary>A Sunken square at (0,3) reduces upward scan from 8 to 7.</summary>
        [TestMethod]
        public void GetAvailableSquaresReturn7SquaresAboveSquare8_3OnGrid10x10IfSquare0_3IsMarkedSunken()
        {
            var grid = new EnemyGrid(10, 10);
            grid.ChangeSquareState(0, 3, SquareState.Sunken);
            Assert.AreEqual(7, grid.GetAvailableSquares(8, 3, Direction.Upwards).Count());
        }

        /// <summary>Only 1 square exists below (8,3) on a 10-row grid.</summary>
        [TestMethod]
        public void GetAvailableSquaresReturns1SquareBelowSquare8_3OnGrid10x10()
        {
            Assert.AreEqual(1, new EnemyGrid(10, 10).GetAvailableSquares(8, 3, Direction.Bottomwards).Count());
        }

        /// <summary>A marked square at (9,3) blocks the only square below (8,3), yielding 0.</summary>
        [TestMethod]
        public void GetAvailableSquaresReturns0SquaresBelowSquare8_3OnGrid10x10IfSquare9_3IsMarkedMissed()
        {
            var grid = new EnemyGrid(10, 10);
            grid.ChangeSquareState(9, 3, SquareState.Missed);
            Assert.AreEqual(0, grid.GetAvailableSquares(8, 3, Direction.Bottomwards).Count());
        }
    }
}