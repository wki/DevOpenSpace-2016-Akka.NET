using System;
using Akka.Actor;
using SudokuSolver.Messages;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Actors
{
    /// <summary>
    /// Keep digit counts for a block
    /// </summary>
    public class SudokuBlock : SudokuActor
    {
        private readonly int block;

        private List<int> statistics;

        public SudokuBlock(IActorRef printer, int block)
            : base(printer)
        {
            this.block = block;

            statistics = Enumerable.Range(1, 9).Select(_ => 9).ToList();

            Receive<SetDigit>(SetStatistics, s => s.Block == block);
            Receive<StrikeDigit>(UpdateStatistics, s => s.Block == block);
        }

        private void SetStatistics(SetDigit setDigit)
        {
            var digit = setDigit.Digit;

            statistics[digit - 1] = 1;
            Draw();
        }

        private void UpdateStatistics(StrikeDigit strikeDigit)
        {
            var digit = strikeDigit.Digit;
            if (statistics[digit-1] < 2)
            {
            }
            else if (--statistics[digit-1] == 1)
                Publish(new FindBlockDigit(block, digit));

            Draw();
        }

        private void Draw()
        {
            PrintLine(block, 44, String.Format("{0}: {1}",
                Self.Path.Name,
                String.Join(", ", statistics))
            );
        }
    }
}
