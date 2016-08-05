using System;
using Akka.Actor;
using SudokuSolver.Messages;

namespace SudokuSolver.Actors
{
    /// <summary>
    /// Base class for all Sudoko actors
    /// </summary>
    public class SudokuActor : ReceiveActor
    {
        public SudokuActor()
        {
        }

        protected override void PreStart()
        {
            base.PreStart();

            var eventStream = Context.System.EventStream;

            eventStream.Subscribe(Self, typeof(SetDigit));
            eventStream.Subscribe(Self, typeof(StrikeDigit));

            eventStream.Subscribe(Self, typeof(FindRowDigit));
            eventStream.Subscribe(Self, typeof(FindColDigit));
            eventStream.Subscribe(Self, typeof(FindBlockDigit));
        }

        protected void Publish(object message)
        {
            Context.System.EventStream.Publish(message);
        }

        protected override void Unhandled(object message)
        {
            // we do nothing, just ignore unhandled messages
        }

        protected void PrintLine(int row, int col, string line)
        {
            // printActor.Tell(new PrintLine(row, col, line));

            // NEVER do this in production. NEVER!
            lock (Context.System)
            {
                Console.SetCursorPosition(col, row);
                Console.WriteLine(line);
            }
        }
    }
}
