using Cozyplanes.SudokuApp.Enums;

namespace Cozyplanes.SudokuApp.Interfaces
{
    /// <summary>
    /// All implementations of this interface have a PlayerDecisionType property.
    /// </summary>
    public interface IPlayerAction
    {
        /// <summary>
        /// The type of the player decision.
        /// </summary>
        PlayerActionType PlayerActionType { get; }
    }
}
