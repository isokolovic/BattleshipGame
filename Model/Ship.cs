namespace Battleship.Model
{
    /// <summary>One ship occupying a set of squares. Tracks hits and reports sinking.</summary>
    public class Ship
    {
        #region Fields

        public readonly IEnumerable<Square> Squares;

        #endregion

        #region Constructor

        public Ship(IEnumerable<Square> squares) => Squares = squares;

        #endregion

        #region Public Methods

        /// <summary>
        /// Shoots at this ship.
        /// Returns Missed if the square is not on this ship.
        /// Returns Sunken if this shot destroys the last unhit square.
        /// Returns Hit otherwise.
        /// </summary>
        public HitResult Shoot(int row, int column)
        {
            var found = Squares.FirstOrDefault(s => s.Row == row && s.Column == column);

            if (found == null) return HitResult.Missed;
            if (found.State == SquareState.Sunken) return HitResult.Sunken;
            if (found.State == SquareState.Initial) found.ChangeState(SquareState.Hit);

            int squaresHit = Squares.Count(s => s.State == SquareState.Hit);

            if (squaresHit == Squares.Count())
            {
                foreach (var sq in Squares) sq.ChangeState(SquareState.Sunken);
                return HitResult.Sunken;
            }

            return HitResult.Hit;
        }

        #endregion
    }
}