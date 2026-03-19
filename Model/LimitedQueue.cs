namespace Battleship.Model
{
    /// <summary>Fixed-capacity queue. Removes the oldest item when enqueuing past capacity.</summary>
    public class LimitedQueue<T> : Queue<T>
    {
        #region Fields

        private readonly int length;

        #endregion

        #region Constructor

        public LimitedQueue(int length) => this.length = length;

        #endregion

        #region Public Methods

        /// <summary>Drops the front item before enqueuing if already at capacity.</summary>
        public new void Enqueue(T item)
        {
            if (Count >= length) Dequeue();
            base.Enqueue(item);
        }

        #endregion
    }
}