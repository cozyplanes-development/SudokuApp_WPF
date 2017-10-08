using System;
using Cozyplanes.SudokuApp.Model.Interfaces;
using Cozyplanes.SudokuApp.Model.Enums;

namespace Cozyplanes.SudokuApp.ViewModel
{
	/// <summary>
	/// 이 클래스는 답안이 있는 스도쿠를 플레이어가 플레이 할 수 있는 스도쿠로 바꾸는 기능을 가지고 있습니다.
	/// </summary>
	public class SudokuTransformer : ISudokuTransformer
	{
		// 빈칸의 개수를 여기서 수정할 수 있습니다
		// 다음 수는 보편적인 스도쿠의 난이도에서 빈칸의 평균 값입니다. 
		// 따라서 알고리즘과 전혀 관계가 없음을 알립니다.
		private const int CellsToEraseOnEasyDifficulty = 30; // Easy
		private const int CellsToEraseOnMediumDifficulty = 40; // Medium
		private const int CellsToEraseOnHardDifficulty = 45; // Hard
		private const int CellsToEraseOnImpossibleDifficulty = 55; // Impossible

		private readonly Random random;
		private const string InvalidSudokuBoardMessage = "스도쿠 보드는 반드시 9*9 가변 배열이여야만 합니다!";

		public SudokuTransformer()
		{
			random = new Random();
		}

		/// <summary>
		/// 유효한 스도쿠에서 난이도에 알맞게 셀을 삭제하여 플레이어가 해결할 수 있도록 합니다.
		/// </summary>
		/// <param name="sudokuBoard">9*9 가변 배열</param>
		public void EraseCells(byte[][] sudokuBoard, SudokuDifficultyType sudokuDifficulty)
		{
			if (!SudokuUtils.IsSudokuBoardValid(sudokuBoard))
			{
				throw new ArgumentException(InvalidSudokuBoardMessage);
			}

			// 지울 셀의 개수를 0으로 초기화
			int cellsToErase = 0; 

			// 지울 셀의 개수를 난이도에 따라 지정
			if (sudokuDifficulty == SudokuDifficultyType.Easy)
			{
				cellsToErase = CellsToEraseOnEasyDifficulty;
			}
			else if (sudokuDifficulty == SudokuDifficultyType.Medium)
			{
				cellsToErase = CellsToEraseOnMediumDifficulty;
			}
			else if (sudokuDifficulty == SudokuDifficultyType.Hard)
			{
				cellsToErase = CellsToEraseOnHardDifficulty;
			}
			else if (sudokuDifficulty == SudokuDifficultyType.Impossible)
			{
				cellsToErase = CellsToEraseOnImpossibleDifficulty;
			}
			else
			{
				cellsToErase = 35; // 에러 발생시 Easy와 Medium 평균값의 중간값인 35개의 셀을 지움
			}

			// 셀 지우는 방법:
			// 1. 행에서 시스템의 Random() 함수로 선택된 셀을 지운다.
			// 2. 반 대각선 (Minor Diagonal) 에 있는 셀을 지운다.
			// 3. 반복한다.
			while (cellsToErase > 0)
			{
				// 0 부터 8 까지의 행, 1씩 증가
				for (int row = 0; row < 9; row++)
				{
					// 최소 0, 최대 9 의 열 중 하나를 선택 (시스탬이 Random() 함수로 선택)
					int col = random.Next(0, 9);
					if (sudokuBoard[row][col] != 0) // 선택받은 (?) sudokuBoard[행][열] 이 0이 아니라면 (빈칸이 아니라면)
					{
						sudokuBoard[row][col] = 0; // 빈칸으로 전환
						cellsToErase--; // 지울 셀의 개수 - 1
					}

					if (cellsToErase <= 0) // 지울 셀의 개수가 0 이하가 되면
					{
						return; // 멈춤
					}

					int oppositeRow = 9 - col - 1; // 반대편 행 선택
					int oppositeCol = 9 - row - 1; // 반대편 열 선택
					if (sudokuBoard[oppositeRow][oppositeCol] != 0) // sudokuBoard[반대편 행][반대편 열] 이 0이 아니라면 (빈칸이 아니라면)
					{
						sudokuBoard[oppositeRow][oppositeCol] = 0; // 빈칸으로 전환
						cellsToErase--; // 지울 셀의 개수 - 1
					}

					if (cellsToErase <= 0) // 지울 셀의 개수가 0 이하가 되면
					{
						return; // 멈춤
					}
				}
			}
		}

