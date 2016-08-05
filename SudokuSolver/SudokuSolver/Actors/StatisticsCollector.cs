using System;
using System.Linq;
using Akka.Actor;
using SudokuSolver.Actors;
using SudokuSolver.Messages;
using System.Collections.Generic;

namespace SudokuSolver
{
    public class StatisticsCollector : SudokuActor
    {
        private Dictionary<Type, int> statistics;

        public StatisticsCollector()
        {
            statistics = new Dictionary<Type, int>();

            Receive<SetDigit>(m => UpdateStatistics(m));
            Receive<StrikeDigit>(m => UpdateStatistics(m));
            Receive<FindRowDigit>(m => UpdateStatistics(m));
            Receive<FindColDigit>(m => UpdateStatistics(m));
            Receive<FindBlockDigit>(m => UpdateStatistics(m));

            Receive<string>(s =>
               statistics.Keys.ToList().ForEach(key =>
                    Console.WriteLine("{0}: {1}", key.Name, statistics[key])
               )
            );
        }

        private void UpdateStatistics(object message)
        {
            var type = message.GetType();

            if (statistics.ContainsKey(type))
            {
                statistics[type]++;
                // statistics[type] = statistics[type] + 1;
            }
            else
            {
                statistics.Add(type, 1);
            }
        }
    }
}

