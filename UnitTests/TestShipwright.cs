using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Battleship.Model;

namespace Battleship
{
    [TestClass]
    public class TestShipwright
    {
        /// <summary>Fleet is created with the correct ship count and at least one ship of the max length.</summary>
        [TestMethod]
        public void CreateFleetCreatesFleetForShipLengthsProvided()
        {
            var lengths = new List<int> { 5, 4, 4, 3, 3, 3, 2 };
            var fleet = new Shipwright(10, 10, lengths).CreateFleet();
            Assert.AreEqual(7, fleet.Ships.Count());
            Assert.AreEqual(1, fleet.Ships.Count(s => s.Squares.Count() == 5));
        }
    }
}