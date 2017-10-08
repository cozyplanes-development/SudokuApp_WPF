using Cozyplanes.SudokuApp.Model.Interfaces;
using Cozyplanes.SudokuApp.Model.Enums;


namespace Cozyplanes.SudokuApp.Model.PlayerActions
{
	public class RestartAction : ChangeSudokuGridCompletelyAction, IPlayerAction
	{
		/// <summary>
		/// 스도쿠를 재시작하는 액션입니다. (= 초기화)
		/// </summary>
		/// <param name="sudokuGrid">스도쿠 보드</param>
		public RestartAction(SudokuRow[] sudokuGrid) : base(sudokuGrid) { }

		public PlayerActionType PlayerActionType
		{
			get
			{
				return PlayerActionType.Restart;
			}
		}
	}
}
