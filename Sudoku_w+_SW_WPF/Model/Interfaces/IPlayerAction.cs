using Cozyplanes.SudokuApp.Model.Enums;

namespace Cozyplanes.SudokuApp.Model.Interfaces
{
	/// <summary>
	/// 이 인터페이스의 모든 구현은 PlayerActionType 속성을 가지고 있습니다.
	/// </summary>
	public interface IPlayerAction
    {
        /// <summary>
		/// 플레이어 액션의 종류
        /// </summary>
        PlayerActionType PlayerActionType { get; }
    }
}
