using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GUI.ViewModels
{
    /// <summary>Holds the 100 cells of a 10x10 grid in row-major order; drives the ItemsControl in the View.</summary>
    public class GridViewModel : ViewModelBase
    {
        #region Fields

        /// <summary>Flat cell list; index = row * 10 + col.</summary>
        public ObservableCollection<SquareCellViewModel> Cells { get; } = new ObservableCollection<SquareCellViewModel>();

        /// <summary>Column header labels A-J for the grid UI.</summary>
        public IReadOnlyList<string> ColumnHeaders { get; } = Enumerable.Range(0, 10).Select(i => ((char)('A' + i)).ToString()).ToList();

        /// <summary>Row number labels 1-10 for the grid UI.</summary>
        public IReadOnlyList<string> RowNumbers { get; } = Enumerable.Range(1, 10).Select(i => i.ToString()).ToList();

        #endregion

        #region Constructor

        /// <summary>Populates all 100 cells; enemy cells receive the shoot action, player cells do not.</summary>
        public GridViewModel(bool isEnemy, Action<SquareCellViewModel>? shootAction = null)
        {
            for (int row = 0; row < 10; row++)
            { 
                for (int col = 0; col < 10; col++)
                { 
                    Cells.Add(new SquareCellViewModel(row, col, isEnemy, isEnemy ? shootAction : null));
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>Direct cell access by coordinate.</summary>
        public SquareCellViewModel GetCell(int row, int col) => Cells[row * 10 + col];

        #endregion
    }
}