using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Battleship.Model;

namespace Battleship
{
    [TestClass]
    public class TestRandomShooting
    {
        /// <summary>Target is always one of the grid's available squares.</summary>
        [TestMethod]
        public void RandomShootingSelectsOneOfSquaresFromEmptyGrid()
        {
            var grid = new EnemyGrid(10, 10);
            CollectionAssert.Contains(grid.Squares.ToArray(), new RandomShooting(grid, 3).NextTarget());
        }
    }
}