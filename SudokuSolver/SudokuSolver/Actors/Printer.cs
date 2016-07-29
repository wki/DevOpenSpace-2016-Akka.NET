using System;
using Akka.Actor;
using SudokuSolver.Messages;

namespace SudokuSolver
{
    public class Printer : ReceiveActor
    {
        public Printer()
        {
            Receive<PrintLine>(p =>
                {
                    Console.SetCursorPosition(p.Col, p.Row);
                    Console.WriteLine(p.Line);
                });
        }
    }
}
