**IMPORTANT! DUE (TUESDAY M10 24 23:59 KST). MUST SEND PROJECT FILE VIA EMAIL**      - Set up needed

**MAKE NEW PROJECT + DELETE UNNECESSARY REFS**      - Finished

**INVESTIGATE INNO SETUP INSTALLER**       - Investigate


## SudokuApp_WPF_cozyplanes
Sudoku made with WPF in C#. (final-year project)
See README.md in publish_beta for installing and checking options.

### Program name
**`$ sudo { ku }`**    - in review

### ISSUES
- (Bug) Only need to take input of 1-9, but excepting all numbers **IN REVIEW**
- (Bug) Exception in `View/SudokuUserControl.xaml.cs` - `RefreshSudokuItems()` (AP003) **IN REVIEW**

- (Bug) Level sometimes repeats (AP001) **FIXED**
- (Bug) Not displaying `PlayerSolvedSudokuMessage` when redone after undone in finish. (before pressing the final enter) (AP004) **FIXED**
- (Bug) Not checking whether the valid number has been written by the user. Only checks whether the current board is valid when `Hint?` button is pressed. (AP005) **FIXED**
- (Bug) Undoing after finish, then adding numbers again does not end the game (AP006) **FIXED**
- (Bug) Do not accept special characters (unicode) except numbers **FIXED**

- (Feature) Score system, Add function around the code where `PlayerSolvedSudokuMessage` is displayed. (Save with .dat) (FR003) **WILL NOT IMPLEMENT 60%** **IN REVIEW**
- (Feature) Hint in places where user wants (FR001) **WILL NOT IMPLEMENT 100%**
- (Feature) Input Pad to eliminate sudoku board number errors (FR002) **WILL NOT IMPLEMENT 100%**
- (Bug) Hint comes from top to right, then goes downwards, needs to appear random or implement FR001 (AP002) **WILL NOT IMPLEMENT 100%**


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
