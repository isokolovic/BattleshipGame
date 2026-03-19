namespace Battleship.Model
{
    /// <summary>Probes the four cardinal neighbours of the first hit square to find ship orientation.</summary>
    public class SurroundingShooting : INextTarget
    {
        #region Fields

        private readonly Square firstSquareHit;
        private readonly EnemyGrid grid;

        #endregion

        #region Constructor

        public SurroundingShooting(EnemyGrid grid, Square firstSquareHit)
        {
            this.grid = grid;
            this.firstSquareHit = firstSquareHit;
        }

        #endregion

        #region Public Methods

        /// <summary>Returns any untouched cardinal neighbour of the first hit, or null if all are taken.</summary>
        public Square? NextTarget()
        {
            int r = firstSquareHit.Row;
            int c = firstSquareHit.Column;

            return grid.Squares.FirstOrDefault(s =>
                s.State == SquareState.Initial &&
                ((s.Row == r && (s.Column == c - 1 || s.Column == c + 1)) ||
                 (s.Column == c && (s.Row == r - 1 || s.Row == r + 1))));
        }

        #endregion
    }
}