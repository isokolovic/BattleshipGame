using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Battleship.Model;

namespace Battleship
{
    [TestClass]
    public class TestFleetGrid
    {
        /// <summary>10x10 grid initialises with 100 squares at all four corners.</summary>
        [TestMethod]
        public void ConstructorCreatesGridOf100SquaresForAGridWith10Rows10Columns()
        {
            var grid = new FleetGrid(10, 10);
            Assert.AreEqual(100, grid.Squares.Count());
            Assert.IsTrue(grid.Squares.Contains(new Square(0, 0)));
            Assert.IsTrue(grid.Squares.Contains(new Square(9, 9)));
        }

        /// <summary>1x4 grid yields 2 placements for a length-3 ship.</summary>
        [TestMethod]
        public void GetAvailablePlacementsReturns2PlacementsForAShip3SquaresLongOnGrid1Rows4Columns()
        {
            Assert.AreEqual(2, new FleetGrid(1, 4).GetAvailablePlacements(3).Count());
        }

        /// <summary>5x1 grid yields 3 placements for a length-3 ship.</summary>
        [TestMethod]
        public void GetAvailablePlacementsReturns2PlacementsForAShip3SquaresLongOnGrid5Rows1Columns()
        {
            Assert.AreEqual(3, new FleetGrid(5, 1).GetAvailablePlacements(3).Count());
        }

        /// <summary>Eliminating a square in the middle splits a row into two separate placement groups.</summary>
        [TestMethod]
        public void GetAvailablePlacementsReturns3PlacementsForAShip2SquaresLongOnGrid1Row6ColumnsAfterSquareInColumn2IsEliminated()
        {
            var grid = new FleetGrid(1, 6);
            grid.EliminateSquare(0, 2);
            Assert.AreEqual(5, grid.Squares.Count());
            Assert.AreEqual(3, grid.GetAvailablePlacements(2).Count());
        }

        /// <summary>Eliminating a row square reduces available vertical placements correctly.</summary>
        [TestMethod]
        public void GetAvailablePlacementsReturns2PlacementsForAShip2SquaresLongOnGrid5Rows1ColumnAfterSquareInRow1IsEliminated()
        {
            var grid = new FleetGrid(5, 1);
            grid.EliminateSquare(1, 0);
            Assert.AreEqual(4, grid.Squares.Count());
            Assert.AreEqual(2, grid.GetAvailablePlacements(2).Count());
        }
    }
}