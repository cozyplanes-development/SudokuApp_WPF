using Cozyplanes.SudokuApp.Interfaces;
using Cozyplanes.SudokuApp.Enums;

namespace Cozyplanes.SudokuApp.Model.PlayerActions
{
	public class SolveAction : ChangeSudokuGridCompletelyAction, IPlayerAction
	{ 
		public SolveAction(SudokuRow[] sudokuGrid) : base(sudokuGrid)
		{
		}

		public PlayerActionType PlayerActionType
		{
			get
			{
				return PlayerActionType.Solve;
			}
		}
	}
}
