using Cozyplanes.SudokuApp.Model.Enums;
using Cozyplanes.SudokuApp.Model;

namespace Cozyplanes.SudokuApp.Model.Interfaces
{
	/// <summary>
	/// 이 인터페이스의 모든 구현은 유효한 스도쿠 보드를 생성하는 기능을 가지고 있습니다.
	/// </summary>
	public interface ISudokuGenerator
    {
        /// <summary>
		/// 해결할 수 있는 유효한 스도쿠 보드를 생성합니다.
        /// </summary>
        /// <param name="sudokuDifficulty">생성된 스도쿠 보드의 난이도</param>
        /// <returns>지정된 난이도에 대한 유효한 스도쿠 보드를 반환합니다.</returns>
        SudokuRow[] GenerateSudoku(SudokuDifficultyType sudokuDifficulty);
    }
}
