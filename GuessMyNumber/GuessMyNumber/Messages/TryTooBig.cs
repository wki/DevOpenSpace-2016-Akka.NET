namespace GuessMyNumber.Messages
{
    /// <summary>
    /// Answer to Enquirer if a try was too big
    /// </summary>
    public class TryTooBig
    {
        public int Number { get; private set; }

        public TryTooBig(int number)
        {
            Number = number;
        }
    }
}
