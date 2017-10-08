namespace Cozyplanes.SudokuApp.Model.Interfaces
{
	/// <summary>
	/// 이 인터페이스의 모든 구현은 새로운 셀을 유효한지 확인하고 스도쿠를 해결하는 기능을 가지고 있습니다.
	/// </summary>
	public interface ISudokuSolver
    {
		/// <summary>
		/// 스도쿠 규칙에 알맞게 새로운 셀이 유효한지 검사합니다.
		/// </summary>
		/// <param name="sudokuBoard">9x9 가변 배열</param>
		/// <param name="row">새로운 셀의 행</param>
		/// <param name="column">새로운 셀의 열</param>
		/// <param name="value">새로운 셀의 값</param>
		/// <returns>새로운 셀이 유효한지를 반환합니다.</returns>
		bool IsNewCellValid(byte[][] sudokuBoard, byte row, byte column, byte value);

		/// <summary>
		/// 스도쿠 기본 규칙을 준수하여 스도쿠를 해결합니다.
		/// </summary>
		/// <param name="sudokuBoard">9x9 가변 배열</param>
		/// <returns>스도쿠를 풀 수 있는지를 반환합니다.</returns>
		bool SolveSudoku(byte[][] sudokuBoard);
    }
}
