using System;
using Akka.Actor;
using SudokuSolver.Messages;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Actors
{
    /// <summary>
    /// Keep digit counts for a column
    /// </summary>
    public class SudokuCol : SudokuActor
    {
        private readonly int col;

        private List<int> statistics;

        public SudokuCol(int col)
        {
            this.col = col;

            statistics = Enumerable.Range(1, 9).Select(_ => 9).ToList();

            Receive<SetDigit>(SetStatistics, s => s.Col == col);
            Receive<StrikeDigit>(UpdateStatistics, s => s.Col == col);
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
                Publish(new FindColDigit(col, digit));

            Draw();
        }

        private void Draw()
        {
            PrintLine(10 + col, 44, String.Format("{0}: {1}",
                Self.Path.Name,
                String.Join(", ", statistics))
            );
        }
    }
}
