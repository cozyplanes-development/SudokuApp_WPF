using System;

namespace Cozyplanes.SudokuApp.Model.PlayerActions
{
	/// <summary>
	/// 전체 스도쿠 보드가 변경되는 플레이어의 액션을 나타냅니다.
	/// </summary>
	public abstract class ChangeSudokuGridCompletelyAction
	{
    private SudokuRow[] sudokuGridBeforeAction;

    public ChangeSudokuGridCompletelyAction(SudokuRow[] sudokuGrid)
    {
        this.SudokuGridBeforeAction = sudokuGrid;
    }

    public SudokuRow[] SudokuGridBeforeAction
    {
        get
        {
            return this.sudokuGridBeforeAction;
        }

        private set
        {
            if (value == null || value.Length != 9)
            {
                throw new ArgumentException("SudokuGrid must have nine elements!");
            }

            this.sudokuGridBeforeAction = value;
        }
    }
}
}
