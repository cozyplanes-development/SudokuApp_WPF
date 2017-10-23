using System;

using Cozyplanes.SudokuApp.Model.Interfaces;
using Cozyplanes.SudokuApp.Model.Enums;

namespace Cozyplanes.SudokuApp.Model.PlayerActions
{
    public class FillCellAction : IPlayerAction
    {
        private byte row;
        private byte column;
        private byte value;

		/// <summary>
		/// 셀을 채우는 액션입니다.
		/// </summary>
		/// <param name="row">새로운 셀의 행</param>
		/// <param name="column">새로운 셀의 열</param>
		/// <param name="value">새로운 셀의 값</param>
		public FillCellAction(byte row, byte column, byte value)
        {
            this.Row = row;
            this.Column = column;
            this.Value = value;
        }

        public byte Row
        {
            get
            {
                return this.row;
            }

            set
            {
                if (value < 0 || 8 < value)
                {
                    throw new ArgumentOutOfRangeException("PlayerDecision 행은 반드시 0과 8 사이 이여야 합니다!");
                }

                this.row = value;
            }
        }

        public byte Column
        {
            get
            {
                return this.column;
            }

            set
            {
                if (value < 0 || 8 < value)
                {
                    throw new ArgumentOutOfRangeException("PlayerDecision 열은 반드시 0과 8 사이 이여야 합니다!");
                }

                this.column = value;
            }
        }

        public byte Value
        {
            get
            {
                return this.value;
            }

            set
            {
                if (value < 1 || 9 < value)
                {
                    throw new ArgumentOutOfRangeException("PlayerDecision 값은 반드시 1과 9 사이 이여야 합니다!");
                }

                this.value = value;
            }
        }

        public PlayerActionType PlayerActionType
		{
            get
            {
                return PlayerActionType.FillCell;
            }
        }
    }
}
