using Battleship.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GUI.ViewModels
{
    /// <summary>
    /// Central game logic; owns both grids, drives turn sequencing
    /// </summary>
    public partial class GameViewModel : ViewModelBase
    {
        #region Fields

        private BattleShip game = null!;

        public GridViewModel PlayerGrid { get; private set; } = null!;
        public GridViewModel EnemyGrid { get; private set; } = null!;

        [ObservableProperty] private int humanShipsLeft;
        [ObservableProperty] private int computerShipsLeft;
        [ObservableProperty] private string lastHumanTarget = string.Empty;
        [ObservableProperty] private string lastComputerTarget = string.Empty;

        [ObservableProperty] private string gameMessage = string.Empty;
        [ObservableProperty] private bool isGameOver;
        [ObservableProperty] private bool isPlayerTurn;
        [ObservableProperty] private bool isPlayerGridActive;
        [ObservableProperty] private bool isEnemyGridActive;

        #endregion

        #region Public Methods

        /// <summary>Resets all state and starts a new game; safe to call on restart.</summary>
        public void StartGame()
        {
            game = new BattleShip();
            EnemyGrid = new GridViewModel(isEnemy: true, shootAction: OnPlayerShoot);
            PlayerGrid = new GridViewModel(isEnemy: false);

            OnPropertyChanged(nameof(EnemyGrid));
            OnPropertyChanged(nameof(PlayerGrid));

            foreach (var sq in game.CreatePlayerFleet())
                PlayerGrid.GetCell(sq.Row, sq.Column).State = CellState.Ship;

            HumanShipsLeft = game.TotalShips;
            ComputerShipsLeft = game.TotalShips;
            LastHumanTarget = string.Empty;
            LastComputerTarget = string.Empty;
            IsGameOver = false;

            if (new Random().Next(2) == 0)
            {
                IsPlayerTurn = true;
                IsEnemyGridActive = true;
                IsPlayerGridActive = false;
                GameMessage = string.Empty;
            }
            else
            {
                IsPlayerTurn = false;
                IsEnemyGridActive = false;
                IsPlayerGridActive = true;
                GameMessage = "Computer goes first";
                _ = ComputerTurnAsync();
            }
        }

        /// <summary>Resets and starts a fresh game; bound to the Restart button in GameView.</summary>
        [RelayCommand]
        private void Restart() => StartGame();

        #endregion

        #region Private Methods

        /// <summary>Handles a player click on an enemy cell; triggers computer retaliation on completion.</summary>
        private async void OnPlayerShoot(SquareCellViewModel cell)
        {
            if (!IsPlayerTurn || IsGameOver) return;

            var result = game.PlayerShoot(cell.Row, cell.Column);
            LastHumanTarget = $"{(char)('A' + cell.Column)}{cell.Row + 1}";

            switch (result)
            {
                case HitResult.Missed:
                    cell.State = CellState.Missed;
                    break;
                case HitResult.Hit:
                    cell.State = CellState.Hit;
                    break;
                case HitResult.Sunken:
                    foreach (var sq in game.GetComputerShipSquares(cell.Row, cell.Column))
                        EnemyGrid.GetCell(sq.Row, sq.Column).State = CellState.Sunken;

                    MarkAdjacentEliminated(EnemyGrid, game.GetComputerShipSquares(cell.Row, cell.Column));
                    ComputerShipsLeft--;
                    break;
            }

            if (ComputerShipsLeft <= 0)
            {
                GameMessage = "You win!";
                IsGameOver = true;
                IsEnemyGridActive = false;
                IsPlayerGridActive = false;
                return;
            }

            IsPlayerTurn = false;
            IsEnemyGridActive = false;
            IsPlayerGridActive = true;
            await Task.Delay(1000);
            await ComputerTurnAsync();
        }

        /// <summary>Executes the computer turn; called after 1s delay from OnPlayerShoot.</summary>
        private async Task ComputerTurnAsync()
        {
            if (IsGameOver) return;

            var target = game.GetComputerTarget();
            if (target == null) { IsPlayerTurn = true; return; }

            var result = game.ComputerShoot(target);
            var playerCell = PlayerGrid.GetCell(target.Row, target.Column);
            LastComputerTarget = $"{(char)('A' + target.Column)}{target.Row + 1}";

            switch (result)
            {
                case HitResult.Missed:
                    playerCell.State = CellState.Missed;
                    break;

                case HitResult.Hit:
                    playerCell.State = CellState.Hit;
                    break;

                case HitResult.Sunken:
                    foreach (var sq in game.GetPlayerShipSquares(target.Row, target.Column))
                        PlayerGrid.GetCell(sq.Row, sq.Column).State = CellState.Sunken;
                    MarkAdjacentEliminated(PlayerGrid, game.GetPlayerShipSquares(target.Row, target.Column));
                    HumanShipsLeft--;
                    break;
            }

            if (HumanShipsLeft <= 0)
            {
                GameMessage = "Computer wins!";
                IsGameOver = true;
                IsEnemyGridActive = false;
                IsPlayerGridActive = false;
                return;
            }

            IsPlayerTurn = true;
            IsEnemyGridActive = true;
            IsPlayerGridActive = false;
            GameMessage = string.Empty;
            await Task.CompletedTask;
        }

        /// <summary>Marks all Initial/Ship cells adjacent to a sunk ship as Eliminated.</summary>
        private static void MarkAdjacentEliminated(GridViewModel grid, IEnumerable<Square> shipSquares)
        {
            foreach (var sq in shipSquares)
            {
                for (int dr = -1; dr <= 1; dr++)
                {
                    for (int dc = -1; dc <= 1; dc++)
                    {
                        int r = sq.Row + dr, c = sq.Column + dc;
                        if (r < 0 || r >= 10 || c < 0 || c >= 10) continue;

                        var cell = grid.GetCell(r, c);
                        if (cell.State == CellState.Initial)
                            cell.State = CellState.Eliminated;
                    }
                }
            }
        }

        #endregion
    }
}