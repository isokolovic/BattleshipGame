using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Battleship.Model;

namespace Battleship
{
    [TestClass]
    public class TestShip
    {
        /// <summary>Constructor stores exactly the squares provided.</summary>
        [TestMethod]
        public void ConstructorCreatesShipWithSquaresProvided()
        {
            var ship = new Ship(new List<Square> { new Square(1, 1), new Square(1, 2), new Square(1, 3) });
            Assert.AreEqual(3, ship.Squares.Count());
        }

        /// <summary>Shot outside ship boundaries returns Missed.</summary>
        [TestMethod]
        public void ShootReturnsMissedIfTargetSquareIsNotInShip()
        {
            var ship = new Ship(new List<Square> { new Square(1, 1), new Square(1, 2), new Square(1, 3) });
            Assert.AreEqual(HitResult.Missed, ship.Shoot(2, 5));
        }

        /// <summary>First shot on an occupied square returns Hit.</summary>
        [TestMethod]
        public void ShootReturnsHitIfTargetSquareIsInShip()
        {
            var ship = new Ship(new List<Square> { new Square(1, 1), new Square(1, 2), new Square(1, 3) });
            Assert.AreEqual(HitResult.Hit, ship.Shoot(1, 2));
        }

        /// <summary>Hitting the same already-hit square again still returns Hit.</summary>
        [TestMethod]
        public void ShootReturnsHitForSecondAttemptIfTargetSquareIsInShip()
        {
            var ship = new Ship(new List<Square> { new Square(1, 1), new Square(1, 2), new Square(1, 3) });
            ship.Shoot(1, 2);
            Assert.AreEqual(HitResult.Hit, ship.Shoot(1, 2));
        }

        /// <summary>Hitting the last unhit square sinks the ship.</summary>
        [TestMethod]
        public void ShootReturnsSunkenForTheLastShipSquare()
        {
            var ship = new Ship(new List<Square> { new Square(1, 1), new Square(1, 2), new Square(1, 3) });
            ship.Shoot(1, 2);
            ship.Shoot(1, 3);
            Assert.AreEqual(HitResult.Sunken, ship.Shoot(1, 1));
        }

        /// <summary>Re-shooting a sunken ship returns Sunken immediately.</summary>
        [TestMethod]
        public void ShootReturnsSunkenForTheLastShipSquareIfItIsHitAgain()
        {
            var ship = new Ship(new List<Square> { new Square(1, 1), new Square(1, 2), new Square(1, 3) });
            ship.Shoot(1, 2); ship.Shoot(1, 3); ship.Shoot(1, 1);
            Assert.AreEqual(HitResult.Sunken, ship.Shoot(1, 3));
        }

        /// <summary>Single-square ship sinks on first hit.</summary>
        [TestMethod]
        public void ShootReturnsSunkenForShipConsistingOfSingleSquareIfItIsHit()
        {
            var ship = new Ship(new List<Square> { new Square(1, 2) });
            Assert.AreEqual(HitResult.Sunken, ship.Shoot(1, 2));
        }

        /// <summary>Miss on a partially hit ship still returns Missed.</summary>
        [TestMethod]
        public void ShootReturnsMissedIfTargetSquareIsNotInShipThatHasAlredyBeenHit()
        {
            var ship = new Ship(new List<Square> { new Square(1, 1), new Square(1, 2), new Square(1, 3) });
            ship.Shoot(1, 2);
            Assert.AreEqual(HitResult.Missed, ship.Shoot(1, 5));
        }

        /// <summary>Miss on an already-sunken ship still returns Missed.</summary>
        [TestMethod]
        public void ShootReturnsMissedIfTargetSquareIsNotInShipThatHasAlredyBeenSunken()
        {
            var ship = new Ship(new List<Square> { new Square(1, 1), new Square(1, 2), new Square(1, 3) });
            ship.Shoot(1, 2); ship.Shoot(1, 3); ship.Shoot(1, 1);
            Assert.AreEqual(HitResult.Missed, ship.Shoot(1, 5));
        }
    }
}