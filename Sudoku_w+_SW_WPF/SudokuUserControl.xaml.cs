using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using Cozyplanes.SudokuApp.Interfaces;
using Cozyplanes.SudokuApp.Enums;
using Cozyplanes.SudokuApp.Model;
using Cozyplanes.SudokuApp.Model.PlayerActions;

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
		/// Returns the current progress of the sudoku based on the player filled cells.
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
		/// Populates the SudokuGrid with a new sudoku.
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
		/// Populates the SudokuGrid with the last generated sudoku.
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
		/// If the SudokuGrid is valid, fills the first empty cell of the SudokuGrid.
		/// </summary>
		/// <returns>Whether the sudoku is solvable.</returns>
		public bool GetHint()
		{
			if (this.IsUnvalidCellValueAdded)
			{
				return false;
			}

			// Finds the first empty SudokuCell and tries to solve the sudoku.
			// If the sudoku is solvable, fills the cell with the value from the solved sudoku and returns true.
			// If not, returns false.
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
