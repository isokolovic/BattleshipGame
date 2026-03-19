namespace Battleship.Model
{
    using SquareSequence = IEnumerable<Square>;

    /// <summary>Base grid with placement logic over a 2D square array.</summary>
    public abstract class Grid
    {
        #region Fields

        public readonly int Rows;
        public readonly int Columns;

        /// <summary>All non-null squares in the grid.</summary>
        public IEnumerable<Square> Squares =>
            squares.Cast<Square>().Where(s => s != null);

        protected Square[,] squares;

        #endregion

        #region Constructor

        /// <summary>Fills every cell with a fresh square in Initial state.</summary>
        public Grid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            squares = new Square[Rows, Columns];

            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                    squares[r, c] = new Square(r, c);
        }

        #endregion

        #region Public Methods

        /// <summary>All horizontal and vertical runs of available squares that fit the given ship length.</summary>
        public IEnumerable<SquareSequence> GetAvailablePlacements(int length)
        {
            return GetPlacements(length, new LoopIndex(Rows, Columns), (i, j) => squares[i, j])
                .Concat(GetPlacements(length, new LoopIndex(Columns, Rows), (i, j) => squares[j, i]))
                .Where(pl => pl.Count() > 0);
        }

        #endregion

        #region Private Methods

        /// <summary>Subclass defines what available means for its grid type.</summary>
        protected abstract bool IsSquareAvailable(int i1, int i2, Func<int, int, Square> squareSelect);

        /// <summary>Slides a fixed-length window along each row or column and collects valid placements.</summary>
        private IEnumerable<SquareSequence> GetPlacements(int length, LoopIndex loopIndex, Func<int, int, Square> squareSelect)
        {
            var result = new List<SquareSequence>();

            foreach (int o in loopIndex.Outer())
            {
                var queue = new LimitedQueue<Square>(length);

                foreach (int i in loopIndex.Inner())
                {
                    if (IsSquareAvailable(o, i, squareSelect))
                    {
                        queue.Enqueue(squareSelect(o, i));
                        if (queue.Count >= length)
                            result.Add(queue.ToArray());
                    }
                    else
                    {
                        queue.Clear();
                    }
                }
            }

            return result;
        }

        #endregion

        #region Helpers

        /// <summary>Wraps outer/inner bounds to support both row-major and column-major traversal.</summary>
        private class LoopIndex
        {
            private readonly int outerBound;
            private readonly int innerBound;

            public LoopIndex(int outerBound, int innerBound)
            {
                this.outerBound = outerBound;
                this.innerBound = innerBound;
            }

            public IEnumerable<int> Outer()
            {
                for (int i = 0; i < outerBound; i++) yield return i;
            }

            public IEnumerable<int> Inner()
            {
                for (int i = 0; i < innerBound; i++) yield return i;
            }
        }

        #endregion
    }
}