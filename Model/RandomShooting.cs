namespace Battleship.Model
{
    /// <summary>Picks a random square from all positions that could still contain the smallest remaining ship.</summary>
    public class RandomShooting : INextTarget
    {
        #region Fields

        private readonly EnemyGrid grid;
        private readonly int shipLength;
        private readonly Random random = new Random();

        #endregion

        #region Constructor

        public RandomShooting(EnemyGrid grid, int shipLength)
        {
            this.grid = grid;
            this.shipLength = shipLength;
        }

        #endregion

        #region Public Methods

        /// <summary>Picks uniformly at random across all squares in all valid placements. Returns null if no placements exist.</summary>
        public Square? NextTarget()
        {
            var all = grid.GetAvailablePlacements(shipLength).SelectMany(x => x).ToList();
            if (!all.Any()) return null;
            return all[random.Next(all.Count)];
        }

        #endregion
    }
}