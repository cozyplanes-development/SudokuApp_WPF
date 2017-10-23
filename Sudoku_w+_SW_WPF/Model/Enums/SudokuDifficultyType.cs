namespace Cozyplanes.SudokuApp.Model.Enums
{
	/// <summary>
	/// 스도쿠의 난이도의 종류를 명시합니다.
	/// </summary>
    public enum SudokuDifficultyType
    {
        Custom, // 빈칸 수 : 81
        데모, // 빈칸 수 : 10
        Easy, // 빈칸 수 : 30
        Medium, // 빈칸 수 : 40
        Hard, // 빈칸 수 : 45
        Expert // 빈칸 수 : 55
        // 난이도 이곳에서 Enum 으로 추가, ViewModel/SudokuTransformer.cs 에서 난이도 명시하지 않을 경우, 빈칸 20개 (else문으로 처리)
    }
}
