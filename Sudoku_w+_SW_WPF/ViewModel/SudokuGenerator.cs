using System;

using Cozyplanes.SudokuApp.Model.Interfaces;
using Cozyplanes.SudokuApp.Model.Enums;
using Cozyplanes.SudokuApp.Model;

namespace Cozyplanes.SudokuApp.ViewModel
{
	/// <summary>
	/// 이 클래스는 해결하기 위한 유효한 스도쿠 보드를 생성하는 기능을 가지고 있습니다.
	/// </summary>
	public class SudokuGenerator : ISudokuGenerator
	{
		private readonly ISudokuSolver sudokuSolver;
		private readonly ISudokuTransformer sudokuTransformer;
		private byte[][] generatedSudokuBoard;
		private byte[][] sudokuBoardForPlayer;

		public SudokuGenerator(ISudokuSolver sudokuSolver, ISudokuTransformer sudokuTransformer)
		{
			if (sudokuSolver == null || sudokuTransformer == null)
			{
				throw new ArgumentNullException("SudokuSolver or sudokuTransformer is null!");
			}

			this.sudokuSolver = sudokuSolver;
			this.sudokuTransformer = sudokuTransformer;

			generatedSudokuBoard = new byte[9][];
			for (int i = 0; i < 9; i++)
			{
				generatedSudokuBoard[i] = new byte[9];
			}

			// 이곳에서는 스도쿠 보드를 생성하되, 빈칸은 0으로 채워져 있습니다.
			// sudokuBoard[행][열]로 저장하여 나중에 저장된 이 byte[][] 배열을 참조하여 다른 스도쿠 보드를 생성합니다.
			// 또한 새롭게 생성된 스도쿠 보드를 또다시 섞어 다른 스도쿠 보드를 만듭니다.
			this.sudokuSolver.SolveSudoku(generatedSudokuBoard);
			sudokuBoardForPlayer = new byte[9][];

			for (int i = 0; i < 9; i++)
			{
				sudokuBoardForPlayer[i] = new byte[9];
			}
		}

		public SudokuGenerator() : this(new SudokuSolver(), new SudokuTransformer()) { }

		public byte[][] GeneratedSudokuBoard
		{
			get
			{
				return generatedSudokuBoard;
			}
		}

		public byte[][] SudokuBoardForPlayer
		{
			get
			{
				return sudokuBoardForPlayer;
			}
		}

		/// <summary>
		/// 해결하기 위한 유효한 스도쿠 보드를 생성합니다.
		/// </summary>
		/// <param name="sudokuDifficulty">생성된 스도쿠의 난이도</param>
		public SudokuRow[] GenerateSudoku(SudokuDifficultyType sudokuDifficulty)
		{
			sudokuTransformer.ShuffleSudoku(generatedSudokuBoard);

			for (int i = 0; i < 9; i++)
			{
				generatedSudokuBoard[i].CopyTo(sudokuBoardForPlayer[i], 0);
			}

			sudokuTransformer.EraseCells(sudokuBoardForPlayer, sudokuDifficulty);

			return SudokuUtils.GenerateSudokuGridFromBoard(sudokuBoardForPlayer);
		}
	}
}
