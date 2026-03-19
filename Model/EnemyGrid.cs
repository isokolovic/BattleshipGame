namespace Battleship.Model
{
    using SquareSequence = IEnumerable<Square>;

    /// <summary>Shooting directions used when scanning open squares from a hit.</summary>
    public enum Direction
    {
        Leftwards,
        Upwards,
        Rightwards,
        Bottomwards
    }

    /// <summary>Tracks shots against the player grid. Supports directional scans for targeting.</summary>
    public class EnemyGrid : Grid
    {
        #region Constructor

        public EnemyGrid(int rows, int columns) : base(rows, columns) { }

        #endregion

        #region Public Methods

        /// <summary>Moves a square's state forward without allowing it to go back.</summary>
        public void ChangeSquareState(int row, int column, SquareState newState)
        {
            if ((squares[row, column].State == SquareState.Hit || squares[row, column].State == SquareState.Initial) && squares[row, column].State < newState)
            {
                squares[row, column].ChangeState(newState);
            }
        }

        /// <summary>
        /// Walks from a cell in the given direction, stopping at any non-Initial square or the grid edge.
        /// Returns the open squares found.
        /// </summary>
        public SquareSequence GetAvailableSquares(int row, int column, Direction direction)
        {
            int deltaRow = 0;      // Change in row per step (up = -1, down = +1, none = 0)
            int deltaColumn = 0;   // Change in column per step (left = -1, right = +1, none = 0)
            int counter = 0;       // Number of steps available in the chosen direction

            switch (direction)
            {
                case Direction.Leftwards:
                    deltaColumn = -1;
                    counter = column;
                    break;
                case Direction.Upwards:
                    deltaRow = -1;
                    counter = row;
                    break;
                case Direction.Rightwards:
                    deltaColumn = +1;
                    counter = Columns - column - 1;
                    break;
                case Direction.Bottomwards:
                    deltaRow = +1;
                    counter = Rows - row - 1;
                    break;
            }

            var result = new List<Square>();

            for (int i = 0; i < counter; i++)
            {
                row += deltaRow;
                column += deltaColumn;

                if (squares[row, column].State != SquareState.Initial) break;

                result.Add(new Square(row, column));
            }

            return result;
        }

        #endregion

        #region Private Methods

        /// <summary>A square is available if it is still Initial.</summary>
        protected override bool IsSquareAvailable(int i1, int i2, Func<int, int, Square> squareSelect) => squareSelect(i1, i2).State == SquareState.Initial;

        #endregion
    }
}