using System;

namespace Cozyplanes.SudokuApp.Model
{
	public class SudokuCell
	{
		private byte? value;

		/// <summary>
		/// 스도쿠의 현재 상태
		/// </summary>
		/// <param name="sudokuCell">스도쿠의 셀</param>
		public SudokuCell(SudokuCell sudokuCell)
		{
			if (sudokuCell != null)
			{
				Value = sudokuCell.Value;
				IsReadOnly = sudokuCell.IsReadOnly;
			}
		}

		public SudokuCell(byte? value, bool isReadOnly)
		{
			Value = value;
			IsReadOnly = isReadOnly;
		}

		public byte? Value
		{
			get
			{
				return value;
			}
			set
			{
				if (value != null && (value < 1 || 9 < value))
				{
					throw new ArgumentOutOfRangeException("SudokuCell value must be between 1 and 9.");
				}

				this.value = value;
			}
		}

		public bool IsReadOnly { get; set; }
	}
}
