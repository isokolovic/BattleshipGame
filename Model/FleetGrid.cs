namespace Battleship.Model
{
    /// <summary>Grid used during fleet placement. Squares can be nulled to block a position.</summary>
    public class FleetGrid : Grid
    {
        #region Constructor

        public FleetGrid(int rows, int columns) : base(rows, columns) { }

        #endregion

        #region Public Methods

        /// <summary>Removes a square from placement permanently.</summary>
        public void EliminateSquare(int row, int column) => squares[row, column] = null!;

        #endregion

        #region Private Methods

        /// <summary>A square is available if it has not been eliminated.</summary>
        protected override bool IsSquareAvailable(int i1, int i2, Func<int, int, Square> squareSelect) => squareSelect(i1, i2) != null;

        #endregion
    }
}