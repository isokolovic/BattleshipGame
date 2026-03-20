using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace GUI.ViewModels
{
    /// <summary>Visual state of a single grid cell.</summary>
    public enum CellState
    {
        Initial,
        Ship,
        Eliminated,
        Missed,
        Hit,
        Sunken
    }

    /// <summary>ViewModel for one grid cell; drives background colour, click enablement, and shoot command.</summary>
    public partial class SquareCellViewModel : ViewModelBase
    {
        #region Fields

        public int Row { get; }
        public int Column { get; }
        public bool IsEnemyCell { get; }

        /// <summary>Callback supplied by GameViewModel; invoked when the player fires at this cell.</summary>
        private readonly Action<SquareCellViewModel>? shootAction;

        /// <summary>Changing State invalidates BackgroundBrush, IsClickable, and ShootCommand.CanExecute.</summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(BackgroundBrush))]
        [NotifyPropertyChangedFor(nameof(IsClickable))]
        [NotifyCanExecuteChangedFor(nameof(ShootCommand))]
        private CellState state = CellState.Initial;

        #endregion

        #region Constructor

        /// <summary>Constructs a cell at the given position; only enemy cells receive a shoot action.</summary>
        public SquareCellViewModel(int row, int column, bool isEnemyCell,
            Action<SquareCellViewModel>? shootAction = null)
        {
            Row = row;
            Column = column;
            IsEnemyCell = isEnemyCell;
            this.shootAction = shootAction;
        }

        #endregion

        #region Public Methods

        /// <summary>True only for unshot enemy cells - gates ShootCommand.CanExecute.</summary>
        public bool IsClickable => IsEnemyCell && State == CellState.Initial;

        private static readonly IBrush WaterBrush = new SolidColorBrush(Color.Parse("#90CAF9"));
        private static readonly IBrush ShipBrush = new SolidColorBrush(Color.Parse("#546E7A"));
        private static readonly IBrush MissedBrush = new SolidColorBrush(Color.Parse("#CFD8DC"));
        private static readonly IBrush HitBrush = new SolidColorBrush(Color.Parse("#FF9800"));
        private static readonly IBrush SunkenBrush = new SolidColorBrush(Color.Parse("#EF5350"));
        private static readonly IBrush EliminatedBrush = new SolidColorBrush(Color.Parse("#E0E0E0"));

        /// <summary>Maps CellState to a brush.</summary>
        public IBrush BackgroundBrush => State switch
        {
            CellState.Initial => WaterBrush,
            CellState.Ship => ShipBrush,
            CellState.Missed => MissedBrush,
            CellState.Hit => HitBrush,
            CellState.Sunken => SunkenBrush,
            CellState.Eliminated => EliminatedBrush,
            _ => WaterBrush
        };

        /// <summary>Fires the shoot callback; CanExecute is tied to IsClickable.</summary>
        [RelayCommand(CanExecute = nameof(CanShoot))]
        private void Shoot() => shootAction?.Invoke(this);

        private bool CanShoot() => IsClickable;

        #endregion
    }
}