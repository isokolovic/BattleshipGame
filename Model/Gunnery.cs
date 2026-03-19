using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Battleship.Model
{
    /// <summary>Shooting phases — escalates as hits accumulate, resets on sunk.</summary>
    public enum ShootingTactics { Random, Surrounding, Inline }

    /// <summary>
    /// Runs the three-phase targeting loop:
    /// Random → first hit → Surrounding → second hit → Inline → ship sunk → Random.
    /// </summary>
    public class Gunnery
    {
        #region Fields

        public ShootingTactics ShootingTactics => currentTactics;

        private readonly EnemyGrid monitoringGrid;
        private readonly SquareEliminator squareEliminator;
        private readonly List<int> shipsToShoot;
        private List<Square> squaresHit = new List<Square>();
        private Square? lastTarget;
        private INextTarget targetSelector = null!;
        private ShootingTactics currentTactics = ShootingTactics.Random;

        #endregion

        #region Constructor

        /// <summary>Sets up the monitoring grid and seeds the random shooter with all ship lengths.</summary>
        public Gunnery(int rows, int columns, IEnumerable<int> shipLengths)
        {
            monitoringGrid = new EnemyGrid(rows, columns);
            squareEliminator = new SquareEliminator(rows, columns);
            shipsToShoot = new List<int>(shipLengths);
            ChangeToRandomTactics();
        }

        #endregion

        #region Public Methods

        /// <summary>Asks the active strategy for the next target and caches it for ProcessHitResult.</summary>
        public Square? NextTarget()
        {
            lastTarget = targetSelector.NextTarget();
            return lastTarget;
        }

        /// <summary>Records the shot result on the monitoring grid and updates tactics.</summary>
        public void ProcessHitResult(HitResult hitResult)
        {
            switch (hitResult)
            {
                case HitResult.Missed:
                    RecordOnMonitoringGrid(hitResult);
                    return;

                case HitResult.Hit:
                    squaresHit.Add(lastTarget!);
                    RecordOnMonitoringGrid(hitResult);
                    if (currentTactics == ShootingTactics.Inline) return;
                    break;

                case HitResult.Sunken:
                    squaresHit.Add(lastTarget!);
                    shipsToShoot.Remove(squaresHit.Count);
                    RecordOnMonitoringGrid(hitResult);
                    squaresHit.Clear();
                    break;

                default:
                    Debug.Assert(false);
                    return;
            }

            UpdateTactics(hitResult);
        }

        #endregion

        #region Private Methods

        /// <summary>Marks the last target on the monitoring grid including elimination zones on sunk.</summary>
        private void RecordOnMonitoringGrid(HitResult hitResult)
        {
            switch (hitResult)
            {
                case HitResult.Missed:
                    monitoringGrid.ChangeSquareState(lastTarget!.Row, lastTarget.Column, SquareState.Missed);
                    break;

                case HitResult.Hit:
                    monitoringGrid.ChangeSquareState(lastTarget!.Row, lastTarget.Column, SquareState.Hit);
                    break;

                case HitResult.Sunken:
                    foreach (var s in squaresHit)
                        monitoringGrid.ChangeSquareState(s.Row, s.Column, SquareState.Sunken);
                    foreach (var sq in squareEliminator.ToEliminate(squaresHit))
                        monitoringGrid.ChangeSquareState(sq.Row, sq.Column, SquareState.Eliminated);
                    break;
            }
        }

        /// <summary>Sunk resets to Random. Hit escalates Surrounding -> Inline.</summary>
        private void UpdateTactics(HitResult hitResult)
        {
            if (hitResult == HitResult.Sunken) { ChangeToRandomTactics(); return; }

            switch (currentTactics)
            {
                case ShootingTactics.Random: ChangeToSurroundingTactics(); break;
                case ShootingTactics.Surrounding: ChangeToInlineTactics(); break;
                default: Debug.Assert(false); break;
            }
        }

        private void ChangeToSurroundingTactics()
        {
            currentTactics = ShootingTactics.Surrounding;
            targetSelector = new SurroundingShooting(monitoringGrid, squaresHit.First());
        }

        private void ChangeToInlineTactics()
        {
            currentTactics = ShootingTactics.Inline;
            targetSelector = new InlineShooting(monitoringGrid, squaresHit, shipsToShoot[0]);
        }

        private void ChangeToRandomTactics()
        {
            currentTactics = ShootingTactics.Random;
            targetSelector = new RandomShooting(monitoringGrid, shipsToShoot[0]);
        }

        #endregion
    }
}