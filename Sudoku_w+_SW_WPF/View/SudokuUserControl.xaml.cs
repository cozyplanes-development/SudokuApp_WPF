using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using Cozyplanes.SudokuApp.Model.Interfaces;
using Cozyplanes.SudokuApp.Model.Enums;
using Cozyplanes.SudokuApp.Model;
using Cozyplanes.SudokuApp.Model.PlayerActions;
using Cozyplanes.SudokuApp.ViewModel;

namespace Cozyplanes.SudokuApp
{
    public partial class SudokuUserControl
	{
		public event EventHandler SudokuSolved;
		public event EventHandler UnvalidCellValueAdded;
		public event EventHandler UnvalidCellValueRemoved;

		private readonly ISudokuGenerator sudokuGenerator;
		private readonly ISudokuSolver sudokuSolver;
		private SudokuRow[] initialSudokuGrid;
		private ObservableCollection<SudokuRow> currentSudokuGrid;
		private const int MaxFilledSudokuCellsCount = 9 * 9;
		private SudokuDifficultyType sudokuDifficulty = SudokuDifficultyType.Easy;
		private Stack<IPlayerAction> playerActions;
		private Stack<IPlayerAction> undonePlayerActions;

		public SudokuUserControl(ISudokuGenerator sudokuGenerator, ISudokuSolver sudokuSolver)
		{
			if (sudokuGenerator == null || sudokuSolver == null)
			{
				throw new ArgumentNullException("SudokuGenerator and/or sudokuSolver is null!");
			}

			this.sudokuGenerator = sudokuGenerator;
			this.sudokuSolver = sudokuSolver;
		}

		public SudokuUserControl() : this(new SudokuGenerator(), new SudokuSolver())
		{
			InitializeComponent();
		}

		public int InitiallyFilledSudokuCellsCount { get; private set; }

		public bool IsUnvalidCellValueAdded { get; private set; }

		/// <summary>
        	/// 스도쿠 난이도
        	/// </summary>
		public SudokuDifficultyType SudokuDifficulty
		{
			get
			{
				return this.sudokuDifficulty;
			}

			set
			{
				this.sudokuDifficulty = value;
			}
		}


		
		/// <summary>
		/// 플레이어가 채운 셀을 기반으로 스도쿠의 현재 진행 상황을 반환합니다.
		/// </summary>
		public double GetProgress()
		{
			if (this.InitiallyFilledSudokuCellsCount == MaxFilledSudokuCellsCount)
			{
				return 100;
			}

			int playerFilledCells = SudokuUtils.GetFilledSudokuCellsCount(this.currentSudokuGrid.ToArray(), true);

			double progress = playerFilledCells / (double)(MaxFilledSudokuCellsCount - this.InitiallyFilledSudokuCellsCount) * 100;

			if (progress == 100)
			{
				this.SudokuSolved?.Invoke(this, new EventArgs());
			}

			return progress;
		}

		/// <summary>
		/// 스도쿠 보드 (Grid)에 새로운 스도쿠를 채웁니다.
		/// </summary>
		public void GenerateAndPopulateSudoku()
		{
			var newSudokuGrid = this.sudokuGenerator.GenerateSudoku(this.SudokuDifficulty);
			this.initialSudokuGrid = new SudokuRow[9];
			SudokuUtils.CopySudokuGrid(newSudokuGrid, this.initialSudokuGrid);
			this.InitiallyFilledSudokuCellsCount = SudokuUtils.GetFilledSudokuCellsCount(this.initialSudokuGrid, false);

			this.UpdateSudokuGridItems(newSudokuGrid);
			this.IsUnvalidCellValueAdded = false;

			this.playerActions = new Stack<IPlayerAction>();
			this.undonePlayerActions = new Stack<IPlayerAction>();
		}

		/// <summary>
		/// 스도쿠 보드 (Grid)에 마지막으로 생성된 스도쿠를 채웁니다.
		/// </summary>
		public void RestartSudoku()
		{
			this.playerActions.Push(new RestartAction(this.currentSudokuGrid.ToArray()));

			var restartedSudokuGrid = new SudokuRow[9];
			SudokuUtils.CopySudokuGrid(this.initialSudokuGrid, restartedSudokuGrid);

			this.UpdateSudokuGridItems(restartedSudokuGrid);
			this.IsUnvalidCellValueAdded = false;

			this.InitiallyFilledSudokuCellsCount = SudokuUtils.GetFilledSudokuCellsCount(this.initialSudokuGrid, false);
		}

