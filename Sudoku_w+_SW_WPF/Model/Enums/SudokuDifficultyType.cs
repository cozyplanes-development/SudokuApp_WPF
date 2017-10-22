namespace Cozyplanes.SudokuApp.Model.Enums
{
	/// <summary>
	/// 스도쿠의 난이도의 종류를 명시합니다.
	/// </summary>
    public enum SudokuDifficultyType
    {
		데모, // 명시하되, SudokuTransformer.cs에서 else 예외로 처리
        Custom,
        Easy,
        Medium,
        Hard,
        Expert
    }
}
