using System;
using Cozyplanes.SudokuApp.Model.Interfaces;

namespace Cozyplanes.SudokuApp.ViewModel
{
	/// <summary>
	/// 이 클래스는 새로운 셀이 유효한지 검사하고 새로운 스도쿠를 해결하는 기능을 가지고 있습니다.
	/// </summary>
	public class SudokuSolver : ISudokuSolver
	{
		private const string InvalidSudokuBoardMessage = "SudokuBoard must be a 9x9 jagged byte array!";

		/// <summary>
		/// 새로운 셀이 스도쿠 기본 규칙을 따르는지, 유효한지를 검사합니다.
		/// </summary>
		/// <param name="sudokuBoard">9x9 가변 배열</param>
		/// <param name="row">새로운 셀의 행</param>
		/// <param name="column">새로운 셀의 행</param>
		/// <param name="value">새로운 셀의 행</param>
		/// <returns>새로운 셀이 유효한지를 반환합니다.</returns>
		public bool IsNewCellValid(byte[][] sudokuBoard, byte row, byte column, byte value)
		{
			if (!SudokuUtils.IsSudokuBoardValid(sudokuBoard))
			{
				throw new ArgumentException(InvalidSudokuBoardMessage);
			}

			if ((row < 0 || row > 8) || (column < 0 || column > 8))
			{
				throw new IndexOutOfRangeException("Row and column values must be between 0 and 8!");
			}

			for (int i = 0; i < 9; i++)
			{
				// 행 검사
				if (sudokuBoard[row][i] == value && i != column)
				{
					return false;
				}

				// 열 검사
				if (sudokuBoard[i][column] == value && i != row)
				{
					return false;
				}

				// 3*3 보드 검사
				int groupRow = row / 3 * 3 + i / 3;
				int groupCol = column / 3 * 3 + i % 3;
				if (sudokuBoard[groupRow][groupCol] == value && (groupRow != row || groupCol != column))
				{
					return false;
				}
			}

			// 유효하다면
			return true;
		}

		/// <summary>
		/// 스도쿠 보드의 빈 셀을 스도쿠 기본 규칙에 따라 해결합니다.
		/// </summary>
		/// <param name="sudokuBoard">9*9 가변 배열</param>
		/// <returns>스도쿠가 해결이 가능한지를 반환합니다.</returns>
		public bool SolveSudoku(byte[][] sudokuBoard)
		{
			if (!SudokuUtils.IsSudokuBoardValid(sudokuBoard))
			{
				throw new ArgumentException(InvalidSudokuBoardMessage);
			}

			return SolveSudokuRec(sudokuBoard);
		}

		/// <summary>
		/// 백트래킹 알고리즘을 사용하여 스도쿠를 해결합니다.
		/// </summary>
		/// <param name="sudokuBoard">9*9 가변 배열</param>
		/// <returns>스도쿠가 해결이 가능한지를 반환합니다.</returns>
		private bool SolveSudokuRec(byte[][] sudokuBoard, int row = 0, int column = 0)
		{
			if (column == 9) // 열이 9라면
			{
				// 행의 마지막까지 왔다면 다음 행으로
				row++; // 행 1씩 증가
				column = 0; // 열을 0으로 초기화
				if (row == 9) // 행이 9가 되면
				{
					// 재귀 멈춤
					return true;
				}
			}

			// 셀에 어떤 숫자가 있으면 다음 셀로
			if (sudokuBoard[row][column] > 0)
			{
				return SolveSudokuRec(sudokuBoard, row, column + 1);
			}

			// 셀의 값 대입을 위한 알고리즘
			// 1부터 9까지 1씩 증가
			for (int cellValue = 1; cellValue <= 9; cellValue++)
			{
				// 셀이 기본 규칙을 따르고 있다!
				bool isCellValueValid = true;

				// 0부터 8까지 1씩 증가
				for (int i = 0; i < 9; i++)
				{
					if (sudokuBoard[row][i] == cellValue || // sudokuBoard[행][i] 가 cellValue와 같다
						sudokuBoard[i][column] == cellValue || // sudokuBoard[i][열] 이 cellValue와 같다
						sudokuBoard[row / 3 * 3 + i / 3][column / 3 * 3 + i % 3] == cellValue) // 축약: sudokuBoard[행 / 3 * 3 + i / 3][열 / 3 * 3 + i % 3] 이 cellValue와 같다
						// 위 3개의 조건을 모두 만족한다면...
					{
						// 셀이 기본 규칙을 따르고 있지 않다!
						isCellValueValid = false;
						break;
					}
				}

				if (!isCellValueValid) // 셀이 기본 규칙을 따르고 있지 않다면
				{
					// 다음 값으로
					continue;
				}

				// 값 저장
				sudokuBoard[row][column] = (byte)cellValue;

				// 다음 셀에도 알고리즘 적용, 반복
				if (this.SolveSudokuRec(sudokuBoard, row, column + 1))
				{
					return true;
				}

				// sudokuBoard[행][열]을 0으로 초기화
				sudokuBoard[row][column] = 0;
			}

			// 다음 줄에 알고리즘이 왔다면 답안은 없음, false 반환
			return false;
		}
	}
}
