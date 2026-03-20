using Battleship.Model;
using System.Collections.Generic;
using System.Linq;

namespace GUI
{
    /// <summary>Facade over the Model layer; owns both fleets and wires Gunnery to the player fleet.</summary>
    public class BattleShip
    {
        #region Fields

        private readonly Gunnery gunnery;
        private readonly Shipwright shipwright;
        private Fleet playerFleet = new Fleet();
        private Fleet computerFleet;

        private static readonly List<int> ShipLengths = new List<int> { 5, 4, 4, 3, 3, 3, 2, 2, 2, 2 };
        private const int GridSize = 10;

        #endregion

        #region Constructor

        /// <summary>Initialises gunnery and places the computer fleet, retrying if placement fails.</summary>
        public BattleShip()
        {
            gunnery = new Gunnery(GridSize, GridSize, ShipLengths);
            shipwright = new Shipwright(GridSize, GridSize, ShipLengths);
            do { computerFleet = shipwright.CreateFleet(); }
            while (!computerFleet.Ships.Any());
        }

        #endregion

        #region Public Methods

        /// <summary>Places the player fleet randomly and returns all occupied squares for UI rendering.</summary>
        public IEnumerable<Square> CreatePlayerFleet()
        {
            do { playerFleet = shipwright.CreateFleet(); }
            while (!playerFleet.Ships.Any());
            return playerFleet.Ships.SelectMany(s => s.Squares);
        }

        /// <summary>Fires the player's shot at the computer fleet.</summary>
        public HitResult PlayerShoot(int row, int col) => computerFleet.Shoot(row, col);

        /// <summary>Returns the computer's chosen target; nullable because SurroundingShooting can yield null.</summary>
        public Square? GetComputerTarget() => gunnery.NextTarget();

        /// <summary>Fires the computer's shot and feeds the result back into Gunnery for tactic updates.</summary>
        public HitResult ComputerShoot(Square target)
        {
            var result = playerFleet.Shoot(target.Row, target.Column);
            gunnery.ProcessHitResult(result);
            return result;
        }

        /// <summary>Returns all squares belonging to the player ship that contains the given cell.</summary>
        public IEnumerable<Square> GetPlayerShipSquares(int row, int col) =>
            playerFleet.Ships
                .First(sh => sh.Squares.Any(sq => sq.Row == row && sq.Column == col))
                .Squares;

        /// <summary>Returns all squares belonging to the computer ship that contains the given cell.</summary>
        public IEnumerable<Square> GetComputerShipSquares(int row, int col) =>
            computerFleet.Ships
                .First(sh => sh.Squares.Any(sq => sq.Row == row && sq.Column == col))
                .Squares;

        /// <summary>Total ship count - used to initialise ships-left counters in the UI.</summary>
        public int TotalShips => ShipLengths.Count;

        #endregion
    }
}