using Avalonia.Controls;
using Avalonia.Input;
using GUI.ViewModels;

namespace GUI.Views
{
    /// <summary>10x10 grid with A-J column headers and 1-10 row numbers.</summary>
    public partial class GridView : UserControl
    {
        public GridView() => InitializeComponent();

        /// <summary>Executes ShootCommand on the clicked cell if it is clickable.</summary>
        private void OnCellPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (sender is Border border &&
                border.DataContext is SquareCellViewModel cell &&
                cell.IsClickable)
            {
                cell.ShootCommand.Execute(null);
            }
        }
    }
}