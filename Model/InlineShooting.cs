namespace Battleship.Model
{
    /// <summary>Horizontal or vertical orientation of a partially hit ship.</summary>
    public enum ShipPosition { Horizontal, Vertical }

    /// <summary>Extends shots along the confirmed ship axis once two hits fix the orientation.</summary>
    public class InlineShooting : INextTarget
    {
        #region Fields

        private readonly EnemyGrid grid;
        private readonly List<Square> squaresAlreadyHit;
        private readonly int shipLength;

        #endregion

        #region Constructor

        public InlineShooting(EnemyGrid grid, List<Square> squaresAlreadyHit, int shipLength)
        {
            this.grid = grid;
            this.squaresAlreadyHit = squaresAlreadyHit;
            this.shipLength = shipLength;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Scans both ends of the hit sequence.
        /// Shoots toward whichever end has more open space.
        /// Returns null if both ends are blocked.
        /// </summary>
        public Square? NextTarget()
        {
            IEnumerable<Square> first, last;

            if (GetShipPosition() == ShipPosition.Horizontal)
            {
                int row = squaresAlreadyHit.First().Row;
                int firstColumn = squaresAlreadyHit.Min(x => x.Column);
                int lastColumn = squaresAlreadyHit.Max(x => x.Column);
                first = grid.GetAvailableSquares(row, firstColumn, Direction.Leftwards);
                last = grid.GetAvailableSquares(row, lastColumn, Direction.Rightwards);
            }
            else
            {
                int col = squaresAlreadyHit.First().Column;
                int firstRow = squaresAlreadyHit.Min(x => x.Row);
                int lastRow = squaresAlreadyHit.Max(x => x.Row);
                first = grid.GetAvailableSquares(firstRow, col, Direction.Upwards);
                last = grid.GetAvailableSquares(lastRow, col, Direction.Bottomwards);
            }

            var firstList = first.ToList();
            var lastList = last.ToList();

            if (!firstList.Any() && !lastList.Any()) return null;
            if (!firstList.Any()) return lastList.First();
            if (!lastList.Any()) return firstList.First();

            return firstList.Count > lastList.Count ? firstList.First() : lastList.First();
        }

        #endregion

        #region Private Methods

        /// <summary>Two hits sharing a row means horizontal; otherwise vertical.</summary>
        private ShipPosition GetShipPosition() =>
            squaresAlreadyHit.First().Row == squaresAlreadyHit.Last().Row &&
            squaresAlreadyHit.First().Column != squaresAlreadyHit.Last().Column
                ? ShipPosition.Horizontal
                : ShipPosition.Vertical;

        #endregion
    }
}