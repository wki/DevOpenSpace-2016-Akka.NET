using System;
using Akka.Actor;
using SudokuSolver.Actors;
using System.Threading;

namespace SudokuSolver
{
    // TODO:
    //  - create SodukuSolver actor (grid array is initial arg)
    //     - create all actor* actors
    //     - wait until all are ready
    //     - start solving
    //     - also register for SetDigit Messages
    //     - timeout: cannot solve
    //     - count SetDigit Messages (81 = ready)
    //  - discover solved sudoku
    //  - discover unsolvable sudoku
    //  - automatically print statistics at end
    class MainClass
    {
        public static void Main(string[] args)
        {
            var system = ActorSystem.Create("Sudoku");

            CreateSudokuActors(system);
            ClearScreen();

            var statistics = system.ActorOf<StatisticsCollector>();

            PopulateSudoku(system);

            Console.SetCursorPosition(0, 44);
            Console.WriteLine("Press [enter] to Quit");
            Console.ReadLine();

            Console.SetCursorPosition(0, 38);
            statistics.Tell("print");
            Console.ReadLine();

            system.Terminate();
        }

        private static void CreateSudokuActors(ActorSystem system)
        {
            var printer = system.ActorOf<Printer>();

            for (int row = 0; row < 9; row++)
            {
                system.ActorOf(
                    Props.Create<SudokuRow>(printer, row), 
                    String.Format("r-{0}", row)
                );

                for (int col = 0; col < 9; col++)
                {
                    if (row == 0)
                        system.ActorOf(
                            Props.Create<SudokuCol>(printer, col), 
                            String.Format("c-{0}", col)
                        );

                    system.ActorOf(
                        Props.Create<SudokuCell>(printer, row, col), 
                        String.Format("{0}-{1}", row, col)
                    );
                }
            }
                
            for (int block = 0; block < 9; block++)
                system.ActorOf(
                    Props.Create<SudokuBlock>(printer, block), 
                    String.Format("b-{0}", block)
                );
        }

        private static void ClearScreen()
        {
            for (var row = 0; row < 44; row++)
            {
                Console.SetCursorPosition(0, row);
                if (row == 11 || row == 24)
                    Console.Write("_________________________________________                                                              ");
                else if (row < 37)
                    Console.Write("             |              |                                                                          ");
                else
                    Console.Write("{0,80}", "");
            }
        }
        private static void PopulateSudoku(ActorSystem system)
        {
            // TODO: find better was. possibly wait untl all report "ready"
            Thread.Sleep(TimeSpan.FromSeconds(1));

            var sudoku = HardSudoku();
            for (var i = 0; i < sudoku.Length; i++)
            {
                int digit = sudoku[i];

                int row = i / 9;
                int col = i % 9;

                if (digit != 0)
                {
                    system.EventStream.Publish(new SetDigit(row, col, digit));
                    Thread.Sleep(TimeSpan.FromMilliseconds(200));
                }
            }
        }

        private static int[] HardSudoku()
        {
            return new []
                {
                    0,0,4,   0,8,0,   0,1,0,
                    0,0,0,   9,0,2,   0,0,0,
                    2,6,0,   0,0,7,   5,0,0,

                    0,7,3,   0,0,0,   0,0,0,
                    6,0,0,   0,0,0,   4,9,3,
                    0,5,0,   0,1,0,   0,0,7,

                    0,0,1,   7,3,0,   0,2,0,
                    0,0,0,   0,0,0,   0,0,0,
                    0,0,0,   8,0,0,   6,0,0
                };
        }

        private static int[] EasySudoku()
        {
            return new []
                {
                    7,0,1,   9,6,8,   5,3,0,
                    0,9,0,   0,0,1,   8,7,6,
                    2,8,6,   7,0,0,   4,1,9,

                    0,0,0,   2,5,0,   7,8,0,
                    6,7,5,   0,8,0,   2,9,1,
                    0,0,4,   0,9,7,   3,0,0,

                    4,0,2,   0,7,9,   6,0,3,
                    0,6,8,   0,0,3,   0,2,7,
                    0,0,7,   0,1,2,   9,0,8
                };
        }
    }
}