		/// <summary>
		/// 스도쿠 보드 (Grid)가 유효하면 스도쿠 보드 (Grid)의 첫 번째 빈 셀을 채웁니다.
		/// </summary>
		/// <returns>스도쿠를 해결할 수 있는지를 반환합니다.</returns>
		public bool GetHint()
		{
			if (this.IsUnvalidCellValueAdded)
			{
				return false;
			}

			// 첫 번째 빈 스도쿠 셀을 찾고 스도쿠를 해결합니다.
// 스도쿠가 해결 될 수 있으면, 해결된 스도쿠의 값으로 셀을 채우고 true를 반환합니다. 
// 그렇지 않은 경우 false를 반환합니다.
			for (byte row = 0; row < 9; row++)
			{
				for (byte col = 0; col < 9; col++)
				{
					if (this.currentSudokuGrid[row][col].Value == null)
					{
						var solvedSudokuGrid = this.SolveSudoku(this.currentSudokuGrid.ToArray());
						if (solvedSudokuGrid != null)
						{
							this.playerActions.Push(new HintAction(row, col, (byte)solvedSudokuGrid[row][col].Value));

							this.currentSudokuGrid[row][col].Value = solvedSudokuGrid[row][col].Value;
							this.RefreshSudokuGridItems();

							return true;
						}
						else
						{
							return false;
						}
					}
				}
			}

			return false;
		}

