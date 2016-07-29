namespace SudokuSolver.Messages
{
    public class FindColDigit : FindDigit
    {
        public int Col { get; private set; }

        public FindColDigit(int col, int digit) : base(digit)
        {
            Col = col;
        }
    }
}
