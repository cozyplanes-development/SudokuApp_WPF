using Cozyplanes.SudokuApp.Model.Interfaces;
using Cozyplanes.SudokuApp.Model.Enums;

namespace Cozyplanes.SudokuApp.Model.PlayerActions
{
	public class SolveAction : ChangeSudokuGridCompletelyAction, IPlayerAction
	{ 
		/// <summary>
		/// 스도쿠를 해결하는 액션입니다.
		/// </summary>
		/// <param name="sudokuGrid">스도쿠 보드 (Grid)</param>
		public SolveAction(SudokuRow[] sudokuGrid) : base(sudokuGrid) { }

		public PlayerActionType PlayerActionType
		{
			get
			{
				return PlayerActionType.Solve;
			}
		}
	}
}
