using MahApps.Metro.Controls;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

using Cozyplanes.SudokuApp.Model.Enums;

namespace Cozyplanes.SudokuApp
{
    public partial class MainWindow : MetroWindow
	{
        private const string InvalidOperationMessage = "Error : Internal\n재시작 해주세요.";
        private const string UnsolvableSudokuMessage = "현재 상태의 스도쿠는 해결할 수 없습니다! 재시작하거나 몇몇 셀을 지워보세요.";
		private const string PlayerSolvedSudokuMessage = "축하드립니다! {0} 초 만에 해결하셨군요! 또다른 게임을 플레이해 보거나 더 어려운 난이도를 플레이 해 보세요! : )";
		private const string UnvalidSudokuCellAddedMessage = "스도쿠는 유효한 상태 (답안이 있는 상태) 이어야만 진행하실 수 있습니다. 재시작하거나 몇몇 셀을 지워보세요.";

		private DispatcherTimer dispatcherTimer;
		private TimeSpan timerTimespan;

		/// <summary>
        /// 초기 설정
        /// </summary>
		public MainWindow()
		{
			InitializeComponent();
			DataContext = this;

			PrepareDispatcherTimer();
			RestartTimer();

			SudokuGrid.GenerateAndPopulateSudoku();
			SudokuGrid.SudokuSolved += new EventHandler(OnSudokuSolved);
			SudokuGrid.UnvalidCellValueAdded += new EventHandler(OnUnvalidCellValueAdded);
			SudokuGrid.UnvalidCellValueRemoved += new EventHandler(OnUnvalidCellValueRemoved);
		}

		/// <summary>
        /// 스도쿠 난이도
        /// </summary>
        public SudokuDifficultyType SelectedSudokuDifficulty
		{
			get
			{
				return SudokuGrid.SudokuDifficulty;
			}

			set
			{
				SudokuGrid.SudokuDifficulty = value;
			}
		}


        #region UI 핸들링
        private void Button_Quit_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void Button_GenerateNew_Click(object sender, RoutedEventArgs e)
		{
			SudokuGrid.GenerateAndPopulateSudoku();

			RestartTimer();
			UpdateProgressBar();
			ClearMessage();
		}

		private void Button_Restart_Click(object sender, RoutedEventArgs e)
		{
			SudokuGrid.RestartSudoku();

			UpdateProgressBar();
			RestartTimer();
			ClearMessage();
		}

		private void Button_Hint_Click(object sender, RoutedEventArgs e)
		{
			bool existsHint = SudokuGrid.GetHint();
			if (!existsHint && ProgressBar_SudokuStatus.Value != 100)
			{
				ShowUnsolvableSudokuMessage();
			}
			else if (ProgressBar_SudokuStatus.Value != 100)
			{
				ClearMessage();
			}

			UpdateProgressBar();
		}

		private void Button_Solve_Click(object sender, RoutedEventArgs e)
		{
			bool isSolvable = SudokuGrid.SolveSudoku();
			if (!isSolvable)
			{
				ShowUnsolvableSudokuMessage();
			}

			UpdateProgressBar();
		}

		private void Button_Undo_Click(object sender, RoutedEventArgs e)
		{
			if (this.SudokuGrid.UndoPlayerAction())
			{
				UpdateProgressBar();
				ClearMessage();
			}
		}

		private void Button_Redo_Click(object sender, RoutedEventArgs e)
		{
			if (SudokuGrid.RedoPlayerAction())
			{
				UpdateProgressBar();
				ClearMessage();
			}
		}

		private void SudokuGrid_KeyUp(object sender, KeyEventArgs e)
		{
			UpdateProgressBar();
		}

		private void SudokuGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			UpdateProgressBar();
		}

		private void SudokuGrid_Loaded(object sender, RoutedEventArgs e)
		{
			UpdateProgressBar();
		}

		private void ComboBox_SudokuDifficulty_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { 
			SudokuGrid.GenerateAndPopulateSudoku();

			RestartTimer();
			UpdateProgressBar();
			ClearMessage();
		}

		private void PrepareDispatcherTimer()
		{
			dispatcherTimer = new DispatcherTimer();
			dispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
			dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
		}

		private void DispatcherTimer_Tick(object sender, EventArgs e)
		{
			timerTimespan += TimeSpan.FromSeconds(1);
			label_Timer.Content = timerTimespan.ToString("hh\\:mm\\:ss");
		}

		private void OnSudokuSolved(object sender, EventArgs e)
		{
			dispatcherTimer.Stop();
			textBlock_Message.Foreground = Brushes.Green; // 게임 완료시 메세지 색깔
			textBlock_Message.Text = string.Format(PlayerSolvedSudokuMessage, timerTimespan.TotalSeconds);
		}

		private void OnUnvalidCellValueAdded(object sender, EventArgs e)
		{
			textBlock_Message.Foreground = Brushes.Red; // 오류시 셀의 테두리 색깔
			textBlock_Message.Text = UnvalidSudokuCellAddedMessage;

			Button_Undo.IsEnabled = false;
			Button_Redo.IsEnabled = false;
			Button_Hint.IsEnabled = false;
			Button_Solve.IsEnabled = false;
		}

		private void OnUnvalidCellValueRemoved(object sender, EventArgs e)
		{
			textBlock_Message.Foreground = Brushes.Black; // 기본 테두리 색깔
			textBlock_Message.Text = "";

			Button_Undo.IsEnabled = true;
			Button_Redo.IsEnabled = true;
			Button_Hint.IsEnabled = true;
			Button_Solve.IsEnabled = true;
		}

		private void UpdateProgressBar()
		{
			ProgressBar_SudokuStatus.Value = SudokuGrid.GetProgress();
		}

		private void ClearMessage()
		{
			textBlock_Message.Text = "";
		}

		private void ShowUnsolvableSudokuMessage()
		{
			textBlock_Message.Foreground = Brushes.Red; // 오류시 메세지 색깔
			textBlock_Message.Text = UnsolvableSudokuMessage;
		}

		private void RestartTimer()
		{
			timerTimespan = new TimeSpan();
			dispatcherTimer.Start();
			label_Timer.Content = timerTimespan.ToString("hh\\:mm\\:ss");
		}
        #endregion
    }
}
