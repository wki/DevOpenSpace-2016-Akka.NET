namespace GuessMyNumber.Messages
{
    /// <summary>
    /// Answer to Enquirer if number was guessed
    /// </summary>
    public class Guessed
    {
        public int Number { get; private set; }

        public Guessed(int number)
        {
            Number = number;
        }
    }
}
