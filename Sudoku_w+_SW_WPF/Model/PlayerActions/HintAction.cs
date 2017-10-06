namespace Cozyplanes.SudokuApp.Model.PlayerActions
{
	public class HintAction : FillCellAction
	{
		public HintAction(byte row, byte column, byte value) : base(row, column, value)
		{
		}
	}
}
