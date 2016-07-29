namespace SudokuSolver.Messages
{
    public class FindBlockDigit : FindDigit
    {
        public int Block { get; private set; }

        public FindBlockDigit(int block, int digit) : base(digit)
        {
            Block = block;
        }
    }
}