		/// <summary>
		/// If the SudokuGrid is valid, solves the current SudokuGrid and populates it.
		/// </summary>
		/// <returns>Whether the sudoku is solvable.</returns>
		public bool SolveSudoku()
		{
			if (this.IsUnvalidCellValueAdded)
			{
				return false;
			}

			var solvedSudokuGrid = this.SolveSudoku(this.currentSudokuGrid.ToArray());
			if (solvedSudokuGrid != null)
			{
				this.playerActions.Push(new SolveAction(this.currentSudokuGrid.ToArray()));

				this.UpdateSudokuGridItems(solvedSudokuGrid);
				this.InitiallyFilledSudokuCellsCount = 81;
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Undoes the last action the player has made. 
		/// When a new sudoku is generated, the player actions are restarted.
		/// </summary>
		/// <returns>Whether there is an available player action to undo.</returns>
		public bool UndoPlayerAction()
		{
			if (this.IsUnvalidCellValueAdded)
			{
				return false;
			}

			if (this.playerActions.Count > 0)
			{
				var playerAction = this.playerActions.Pop();

				if (playerAction.PlayerActionType == PlayerActionType.FillCell)
				{
					this.undonePlayerActions.Push(playerAction);

					var fillCellDec = playerAction as FillCellAction;
					this.currentSudokuGrid[fillCellDec.Row][fillCellDec.Column].Value = null;
					this.RefreshSudokuGridItems();
				}
				else if (playerAction.PlayerActionType == PlayerActionType.Restart)
				{
					this.undonePlayerActions.Push(new RestartAction(this.currentSudokuGrid.ToArray()));

					var restartDec = playerAction as RestartAction;
					this.UpdateSudokuGridItems(restartDec.SudokuGridBeforeAction);
				}
				else if (playerAction.PlayerActionType == PlayerActionType.Solve)
				{
					this.undonePlayerActions.Push(new SolveAction(this.currentSudokuGrid.ToArray()));

					var solveDec = playerAction as SolveAction;
					this.UpdateSudokuGridItems(solveDec.SudokuGridBeforeAction);

					this.InitiallyFilledSudokuCellsCount = SudokuUtils.GetFilledSudokuCellsCount(this.initialSudokuGrid, false);
				}

				return true;
			}

			return false;
		}

		/// <summary>
		/// Redoes the last undone player action. 
		/// When a new sudoku is generated, the player actions are restarted.
		/// </summary>
		/// <returns>Whether there is an available player action to redo.</returns>
		public bool RedoPlayerAction()
		{
			if (this.IsUnvalidCellValueAdded)
			{
				return false;
			}

			if (this.undonePlayerActions.Count > 0)
			{
				var playerAction = this.undonePlayerActions.Pop();

				if (playerAction.PlayerActionType == PlayerActionType.FillCell)
				{
					this.playerActions.Push(playerAction);

					var fillCellDec = playerAction as FillCellAction;
					this.currentSudokuGrid[fillCellDec.Row][fillCellDec.Column].Value = fillCellDec.Value;
					this.RefreshSudokuGridItems();
				}
				else if (playerAction.PlayerActionType == PlayerActionType.Restart)
				{
					this.RestartSudoku();
				}
				else if (playerAction.PlayerActionType == PlayerActionType.Solve)
				{
					this.SolveSudoku();
				}

				return true;
			}

			return false;
		}

		/// <summary>
		/// If the cell being edited is initially filled, it doesn't allow the player to change it.
		/// </summary>
		private void DataGridSudoku_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
		{
			if (this.IsUnvalidCellValueAdded)
			{
				e.Cancel = true;
				return;
			}

			var cell = (e.Row.DataContext as SudokuRow)[e.Column.DisplayIndex];
			if (cell != null && cell.IsReadOnly)
			{
				e.Cancel = true;
			}
		}

		/// <summary>
		/// Validates the value the player wants to save in the cell.
		/// </summary>
		private void DataGridSudoku_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
		{
			var cell = e.EditingElement as TextBox;
			byte cellValue = 0;

			var row = (byte)e.Row.GetIndex();
			var col = (byte)e.Column.DisplayIndex;

			if (cell.Text != string.Empty)
			{
				bool isCellValueValid = false;
				if (byte.TryParse(cell.Text, out cellValue))
				{
					if (0 < cellValue && cellValue < 10)
					{
						var sudokuBoard = SudokuUtils.GenerateSudokuBoardFromGrid(this.currentSudokuGrid.ToArray());
						if (this.sudokuSolver.IsNewCellValid(sudokuBoard, row, col, cellValue))
						{
							isCellValueValid = true;
						}

						this.playerActions.Push(new FillCellAction(row, col, cellValue));
					}
				}

				if (isCellValueValid)
				{
					if (this.IsUnvalidCellValueAdded)
					{
						this.IsUnvalidCellValueAdded = false;
						this.UnvalidCellValueRemoved?.Invoke(sender, e);
					}
				}
				else
				{
					this.IsUnvalidCellValueAdded = true;
					this.UnvalidCellValueAdded?.Invoke(sender, e);

					e.Cancel = true;
					cell.BorderBrush = Brushes.Red;
					cell.BorderThickness = new Thickness(2);
				}
			}
			else
			{
				this.IsUnvalidCellValueAdded = false;
				this.UnvalidCellValueRemoved?.Invoke(sender, e);

				e.Cancel = true;
				this.currentSudokuGrid[row][col].Value = null;
				cell.BorderBrush = Brushes.Black;
				cell.BorderThickness = new Thickness(0);
			}
		}

		private void UpdateSudokuGridItems(SudokuRow[] sudokuGrid)
		{
			this.currentSudokuGrid = new ObservableCollection<SudokuRow>(sudokuGrid);
			this.DataGridSudoku.ItemsSource = this.currentSudokuGrid;
		}

		private void RefreshSudokuGridItems()
		{
			try
			{
				this.DataGridSudoku.Items.Refresh();
			}
			catch (InvalidOperationException)
			{
				// this is here to prevent the application from crashing 
				// if the sudoku DataGrid is unvalid 
				// or there is an operation being performed on the grid while refreshing items
			}
		}

		private SudokuRow[] SolveSudoku(SudokuRow[] sudokuGrid)
		{
			var sudokuBoard = SudokuUtils.GenerateSudokuBoardFromGrid(sudokuGrid);
			bool isSolvable = this.sudokuSolver.SolveSudoku(sudokuBoard);
			if (isSolvable)
			{
				var solvedSudokuGrid = SudokuUtils.GenerateSudokuGridFromBoard(sudokuBoard);
				return solvedSudokuGrid;
			}
			else
			{
				return null;
			}
		}
	}
}
