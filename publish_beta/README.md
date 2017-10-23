# $ sudo { ku } 프로젝트 평가시 참고사항
----
프로젝트 (소스코드) 평가 전 반드시 읽어주세요.          
**Revision 02 (M10 22)**

# 프로그램 개발 환경
---
- **Compiler** : GNU GCC Compiler (MinGW) ver 4.8.2
- **IDE** : VS2017 version 15.4 Preview 2 (BETA)
- **Text Editor** : Notepad++ / VIM
- **OS** : Windows 10 Home

# 프로그램 실행시 환경 안내
----
- **권장 환경**  :        

    - Visual Studio 2015 Community 버전 이상
    - Windows 8 이상
    
- **최소 환경** :           

    - Visual Studio Code 또는 소스 코드를 확인할 수 있는 프로그램 (Notepad++)   
    - Windows 7 이상
    - 닷넷(`.NET`) 프레임워크 버전 4.6.1 이상

- `Windows XP/Vista`에 설치시 부가적인 닷넷(`.NET`) 프레임워크 설치가 필요합니다. [여기](https://www.microsoft.com/ko-KR/download/details.aspx?id=17851)에서 닷넷(`.NET`) 프레임워크 `4.x` 버전을 다운로드하세요. 테스팅하지 않았으므로 프로그램 실행시 발생하는 문제점에 대해서는 개발자는 책임지지 않습니다.

- `Unix` 계열 운영체제는 지원하지 않지만, `macOS`나 `Linux` (`Ubuntu`, `Debian`, `Fedora`) 인 경우에는 `Wine` 라이브러리을 사용하여 프로그램을 구동할 수 있습니다. [여기](https://www.winehq.org)를 참고하세요.



# 프로그램 소스 확인시 안내
---
본 프로그램은 거의 모든 함수와 알고리즘에 주석이 있습니다.   

주석 설명 :     
```
<summary>여기의 텍스트는 함수의 기능입니다.</summary>
<param name=매개변수 이름>매개변수에 대한 설명</param>     
<returns>반환값에 대한 설명</returns>
// C# 한 줄 주석
/*
    C# 여러
    줄 주석
*/
<!-- XAML 주석 -->
```


다음은 주석이 없는 경우입니다.

- `get;` `set;` 매소드만 있는 부분
- `Exception` 처리 부분
- `byte` 배열 처리 부분
- 단순한 `this` 구문 부분
- 반복적인 `switch` 구문 부분 


본 프로젝트의 용어 정의입니다.            

- 시스템 용어 번역
  - `Implementations` = 구현                    
  - `Property` = 속성               
  - `Instantiated` = 인스턴스화 된 ~            
  - `Array` = 배열                
  - `Byte` = 바이트            
  - `Value` = 값                 
  - `Elements` = 요소
<br/>

- 프로젝트 고유 용어 번역                
  - `Sudoku Grid` = 스도쿠 보드 (`Grid`)             
  - `Sudoku Board` = 스도쿠 보드                
  - `Valid` = 유효한                         
  - `Difficulty` = 난이도                 
  - `Generate` = 생성하다               
  - `Solve` = 풀다            
  - `Cell` = 셀            
  - `Jagged` = 가변            
  - `Shuffle` = 섞다            
  - `Represent` = 나타내다              
  - `Action` = 액션              
  - `Validate` = 유효 확인           
  - `Turn` = 전환               
                

# 프로젝트 파일 구성에 대한 안내
---------------------
- `Assets` : 이미지 (`.ico`) 파일
- `Model` : 프로그램의 로직
  - `Enums` : 게임에 필요한 각종 열거형
  - `Interfaces` : 인터페이스
  - `PlayerActions` : 플레이어가 취하는 액션
- `View` : 프로그램의 `UI`
- `ViewModel` : `UI`의 로직

# 간단한 프로그램 작동 원리에 대한 안내
---
1. 숫자를 랜덤 함수로 임의 지정
2. 규칙에 알맞는지 확인, 조정
3. `byte[행][열]` 에 저장
4. `Transformer`가 스도쿠 보드 섞기
5. 섞을 때도 계속해서 2번 반복
6. 모든 스도쿠 그룹 반복 완료시, 완성된 스도쿠를 `byte[행][열]`에 저장
7. 행렬의 주대각선 이론/난이도에 따라 빈칸 뚫기
8. `DataGrid`에 보드 보여주기
9. 힌트/해결 버튼 클릭할 때마다 규칙에 알맞는지 확인, 적절한 `Exception` 나타내기

## 라이선스 (GNU General Public License)
---------
```
    Sudoku Game in WPF made with C#
    
    Copyright (C) 2017 Sohn In Gi

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see http://www.gnu.org/licenses/ .
```

# Contact
---
프로젝트에 대한 문의사항은 <cozyplanes@gmail.com>로 이메일을 보내주세요.

Made with love in S.Korea by cozyplanes