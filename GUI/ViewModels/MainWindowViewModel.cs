using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace GUI.ViewModels
{
    /// <summary>Top-level VM; controls start/game screen switching and owns the single GameViewModel instance.</summary>
    public partial class MainWindowViewModel : ViewModelBase
    {
        #region Fields

        /// <summary>Drives ContentControl visibility - false = StartView, true = GameView.</summary>
        [ObservableProperty]
        private bool isGameStarted;

        /// <summary>Single instance reused across restarts; StartGame() resets internal state.</summary>
        public GameViewModel Game { get; } = new GameViewModel();

        #endregion

        #region Public Methods

        /// <summary>Initialises and shows the game grid.</summary>
        [RelayCommand]
        private void StartGame()
        {
            Game.StartGame();
            IsGameStarted = true;
        }

        #endregion
    }
}