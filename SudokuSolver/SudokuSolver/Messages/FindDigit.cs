namespace SudokuSolver.Messages
{
    public class FindDigit
    {
        public int Digit { get; private set; }

        public FindDigit(int digit)
        {
            Digit = digit;
        }
    }
}
