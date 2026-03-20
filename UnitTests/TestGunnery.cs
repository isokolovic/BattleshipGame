using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Battleship.Model;

namespace Battleship
{
    [TestClass]
    public class TestGunnery
    {
        /// <summary>Tactics start as Random.</summary>
        [TestMethod]
        public void InitiallyShootingTacticsIsRandom()
        {
            var g = new Gunnery(10, 10, new List<int> { 5, 3 });
            Assert.AreEqual(ShootingTactics.Random, g.ShootingTactics);
        }

        /// <summary>A miss does not advance tactics.</summary>
        [TestMethod]
        public void ShootingTacticsRemainsRandomIfHitResultIsMissed()
        {
            var g = new Gunnery(10, 10, new List<int> { 5, 3 });
            g.NextTarget();
            g.ProcessHitResult(HitResult.Missed);
            Assert.AreEqual(ShootingTactics.Random, g.ShootingTactics);
        }

        /// <summary>First hit escalates to Surrounding.</summary>
        [TestMethod]
        public void ShootingTacticsChangesToSurroundingAfterFirstSquareIsHit()
        {
            var g = new Gunnery(10, 10, new List<int> { 5, 3 });
            g.NextTarget();
            g.ProcessHitResult(HitResult.Hit);
            Assert.AreEqual(ShootingTactics.Surrounding, g.ShootingTactics);
        }

        /// <summary>A miss while Surrounding keeps Surrounding active.</summary>
        [TestMethod]
        public void ShootingTacticsRemainsSurroundingAfterSecondSqaureIsMissed()
        {
            var g = new Gunnery(10, 10, new List<int> { 5, 3 });
            g.NextTarget();
            g.ProcessHitResult(HitResult.Hit);
            g.NextTarget();
            g.ProcessHitResult(HitResult.Missed);
            Assert.AreEqual(ShootingTactics.Surrounding, g.ShootingTactics);
        }

        /// <summary>Second hit escalates from Surrounding to Inline.</summary>
        [TestMethod]
        public void ShootingTacticsChangesFromSurroundingToInlineAfterSecondSquareIsHit()
        {
            var g = new Gunnery(10, 10, new List<int> { 5, 3 });
            g.NextTarget();
            g.ProcessHitResult(HitResult.Hit);
            g.NextTarget();
            g.ProcessHitResult(HitResult.Hit);
            Assert.AreEqual(ShootingTactics.Inline, g.ShootingTactics);
        }

        /// <summary>A miss while Inline keeps Inline active.</summary>
        [TestMethod]
        public void ShootingTacticsRemainsInlineAfterThirdSquareIsMissed()
        {
            var g = new Gunnery(10, 10, new List<int> { 5, 3 });
            g.NextTarget();
            g.ProcessHitResult(HitResult.Hit);
            g.NextTarget();
            g.ProcessHitResult(HitResult.Hit);
            g.NextTarget();
            g.ProcessHitResult(HitResult.Missed);
            Assert.AreEqual(ShootingTactics.Inline, g.ShootingTactics);
        }

        /// <summary>Sinking a ship resets tactics back to Random.</summary>
        [TestMethod]
        public void ShootingTacticsChangesToRandomAfterShipIsSunken()
        {
            var g = new Gunnery(10, 10, new List<int> { 5, 2 });
            g.NextTarget();
            g.ProcessHitResult(HitResult.Hit);
            g.NextTarget();
            g.ProcessHitResult(HitResult.Sunken);
            Assert.AreEqual(ShootingTactics.Random, g.ShootingTactics);
        }
    }
}