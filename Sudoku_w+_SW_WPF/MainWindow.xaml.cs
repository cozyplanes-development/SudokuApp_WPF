using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

using Cozyplanes.SudokuApp.Enums;

namespace Cozyplanes.SudokuApp
{
    public partial class MainWindow : Window
	{ 
		private const string UnsolvableSudokuMessage = "The current sudoku is unsolvable! Try restarting or erasing some cells.";
		private const string PlayerSolvedSudokuMessage = "Congratulations, you solved it in {0} seconds! Try on harder difficulty  : )";
		private const string UnvalidSudokuCellAddedMessage = "The sudoku must be in a valid state to proceed.";

		private DispatcherTimer dispatcherTimer;
		private TimeSpan timerTimespan;

		public MainWindow()
		{
			InitializeComponent();
			this.DataContext = this;

			this.PrepareDispatcherTimer();
			this.RestartTimer();

			this.SudokuGrid.GenerateAndPopulateSudoku();
			this.SudokuGrid.SudokuSolved += new EventHandler(this.OnSudokuSolved);
			this.SudokuGrid.UnvalidCellValueAdded += new EventHandler(this.OnUnvalidCellValueAdded);
			this.SudokuGrid.UnvalidCellValueRemoved += new EventHandler(this.OnUnvalidCellValueRemoved);
		}

		public SudokuDifficultyType SelectedSudokuDifficulty
		{
			get
			{
				return this.SudokuGrid.SudokuDifficulty;
			}

			set
			{
				this.SudokuGrid.SudokuDifficulty = value;
			}
		}

		private void Button_Quit_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void Button_GenerateNew_Click(object sender, RoutedEventArgs e)
		{
			this.SudokuGrid.GenerateAndPopulateSudoku();

			this.RestartTimer();
			this.UpdateProgressBar();
			this.ClearMessage();
		}

		private void Button_Restart_Click(object sender, RoutedEventArgs e)
		{
			this.SudokuGrid.RestartSudoku();

			this.UpdateProgressBar();
			this.RestartTimer();
			this.ClearMessage();
		}

		private void Button_Hint_Click(object sender, RoutedEventArgs e)
		{
			bool existsHint = this.SudokuGrid.GetHint();
			if (!existsHint && this.ProgressBar_SudokuStatus.Value != 100)
			{
				this.ShowUnsolvableSudokuMessage();
			}
			else if (this.ProgressBar_SudokuStatus.Value != 100)
			{
				this.ClearMessage();
			}

			this.UpdateProgressBar();
		}

		private void Button_Solve_Click(object sender, RoutedEventArgs e)
		{
			bool isSolvable = this.SudokuGrid.SolveSudoku();
			if (!isSolvable)
			{
				this.ShowUnsolvableSudokuMessage();
			}

			this.UpdateProgressBar();
		}

		private void Button_Undo_Click(object sender, RoutedEventArgs e)
		{
			if (this.SudokuGrid.UndoPlayerAction())
			{
				this.UpdateProgressBar();
				this.ClearMessage();
			}
		}

		private void Button_Redo_Click(object sender, RoutedEventArgs e)
		{
			if (this.SudokuGrid.RedoPlayerAction())
			{
				this.UpdateProgressBar();
				this.ClearMessage();
			}
		}

		private void SudokuGrid_KeyUp(object sender, KeyEventArgs e)
		{
			this.UpdateProgressBar();
		}

		private void SudokuGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			this.UpdateProgressBar();
		}

		private void SudokuGrid_Loaded(object sender, RoutedEventArgs e)
		{
			this.UpdateProgressBar();
		}

		private void ComboBox_SudokuDifficulty_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.SudokuGrid.GenerateAndPopulateSudoku();

			this.RestartTimer();
			this.UpdateProgressBar();
			this.ClearMessage();
		}

		private void PrepareDispatcherTimer()
		{
			this.dispatcherTimer = new DispatcherTimer();
			this.dispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
			this.dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
		}

		private void DispatcherTimer_Tick(object sender, EventArgs e)
		{
			this.timerTimespan += TimeSpan.FromSeconds(1);
			this.label_Timer.Content = this.timerTimespan.ToString("hh\\:mm\\:ss");
		}

		private void OnSudokuSolved(object sender, EventArgs e)
		{
			this.dispatcherTimer.Stop();
			this.textBlock_Message.Foreground = Brushes.Green;
			this.textBlock_Message.Text = string.Format(PlayerSolvedSudokuMessage, this.timerTimespan.TotalSeconds);
		}

		private void OnUnvalidCellValueAdded(object sender, EventArgs e)
		{
			this.textBlock_Message.Foreground = Brushes.Red;
			this.textBlock_Message.Text = UnvalidSudokuCellAddedMessage;

			this.Button_Undo.IsEnabled = false;
			this.Button_Redo.IsEnabled = false;
			this.Button_Hint.IsEnabled = false;
			this.Button_Solve.IsEnabled = false;
		}

		private void OnUnvalidCellValueRemoved(object sender, EventArgs e)
		{
			this.textBlock_Message.Foreground = Brushes.Black;
			this.textBlock_Message.Text = "";

			this.Button_Undo.IsEnabled = true;
			this.Button_Redo.IsEnabled = true;
			this.Button_Hint.IsEnabled = true;
			this.Button_Solve.IsEnabled = true;
		}

		private void UpdateProgressBar()
		{
			this.ProgressBar_SudokuStatus.Value = this.SudokuGrid.GetProgress();
		}

		private void ClearMessage()
		{
			this.textBlock_Message.Text = "";
		}

		private void ShowUnsolvableSudokuMessage()
		{
			this.textBlock_Message.Foreground = Brushes.Red;
			this.textBlock_Message.Text = UnsolvableSudokuMessage;
		}

		private void RestartTimer()
		{
			this.timerTimespan = new TimeSpan();
			this.dispatcherTimer.Start();
			this.label_Timer.Content = this.timerTimespan.ToString("hh\\:mm\\:ss");
		}
	}
}
