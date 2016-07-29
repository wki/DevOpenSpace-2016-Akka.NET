using System;

namespace SudokuSolver
{
    /// <summary>
    /// Command message to set a digit at a row + column
    /// </summary>
    public class SetDigit : HandleDigit
    {
        public SetDigit(int row, int col, int digit)
            : base(row, col, digit)
        {
        }
    }
}
