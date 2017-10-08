namespace Cozyplanes.SudokuApp.Model.PlayerActions
{
	public class HintAction : FillCellAction
	{
		/// <summary>
		/// 플레이어에게 힌트를 제공하는 액션입니다.
		/// </summary>
		/// <param name="row">스도쿠 보드의 행</param>
		/// <param name="column">스도쿠 보드의 행</param>
		/// <param name="value">셀의 값</param>
		public HintAction(byte row, byte column, byte value) : base(row, column, value) { }
	}
}
