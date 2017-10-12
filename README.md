## SudokuApp_WPF_cozyplanes
Sudoku made with WPF in C#. (final-year project)

### Program name
**`$ sudo { ku }`**

### TODO
- (Bug) Undoing after finish, then adding numbers again does not end the game (GP001)
- (Bug) Level sometimes repeats (AP001)
- (Feature) Hint in places where user wants (FR001)
- (Feature) Input Pad to eliminate sudoku board number errors (FR002)
- (Bug) Hint comes from top to right, then goes downwards, needs to appear random or implement FR001 (AP002)
- (Bug) Exception in `View/SudokuUserControl.xaml.cs` - `RefreshSudokuItems()` (AP003)
- (Bug) Not displaying `PlayerSolvedSudokuMessage` when redone after undone in finish. (before pressing the final enter) (AP004)
- (Bug) Not checking whether the valid number has been written by the user. Only checks whether the current board is valid when `Hint?` button is pressed. (AP005)
- (Feature) Score system, Add function around the code where `PlayerSolvedSudokuMessage` is displayed. (Save with .dat) (FR003)

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
