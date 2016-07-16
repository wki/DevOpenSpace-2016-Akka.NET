using System;
using Akka.Actor;
using Akka.Routing;
using WordCount.Actors;
using System.Threading;

namespace WordCount
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var system = ActorSystem.Create("Words");

            var console = system.ActorOf(
                Props.Create<ConsoleWriter>()
            );
            var counter = system.ActorOf(
                Props.Create<WordCounter>(console)
            );
            var splitter = system.ActorOf(
                Props.Create<WordSplitter>(counter)
            );
            system.ActorOf(
                Props.Create<TextReader>(SampleText(), splitter)
            );

            // TODO: dieser Teil der Kette ist das was wir wollen.
            // var caser = system.ActorOf(Props.Create<LowerCaser>(splitter));
            // system.ActorOf(Props.Create<TextReader>(SampleText(), caser));

            Thread.Sleep(1000);

            Console.WriteLine("Press [enter] to Stop");
            Console.ReadLine();

            system.Terminate().Wait();
        }

        private static string SampleText()
        {
            return @"
                Lorem ipsum dolor sit amet, 
                consetetur sadipscing elitr, 
                sed diam nonumy eirmod tempor invidunt 
                ut labore et dolore magna aliquyam erat, 
                sed diam voluptua. 

                At vero eos et accusam et justo duo dolores et ea rebum. 

                Stet clita kasd gubergren, no sea takimata sanctus 
                est Lorem ipsum dolor sit amet. 
                Lorem ipsum dolor sit amet, consetetur sadipscing elitr, 
                sed diam nonumy eirmod tempor invidunt ut labore et
                dolore magna aliquyam erat, sed diam voluptua. 

                At vero eos et accusam et justo duo dolores et ea rebum. 

                Stet clita kasd gubergren, no sea takimata sanctus est 
                Lorem ipsum dolor sit amet.            
            ";
        }
    }
}
