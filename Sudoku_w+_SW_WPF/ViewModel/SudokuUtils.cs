using System;
using Cozyplanes.SudokuApp.Model;

namespace Cozyplanes.SudokuApp.ViewModel
{
	/// <summary>
	/// 스도쿠와 관련된 다른 클래스를 도와주는 매소드의 모음입니다. (Static 클래스)
	/// </summary>
	public static class SudokuUtils
	{
		private const string InvalidSudokuGridMessage = "스도쿠 보드 (Grid)는 9개의 요소가 반드시 필요합니다!";
		private const string InvalidSudokuBoardMessage = "스도쿠 보드는 반드시 9*9 가변 배열이여야 합니다!";

		/// <summary>
		/// 이 함수에 도달한 9*9 가변 배열이 인스턴스화 된 9*9 가변 배열인지 유효 확인 합니다.
		/// </summary>
		public static bool IsSudokuBoardValid(byte[][] sudokuBoard)
		{
			if (sudokuBoard == null || sudokuBoard.Length != 9)
			{
				return false;
			}

			for (int i = 0; i < 9; i++)
			{
				if (sudokuBoard[i] == null || sudokuBoard[i].Length != 9)
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// 스도쿠 보드 (Grid)를 한 곳 (sudokuGridFrom)에서 다른 곳 (sudokuGridTo)로 복사합니다.
		/// </summary>
		/// <param name="sudokuGridFrom">스도쿠 보드 (Grid) 복사의 시작점</param>
		/// <param name="sudokuGridTo">스도쿠 보드 (Grid) 복사의 끝점</param>
		public static void CopySudokuGrid(SudokuRow[] sudokuGridFrom, SudokuRow[] sudokuGridTo)
		{
			if (sudokuGridFrom == null || sudokuGridFrom.Length != 9 ||
				sudokuGridTo == null || sudokuGridTo.Length != 9)
			{
				throw new ArgumentException(InvalidSudokuGridMessage);
			}

			for (int i = 0; i < 9; i++)
			{
				sudokuGridTo[i] = new SudokuRow(sudokuGridFrom[i]);
			}
		}


		/// <summary>
		/// 스도쿠 보드에서 값이 있는 셀의 개수를 찾습니다.
		/// </summary>
		/// <param name="isFromPlayer">채워진 셀이 처음부터 채워졌는지, 플레이어가 채운 셀인지 bool값을 전달</param>
		/// <returns>값이 있는 셀의 개수</returns>
		public static int GetFilledSudokuCellsCount(SudokuRow[] sudokuGrid, bool isFromPlayer)
		{
			if (sudokuGrid == null || sudokuGrid.Length != 9)
			{
				throw new ArgumentException(InvalidSudokuGridMessage);
			}

			int filledCells = 0;
			for (int i = 0; i < 9; i++)
			{
				for (int j = 0; j < 9; j++)
				{
					if (sudokuGrid[i][j] != null &&
						sudokuGrid[i][j].Value != null)
					{
						if (isFromPlayer)
						{
							if (!sudokuGrid[i][j].IsReadOnly)
							{
								filledCells++;
							}
						}
						else
						{
							if (sudokuGrid[i][j].IsReadOnly)
							{
								filledCells++;
							}
						}
					}
				}
			}
			return filledCells;
		}

		/// <summary>
		/// SudokuRow 배열을 가변 바이트 배열로 전환합니다.
		/// </summary>
		public static byte[][] GenerateSudokuBoardFromGrid(SudokuRow[] sudokuGrid)
		{
			if (sudokuGrid == null || sudokuGrid.Length != 9)
			{
				throw new ArgumentException(InvalidSudokuGridMessage);
			}

			var sudokuBoard = new byte[9][];
			for (int i = 0; i < 9; i++)
			{
				sudokuBoard[i] = new byte[9];
				for (int j = 0; j < 9; j++)
				{
					if (sudokuGrid[i][j] != null && sudokuGrid[i][j].Value != null)
					{
						sudokuBoard[i][j] = (byte)sudokuGrid[i][j].Value;
					}
					else
					{
						sudokuBoard[i][j] = 0;
					}
				}
			}

			return sudokuBoard;
		}

		/// <summary>
		/// 가변 바이트 배열을 SudokuRow 배열로 전환합니다.
		/// </summary>
		public static SudokuRow[] GenerateSudokuGridFromBoard(byte[][] sudokuBoard)
		{
			if (!IsSudokuBoardValid(sudokuBoard))
			{
				throw new ArgumentException(InvalidSudokuBoardMessage);
			}

			var sudokuGrid = new SudokuRow[9];
			for (int i = 0; i < 9; i++)
			{
				sudokuGrid[i] = new SudokuRow();
				for (int j = 0; j < 9; j++)
				{
					if (sudokuBoard[i][j] == 0)
					{
						sudokuGrid[i][j] = new SudokuCell(null, false); ;
					}
					else
					{
						sudokuGrid[i][j] = new SudokuCell(sudokuBoard[i][j], true);
					}
				}
			}
			return sudokuGrid;
		}
	} 
}
