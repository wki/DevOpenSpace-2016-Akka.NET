using System;

namespace SudokuSolver.Messages
{
    public class PrintLine
    {
        public int Col { get; private set; }
        public int Row { get; private set; }
        public string Line { get; private set; }

        public PrintLine(int row, int col, string line)
        {
            Row = row;
            Col = col;
            Line = line;
        }
    }
}
