using System;

namespace Cozyplanes.SudokuApp.Model.PlayerActions
{
    /// <summary>
    /// Represents a player decision where the whole sudoku grid changes.
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
