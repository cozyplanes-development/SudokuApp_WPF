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
using System.Windows.Input;

namespace Cozyplanes.SudokuApp
{
    public partial class SudokuUserControl : UserControl
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
                return sudokuDifficulty;
            }

            set
            {
                sudokuDifficulty = value;
            }
        }



        /// <summary>
        /// 플레이어가 채운 셀을 기반으로 스도쿠의 현재 진행 상황을 반환합니다.
        /// </summary>
        public double GetProgress()
        {
            if (InitiallyFilledSudokuCellsCount == MaxFilledSudokuCellsCount)
            {
                return 100;
            }

            int playerFilledCells = SudokuUtils.GetFilledSudokuCellsCount(currentSudokuGrid.ToArray(), true);

            double progress = playerFilledCells / (double)(MaxFilledSudokuCellsCount - InitiallyFilledSudokuCellsCount) * 100;

            if (progress == 100)
            {
                SudokuSolved?.Invoke(this, new EventArgs());
            }

            return progress;
        }

        /// <summary>
        /// 스도쿠 보드 (Grid)에 새로운 스도쿠를 채웁니다.
        /// </summary>
        public void GenerateAndPopulateSudoku()
        {
            var newSudokuGrid = sudokuGenerator.GenerateSudoku(SudokuDifficulty);
            initialSudokuGrid = new SudokuRow[9];
            SudokuUtils.CopySudokuGrid(newSudokuGrid, initialSudokuGrid);
            InitiallyFilledSudokuCellsCount = SudokuUtils.GetFilledSudokuCellsCount(initialSudokuGrid, false);

            UpdateSudokuGridItems(newSudokuGrid);
            IsUnvalidCellValueAdded = false;

            playerActions = new Stack<IPlayerAction>();
            undonePlayerActions = new Stack<IPlayerAction>();
        }

        /// <summary>
        /// 스도쿠 보드 (Grid)에 마지막으로 생성된 스도쿠를 채웁니다.
        /// </summary>
        public void RestartSudoku()
        {
            playerActions.Push(new RestartAction(currentSudokuGrid.ToArray()));

            var restartedSudokuGrid = new SudokuRow[9];
            SudokuUtils.CopySudokuGrid(initialSudokuGrid, restartedSudokuGrid);

            UpdateSudokuGridItems(restartedSudokuGrid);
            IsUnvalidCellValueAdded = false;

            InitiallyFilledSudokuCellsCount = SudokuUtils.GetFilledSudokuCellsCount(initialSudokuGrid, false);
        }

        /// <summary>
        /// 스도쿠 보드 (Grid)가 유효하면 스도쿠 보드 (Grid)의 첫 번째 빈 셀을 채웁니다.
        /// </summary>
        /// <returns>스도쿠를 해결할 수 있는지를 반환합니다.</returns>
        public bool GetHint()
        {
            if (IsUnvalidCellValueAdded)
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
                    if (currentSudokuGrid[row][col].Value == null)
                    {
                        var solvedSudokuGrid = SolveSudoku(currentSudokuGrid.ToArray());
                        if (solvedSudokuGrid != null)
                        {
                            playerActions.Push(new HintAction(row, col, (byte)solvedSudokuGrid[row][col].Value));

                            currentSudokuGrid[row][col].Value = solvedSudokuGrid[row][col].Value;
                            RefreshSudokuGridItems();

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
        /// 스도쿠 보드 (Grid) 가 유효하면 현재 스도쿠 보드 (Grid) 를 해결하고 셀을 채웁니다.
        /// </summary>
        /// <returns>스도쿠를 해결할 수 있는지를 반환합니다.</returns>
        public bool SolveSudoku()
        {
            if (IsUnvalidCellValueAdded)
            {
                return false;
            }

            var solvedSudokuGrid = SolveSudoku(currentSudokuGrid.ToArray());
            if (solvedSudokuGrid != null)
            {
                playerActions.Push(new SolveAction(currentSudokuGrid.ToArray()));

                UpdateSudokuGridItems(solvedSudokuGrid);
                InitiallyFilledSudokuCellsCount = 81;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
		/// 플레이어가 마지막으로 수행한 동작을 취소합니다. 
		/// 새로운 스도쿠가 생성되면 플레이어 액션이 다시 시작됩니다.
        /// </summary>
        /// <returns>취소할 플레이어 액션이 있는지를 반환합니다.</returns>
        public bool UndoPlayerAction()
        {
            if (IsUnvalidCellValueAdded)
            {
                return false;
            }

            if (playerActions.Count > 0)
            {
                var playerAction = playerActions.Pop();

                if (playerAction.PlayerActionType == PlayerActionType.FillCell)
                {
                    undonePlayerActions.Push(playerAction);

                    var fillCellDec = playerAction as FillCellAction;
                    currentSudokuGrid[fillCellDec.Row][fillCellDec.Column].Value = null;
                    RefreshSudokuGridItems();
                }
                else if (playerAction.PlayerActionType == PlayerActionType.Restart)
                {
                    undonePlayerActions.Push(new RestartAction(currentSudokuGrid.ToArray()));

                    var restartDec = playerAction as RestartAction;
                    UpdateSudokuGridItems(restartDec.SudokuGridBeforeAction);
                }
                else if (playerAction.PlayerActionType == PlayerActionType.Solve)
                {
                    undonePlayerActions.Push(new SolveAction(currentSudokuGrid.ToArray()));

                    var solveDec = playerAction as SolveAction;
                    UpdateSudokuGridItems(solveDec.SudokuGridBeforeAction);

                    InitiallyFilledSudokuCellsCount = SudokuUtils.GetFilledSudokuCellsCount(initialSudokuGrid, false);
                }

                return true;
            }

            return false;
        }

		/// <summary>
		/// 마지막으로 취소된 플레이어 액션을 다시 실행합니다.
		/// 새로운 스도쿠가 생성되면 플레이어 동작이 재시작 됩니다.
		/// </summary>
		/// <returns>취소할 플레이어 액션이 있는지를 반환합니다.</returns>
		public bool RedoPlayerAction()
        {
            if (IsUnvalidCellValueAdded)
            {
                return false;
            }

            if (this.undonePlayerActions.Count > 0)
            {
                var playerAction = undonePlayerActions.Pop();

                if (playerAction.PlayerActionType == PlayerActionType.FillCell)
                {
                    playerActions.Push(playerAction);

                    var fillCellDec = playerAction as FillCellAction;
                    currentSudokuGrid[fillCellDec.Row][fillCellDec.Column].Value = fillCellDec.Value;
                    RefreshSudokuGridItems();
                }
                else if (playerAction.PlayerActionType == PlayerActionType.Restart)
                {
                    RestartSudoku();
                }
                else if (playerAction.PlayerActionType == PlayerActionType.Solve)
                {
                    SolveSudoku();
                }

                return true;
            }

            return false;
        }

        /// <summary>
		/// 편집되고 있는 셀이 미리 채워졌다면, 플레이어가 수정하는 액션을 막습니다.
        /// </summary>
        private void DataGridSudoku_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (IsUnvalidCellValueAdded)
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
		/// 플레이어가 셀이 저장하려는 값의 유효성을 검사합니다.
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
                        var sudokuBoard = SudokuUtils.GenerateSudokuBoardFromGrid(currentSudokuGrid.ToArray());
                        if (sudokuSolver.IsNewCellValid(sudokuBoard, row, col, cellValue))
                        {
                            isCellValueValid = true;
                        }

                        playerActions.Push(new FillCellAction(row, col, cellValue));
                    }
                }

                if (isCellValueValid)
                {
                    if (IsUnvalidCellValueAdded)
                    {
                        IsUnvalidCellValueAdded = false;
                        UnvalidCellValueRemoved?.Invoke(sender, e);
                    }
                }
                else
                {
                    IsUnvalidCellValueAdded = true;
                    UnvalidCellValueAdded?.Invoke(sender, e);

                    e.Cancel = true;
                    cell.BorderBrush = Brushes.Red;
                    cell.BorderThickness = new Thickness(2);
                }
            }
            else
            {
                IsUnvalidCellValueAdded = false;
                UnvalidCellValueRemoved?.Invoke(sender, e);

                e.Cancel = true;
                currentSudokuGrid[row][col].Value = null;
                cell.BorderBrush = Brushes.Black;
                cell.BorderThickness = new Thickness(0);
            }
        }

        private void UpdateSudokuGridItems(SudokuRow[] sudokuGrid)
        {
            currentSudokuGrid = new ObservableCollection<SudokuRow>(sudokuGrid);
            DataGridSudoku.ItemsSource = currentSudokuGrid;
        }

        //private void DataGridSudoku_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (DataGridSudoku.SelectedIndex == 1)
        //    {
        //        DataGridSudoku.CancelEdit(DataGridEditingUnit.Row);
        //    }
        //}
        //void DataGridSudoku_Unloaded(object sender, RoutedEventArgs e)
        //{
        //    var grid = (DataGrid)sender;
        //    grid.CommitEdit(DataGridEditingUnit.Row, true);
        //}
        private void RefreshSudokuGridItems()
        {
            //try
            //{
                DataGridSudoku.Items.Refresh();
                //DataGridSudoku.Items.Refresh(); // Exception
            //}
            //catch (InvalidOperationException)
            //{
                //DataGridSudoku.Items.Refresh();
                //  스도쿠 DataGrid가 유효하지 않거나
                // 값을 새로고침할때 스도쿠 보드 (Grid)에 다른 조작이 있을 때
                // 어플리케이션이 중지되는 것을 막기 위한 대응책입니다.
            //}
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

        void DataGridSudoku_OnPreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            if (e.EditingElement is TextBox textBox)
            {
                textBox.PreviewTextInput -= HandlePreviewTextInput;
                textBox.PreviewTextInput += HandlePreviewTextInput;
            }
        }

        void HandlePreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out var numericValue)
                || numericValue < 0 || numericValue > 9)
            {
                e.Handled = true;
            }
        }
    }
}
