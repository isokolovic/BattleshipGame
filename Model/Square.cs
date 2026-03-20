namespace Battleship.Model
{
    /// <summary>Square states in lifecycle order. Numeric order matters - do not reorder.</summary>
    public enum SquareState
    {
        Initial,
        Eliminated,
        Missed,
        Hit,
        Sunken
    }

    /// <summary>One square on the grid. Equality is based on coordinates only, not state.</summary>
    public class Square : IEquatable<Square>
    {
        #region Fields

        public int Row { get; }
        public int Column { get; }

        /// <summary>State can only move forward in the lifecycle.</summary>
        public SquareState State { get; private set; } = SquareState.Initial;

        #endregion

        #region Constructor

        public Square(int row, int column)
        {
            Row = row;
            Column = column;
        }

        #endregion

        #region Public Methods

        /// <summary>Moves state forward. Ignored if newState is equal or behind current state.</summary>
        public void ChangeState(SquareState newState)
        {
            if ((int)State < (int)newState)
                State = newState;
        }

        public bool Equals(Square? other) =>
            other is not null && Row == other.Row && Column == other.Column;

        public override bool Equals(object? obj) => obj is Square s && Equals(s);

        /// <summary>Required when overriding Equals. HashCode.Combine avoids XOR collision patterns.</summary>
        public override int GetHashCode() => HashCode.Combine(Row, Column);

        public static bool operator ==(Square? left, Square? right) => Equals(left, right);
        public static bool operator !=(Square? left, Square? right) => !Equals(left, right);

        #endregion
    }
}