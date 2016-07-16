namespace GuessMyNumber.Messages
{
    /// <summary>
    /// Answer to Enquirer if a try was too small
    /// </summary>
    public class TryTooSmall
    {
        public int Number { get; private set; }

        public TryTooSmall(int number)
        {
            Number = number;
        }
    }
}
