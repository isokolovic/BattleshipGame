namespace Battleship.Model
{
    /// <summary>Randomly places ships on a grid, respecting the no-adjacency rule.</summary>
    public class Shipwright
    {
        #region Fields

        private FleetGrid grid;
        private readonly IEnumerable<int> shipLengths;
        private readonly SquareEliminator squareEliminator;
        private readonly Random random = new Random();

        #endregion

        #region Constructor

        public Shipwright(int rows, int columns, IEnumerable<int> shipLengths)
        {
            grid = new FleetGrid(rows, columns);
            this.shipLengths = shipLengths;
            squareEliminator = new SquareEliminator(rows, columns);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Tries to place all ships randomly.
        /// Returns an empty Fleet if placement fails — caller must retry.
        /// </summary>
        public Fleet CreateFleet()
        {
            var fleet = new Fleet();

            foreach (int shipLength in shipLengths)
            {
                var placements = grid.GetAvailablePlacements(shipLength);

                if (!placements.Any())
                {
                    // Placement failed — reset and signal caller to retry
                    grid = new FleetGrid(grid.Rows, grid.Columns);
                    return new Fleet();
                }

                var selected = placements.ElementAt(random.Next(placements.Count()));
                fleet.CreateShip(selected);

                foreach (var sq in squareEliminator.ToEliminate(selected))
                    grid.EliminateSquare(sq.Row, sq.Column);
            }

            return fleet;
        }

        #endregion
    }
}