		/// <summary>
		/// 섞으면서 유효한 스도쿠로 부터 다른 스도쿠를 생성합니다.
		/// </summary>
		/// <param name="sudokuBoard">9*9 가변 배열</param>
		public void ShuffleSudoku(byte[][] sudokuBoard)
		{
			if (!SudokuUtils.IsSudokuBoardValid(sudokuBoard))
			{
				throw new ArgumentException(InvalidSudokuBoardMessage);
			}

			// 최소 20번, 최대 31번 중의 값만큼 스도쿠를 변환
			int transformationsToPerform = random.Next(20, 31);

			// 시스템 Random() 함수로 선택된 값만큼 0부터 1씩 증가하며 For문을 돌린다.
			for (int transformationsCount = 0; transformationsCount < transformationsToPerform; transformationsCount++)
			{
				var transformationType = (SudokuBoardTransformationType)random.Next(0, 6);
				TransformSudokuBoard(sudokuBoard, transformationType);
			}
		}

		/// <summary>
		/// 스도쿠를 여러가지로 변환하되, 유효하도록 변환하는 함수입니다.
		/// </summary>
		/// <param name="sudokuBoard">9*9 가변 배열</param>
		private void TransformSudokuBoard(byte[][] sudokuBoard, SudokuBoardTransformationType type)
		{
			switch (type)
			{
				case SudokuBoardTransformationType.HorizontalIn9x3Group: // 9*3 의 가로 그룹

					// 각각의 행을 9*3 그룹에서 임의로 5번 교체
					for (int row = 0; row < 9; row += 3)
					{
						for (int timesToSwap = 0; timesToSwap < 5; timesToSwap++)
						{
							int rowToSwap = random.Next(row, row + 3);
							var tempRow = new byte[9];
							sudokuBoard[row].CopyTo(tempRow, 0);
							sudokuBoard[rowToSwap].CopyTo(sudokuBoard[row], 0);
							tempRow.CopyTo(sudokuBoard[rowToSwap], 0);
						}
					}
					break;

				case SudokuBoardTransformationType.HorizontalAroundFourthRowGroup: // 4번째 (가로) 행 주변 그룹

					// 각각의 행을 반대편에 있는 행과 교체									
					for (int row = 0; row < 4; row++)
					{
						int rowToSwap = 9 - row - 1;
						var tempRow = new byte[9];
						sudokuBoard[row].CopyTo(tempRow, 0);
						sudokuBoard[rowToSwap].CopyTo(sudokuBoard[row], 0);
						tempRow.CopyTo(sudokuBoard[rowToSwap], 0);
					}
					break;

				case SudokuBoardTransformationType.VerticalIn3x9Group: // 3*9 의 세로 그룹

					// 각각의 행을 9*3 그룹에서 임의로 5번 교체	
					for (int col = 0; col < 9; col += 3)
					{
						for (int timesToSwap = 0; timesToSwap < 5; timesToSwap++)
						{
							int colToSwap = random.Next(col, col + 3);
							byte tempValue = 0;
							for (int row = 0; row < 9; row++)
							{
								tempValue = sudokuBoard[row][col];
								sudokuBoard[row][col] = sudokuBoard[row][colToSwap];
								sudokuBoard[row][colToSwap] = tempValue;
							}
						}
					}
					break;

				case SudokuBoardTransformationType.VerticalAroundFourthColumnGroup: // 4번째 (세로) 열 주변 그룹

					// 각각의 행을 반대편에 있는 행과 교체
					for (int col = 0; col < 4; col++)
					{
						byte tempValue = 0;
						int colToSwap = 9 - col - 1;
						for (int row = 0; row < 9; row++)
						{
							tempValue = sudokuBoard[row][col];
							sudokuBoard[row][col] = sudokuBoard[row][colToSwap];
							sudokuBoard[row][colToSwap] = tempValue;
						}
					}
					break;

				case SudokuBoardTransformationType.AroundMainDiagonalGroup: // 주 대각선 주변 그룹
					
					// 주 대각선 (Major Diagonal) 위에 있는 각각의 셀을 반대편에 있는 셀과 교체
					for (int row = 0; row < 9; row++)
					{
						byte tempValue = 0;
						for (int col = 0; col < 9; col++)
						{
							tempValue = sudokuBoard[row][col];
							sudokuBoard[row][col] = sudokuBoard[col][row];
							sudokuBoard[col][row] = tempValue;
						}
					}
					break;

				case SudokuBoardTransformationType.AroundMinorDiagonalGroup: // 반 대각선 주변 그룹

					// 반 대각선 (Minor Diagonal) 위에 있는 각각의 셀을 반대편에 있는 셀과 교체
					for (int row = 0; row < 9; row++)
					{
						byte tempValue = 0;
						for (int col = 0; col < 9; col++)
						{
							tempValue = sudokuBoard[row][col];
							int oppositeRow = 9 - col - 1;
							int oppositeCol = 9 - row - 1;
							sudokuBoard[row][col] = sudokuBoard[oppositeRow][oppositeCol];
							sudokuBoard[oppositeRow][oppositeCol] = tempValue;
						}
					}
					break;

				case SudokuBoardTransformationType.Horizontal9x3Group: // 9*3 가로 그룹
					break;

				case SudokuBoardTransformationType.Vertical9x3Group: // 9*3 세로 그룹
					break;

				default: // 기본값
					break;
			}
		}
	}
}
