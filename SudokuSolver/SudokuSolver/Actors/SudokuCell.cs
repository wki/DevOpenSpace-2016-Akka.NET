using System;
using System.Collections.Generic;
using Akka.Actor;
using System.Linq;
using SudokuSolver.Messages;
using System.Threading;

namespace SudokuSolver.Actors
{
    /// <summary>
    /// Store possible digits (or a solution) for a single cell
    /// </summary>
    public class SudokuCell : SudokuActor
    {
        private readonly int row;
        private readonly int col;
        private readonly int block;

        private HashSet<int> possibleDigits;

        public SudokuCell(IActorRef printer, int row, int col)
            : base(printer)
        {
            this.row = row;
            this.col = col;
            this.block = row / 3 * 3 + col / 3;

            possibleDigits = new HashSet<int>(Enumerable.Range(1,9));

            Receive<SetDigit>(d => RestrictPossibilities(d), IsRowOrColOrBlock);
            Receive<SetDigit>(d => SolveCell(d), IsMyCell);

            Receive<FindRowDigit>(d => FindDigitHandler(d), f => f.Row == row);
            Receive<FindColDigit>(d => FindDigitHandler(d), f => f.Col == col);
            Receive<FindBlockDigit>(d => FindDigitHandler(d), f => f.Block == block);
        }

        // restrict possible digits for row, col, block
        private void RestrictPossibilities(SetDigit setDigit)
        {
            var digit = setDigit.Digit;

            if (possibleDigits.Contains(digit))
            {
                possibleDigits.Remove(digit);

                if (possibleDigits.Count == 1)
                    Publish(new SetDigit(row, col, possibleDigits.First()));

                Publish(new StrikeDigit(row, col, digit));

                Draw();
            }
        }

        private void SolveCell(SetDigit setDigit)
        {
            var digit = setDigit.Digit;

            if (!IsSolved(digit))
            {
                possibleDigits.ToList()
                    .ForEach(d => Publish(new StrikeDigit(row, col, d)));

                possibleDigits.Clear();
                possibleDigits.Add(digit);

                Draw();
            }
        }

        private bool IsSolved(int digit)
        {
            return possibleDigits.Contains(digit) && possibleDigits.Count == 1;
        }

        private void FindDigitHandler(FindDigit findDigit)
        {
            var digit = findDigit.Digit;

            if (possibleDigits.Contains(digit))
                Publish(new SetDigit(row, col, digit));
        }

        private bool IsRowOrColOrBlock(SetDigit setDigit)
        {
            if (IsMyCell(setDigit))
                return false;

            return setDigit.Row == row
                || setDigit.Col == col
                || setDigit.Block == block;
        }

        private bool IsMyCell(SetDigit setDigit)
        {
            return setDigit.Row == row && setDigit.Col == col;
        }

        private void Draw()
        {
            int terminalRow = row * 4 + (row / 3);
            int terminalCol = col * 4 + (col / 3 * 3);

            if (possibleDigits.Count == 1)
            {
                PrintLine(terminalRow, terminalCol, "+-+");
                PrintLine(terminalRow + 1, terminalCol, String.Format("|{0}|", possibleDigits.First()));
                PrintLine(terminalRow + 2, terminalCol, "+-+");
            }
            else
            {
                for (var digit = 1; digit <= 9; digit++)
                {
                    int subRow = (digit - 1) / 3;
                    int subCol = (digit - 1) % 3;

                    if (possibleDigits.Contains(digit))
                        PrintLine(terminalRow + subRow, terminalCol + subCol, digit.ToString());
                    else
                        PrintLine(terminalRow + subRow, terminalCol + subCol, " ");
                }
            }
        }
    }
}
