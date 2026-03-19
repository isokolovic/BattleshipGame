using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Battleship.Model;

namespace Battleship
{
    [TestClass]
    public class TestSquareEliminator
    {
        /// <summary>4-square horizontal ship in the middle produces an 18-square exclusion zone.</summary>
        [TestMethod]
        public void ToEliminateReturns18SquaresForShipInSquares4x3_4x6()
        {
            var elim = new SquareEliminator(10, 10);
            var result = elim.ToEliminate(new List<Square> {
                new Square(4,3), new Square(4,4), new Square(4,5), new Square(4,6) });
            Assert.AreEqual(18, result.Count());
            CollectionAssert.Contains(result.ToArray(), new Square(3, 2));
            CollectionAssert.Contains(result.ToArray(), new Square(5, 7));
        }

        /// <summary>2-square ship on the top edge clips the exclusion zone to 8 squares.</summary>
        [TestMethod]
        public void ToEliminateReturns8SquaresForShipInSqaures0x3_0x4()
        {
            var elim = new SquareEliminator(10, 10);
            var result = elim.ToEliminate(new List<Square> { new Square(0, 3), new Square(0, 4) });
            Assert.AreEqual(8, result.Count());
        }

        /// <summary>3-square vertical ship touching the bottom edge clips correctly to 12 squares.</summary>
        [TestMethod]
        public void ToEliminateReturns12SquaresForShipInSqaures7x5_9x5()
        {
            var elim = new SquareEliminator(10, 10);
            var result = elim.ToEliminate(new List<Square> {
                new Square(7,5), new Square(8,5), new Square(9,5) });
            Assert.AreEqual(12, result.Count());
        }
    }
}