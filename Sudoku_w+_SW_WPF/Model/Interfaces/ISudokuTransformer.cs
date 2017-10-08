using Cozyplanes.SudokuApp.Model.Enums;

namespace Cozyplanes.SudokuApp.Model.Interfaces
{
	/// <summary>
	/// 이 인터페이스의 모든 구현은 유효한 스도쿠를 플레이어가 플레이 할 수 있는 스도쿠로 바꾸는 기능을 가지고 있습니다.
	/// </summary>
	public interface ISudokuTransformer
    {
        /// <summary>
		/// 섞으면서 유효한 스도쿠로 부터 다른 스도쿠를 생성합니다.
		/// </summary>
        /// <param name="sudokuBoard">9x9 가변 배열</param>
        void ShuffleSudoku(byte[][] sudokuBoard);

		/// <summary>
		/// 유효한 스도쿠에서 난이도에 알맞게 셀을 제거하여 플레이어가 해결할 수 있도록 합니다.
		/// </summary>
		/// <param name="sudokuBoard">9x9 가변 배열</param>
		/// <param name="difficultyType">난이도에 알맞게 셀을 삭제</param>
		void EraseCells(byte[][] sudokuBoard, SudokuDifficultyType difficultyType);
    }
}
