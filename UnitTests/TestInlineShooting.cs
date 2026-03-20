using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Battleship.Model;

namespace Battleship
{
    [TestClass]
    public class TestInlineShooting
    {
        /// <summary>Vertical hits at (3,3) and (4,3) - next target is either end of the sequence.</summary>
        [TestMethod]
        public void NextTargetIsSquare2_3OrSquare5_3AfterSquares3_3And4_3AreHit()
        {
            var inline = new InlineShooting(new EnemyGrid(10, 10),
                new List<Square> { new Square(3, 3), new Square(4, 3) }, 5);
            CollectionAssert.Contains(
                new List<Square> { new Square(2, 3), new Square(5, 3) }, inline.NextTarget());
        }

        /// <summary>Same squares in reverse order - result is identical.</summary>
        [TestMethod]
        public void NextTargetIsSquare2_3OrSquare5_3AfterSquares4_3And3_3AreHit()
        {
            var inline = new InlineShooting(new EnemyGrid(10, 10),
                new List<Square> { new Square(4, 3), new Square(3, 3) }, 5);
            CollectionAssert.Contains(
                new List<Square> { new Square(2, 3), new Square(5, 3) }, inline.NextTarget());
        }

        /// <summary>Four horizontal hits - next target is either open end of the run.</summary>
        [TestMethod]
        public void NextTargetIsSquare4_2OrSquare4_7AfterSquares4_4And4_5And4_6And4_3AreHit()
        {
            var inline = new InlineShooting(new EnemyGrid(10, 10),
                new List<Square> { new Square(4, 4), new Square(4, 5), new Square(4, 6), new Square(4, 3) }, 5);
            CollectionAssert.Contains(
                new List<Square> { new Square(4, 2), new Square(4, 7) }, inline.NextTarget());
        }
    }
}