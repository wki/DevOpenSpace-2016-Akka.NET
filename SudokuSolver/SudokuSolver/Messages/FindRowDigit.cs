namespace SudokuSolver.Messages
{
    public class FindRowDigit : FindDigit
    {
        public int Row { get; private set; }

        public FindRowDigit(int row, int digit) : base(digit)
        {
            Row = row;
        }
    }
}
