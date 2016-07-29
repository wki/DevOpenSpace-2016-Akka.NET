using System;

namespace SudokuSolver
{
    public class StrikeDigit : HandleDigit
    {
        public StrikeDigit(int row, int col, int digit)
            : base(row, col, digit)
        {
        }
    }
}
