using Cozyplanes.SudokuApp.Interfaces;
using Cozyplanes.SudokuApp.Enums;


namespace Cozyplanes.SudokuApp.Model.PlayerActions
{
	public class RestartAction : ChangeSudokuGridCompletelyAction, IPlayerAction
	{
		public RestartAction(SudokuRow[] sudokuGrid) : base(sudokuGrid)
		{
		}

		public PlayerActionType PlayerActionType
		{
			get
			{
				return PlayerActionType.Restart;
			}
		}
	}
}
