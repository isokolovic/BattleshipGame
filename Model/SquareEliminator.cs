namespace Battleship.Model
{
    /// <summary>Computes the exclusion zone (bounding box + 1 cell padding) around a placed ship.</summary>
    public class SquareEliminator
    {
        #region Fields

        private readonly int rows;
        private readonly int columns;

        #endregion

        #region Constructor

        public SquareEliminator(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the padded bounding rectangle around the given ship squares.
        /// Uses Min/Max so input order does not matter.
        /// </summary>
        public IEnumerable<Square> ToEliminate(IEnumerable<Square> shipSquares)
        {
            int startRow = Math.Max(0, shipSquares.Min(s => s.Row) - 1);
            int endRow = Math.Min(rows - 1, shipSquares.Max(s => s.Row) + 1);
            int startColumn = Math.Max(0, shipSquares.Min(s => s.Column) - 1);
            int endColumn = Math.Min(columns - 1, shipSquares.Max(s => s.Column) + 1);

            var result = new List<Square>();

            for (int r = startRow; r <= endRow; r++)
                for (int c = startColumn; c <= endColumn; c++)
                    result.Add(new Square(r, c));

            return result;
        }

        #endregion
    }
}