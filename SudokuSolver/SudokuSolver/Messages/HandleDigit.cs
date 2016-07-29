using System;

namespace SudokuSolver
{
    public class HandleDigit
    {
        public int Row { get; private set; }
        public int Col { get; private set; }
        public int Block { get { return Row / 3 * 3 + Col / 3; } }

        public int Digit { get; private set; }

        public HandleDigit(int row, int col, int digit)
        {
            Row = row;
            Col = col;
            Digit = digit;
        }
    }
}
