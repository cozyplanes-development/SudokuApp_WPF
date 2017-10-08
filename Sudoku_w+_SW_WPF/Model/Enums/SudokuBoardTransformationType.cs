namespace Cozyplanes.SudokuApp.Model.Enums
{
	/// <summary>
	/// 스도쿠 보드가 변환될 때의 그룹을 각각 명시합니다.
	/// </summary>
    public enum SudokuBoardTransformationType
    {
        HorizontalIn9x3Group,            // 9*3 의 가로 그룹
        HorizontalAroundFourthRowGroup,  // 4번째 (가로) 행 주변 그룹
        VerticalIn3x9Group,              // 3*9 의 세로 그룹
		VerticalAroundFourthColumnGroup, // 4번째 (세로) 열 주변 그룹
		AroundMainDiagonalGroup,         // 주 대각선 주변 그룹
		AroundMinorDiagonalGroup,        // 반 대각선 주변 그룹
        Horizontal9x3Group,              // 9*3 가로 그룹
        Vertical9x3Group,                // 9*3 세로 그룹
    }
}
