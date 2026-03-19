namespace Battleship.Model
{
    /// <summary>Strategy for targeting. Each implementation picks the next square to shoot.</summary>
    public interface INextTarget
    {
        /// <summary>Returns the next square to shoot, or null if no valid target exists.</summary>
        Square? NextTarget();
    }
}