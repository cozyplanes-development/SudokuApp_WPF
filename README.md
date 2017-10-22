**IMPORTANT! DUE (TUESDAY M10 24 23:59 KST). MUST SEND PROJECT FILE VIA EMAIL**
**MAKE NEW PROJECT + DELETE UNNECESSARY REFS**
**INVESTIGATE INNO SETUP INSTALLER**

## SudokuApp_WPF_cozyplanes
Sudoku made with WPF in C#. (final-year project)
See README.md in publish_beta for installing and checking options.

### Program name
**`$ sudo { ku }`**

### ISSUES
- (Bug) Level sometimes repeats (AP001) **FIXED**
- (Feature) Hint in places where user wants (FR001) **WILL NOT IMPLEMENT**
- (Feature) Input Pad to eliminate sudoku board number errors (FR002) **WILL NOT IMPLEMENT**
- (Bug) Hint comes from top to right, then goes downwards, needs to appear random or implement FR001 (AP002) **WILL NOT IMPLEMENT**
- (Bug) Exception in `View/SudokuUserControl.xaml.cs` - `RefreshSudokuItems()` (AP003) **INVESTIGATE**
- (Bug) Not displaying `PlayerSolvedSudokuMessage` when redone after undone in finish. (before pressing the final enter) (AP004) **FIXED**
- (Bug) Not checking whether the valid number has been written by the user. Only checks whether the current board is valid when `Hint?` button is pressed. (AP005) **FIXED**
- (Bug) Undoing after finish, then adding numbers again does not end the game (AP006) **FIXED**
- (Feature) Score system, Add function around the code where `PlayerSolvedSudokuMessage` is displayed. (Save with .dat) (FR003) **INVESTIGATE**

### Contact
<cozyplanes@tuta.io>

### License
```
    Sudoku Game in WPF made with C#
    
    Copyright (C) 2017 cozyplanes

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
```
