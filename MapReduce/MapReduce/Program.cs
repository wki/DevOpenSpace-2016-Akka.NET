using System;
using Akka.Actor;
using MapReduce.Actors;

namespace MapReduce
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var system = ActorSystem.Create("MapReduce");
            var master = system.ActorOf(Props.Create<Master>());

            for (var i = 0; i < 100; i++)
            {
                master.Tell(Sample1());
                master.Tell(Sample2());
            }

            Console.ReadLine();

            system.Terminate();
        }

        private static string Sample1()
        {
            return @"

ACME, Inc  | Mr. Green | C#
ACME, Inc  | Mr. Blue  | JavaScript

Facilities | Coolman   | Ruby

FastCoders | Speedy    | C#

SlowCoders | Snail     | Java

";
        }

        private static string Sample2()
        {
            return @"
Foo | Mr. Green | C#
Foo | Mr. Blue | Java
Foo | Mr. Black | C#
Foo | Mr. White | F#

Bla | Coolman | Python
Bla | Catwoman | C#

SlowMovers | Snail | C#
SlowMovers | Snail | F#
";
        }
    }
}
