using System;
using Akka.Actor;
using SudokuSolver.Messages;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Actors
{
    /// <summary>
    /// Keep digit counts for a row
    /// </summary>
    public class SudokuRow : SudokuActor
    {
        private readonly int row;

        private List<int> statistics;

        public SudokuRow(int row)
        {
            this.row = row;

            statistics = Enumerable.Range(1, 9).Select(_ => 9).ToList();

            Receive<SetDigit>(SetStatistics, s => s.Row == row);
            Receive<StrikeDigit>(UpdateStatistics, s => s.Row == row);
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
                Publish(new FindRowDigit(row, digit));

            Draw();
        }

        private void Draw()
        {
            PrintLine(20 + row, 44, String.Format("{0}: {1}",
                Self.Path.Name,
                String.Join(", ", statistics))
            );
        }
    }
}
