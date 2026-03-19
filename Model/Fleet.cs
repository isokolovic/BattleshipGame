namespace Battleship.Model
{
    /// <summary>Possible outcomes when shooting at a fleet.</summary>
    public enum HitResult { Missed, Hit, Sunken }

    /// <summary>Collection of ships. Delegates shooting to each ship in turn.</summary>
    public class Fleet
    {
        #region Fields

        public IEnumerable<Ship> Ships => ships;

        private readonly List<Ship> ships = new List<Ship>();

        #endregion

        #region Public Methods

        /// <summary>Creates a ship from the given squares and adds it to the fleet.</summary>
        public void CreateShip(IEnumerable<Square> squares) => ships.Add(new Ship(squares));

        /// <summary>Fires at all ships. Returns the first non-Missed result, or Missed if none hit.</summary>
        public HitResult Shoot(int row, int column)
        {
            foreach (var ship in ships)
            {
                var result = ship.Shoot(row, column);
                if (result != HitResult.Missed) return result;
            }
            return HitResult.Missed;
        }

        #endregion
    }
}