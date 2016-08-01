using System;
using Akka.Actor;
using Akka.Configuration;

namespace SimpleClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var system = ActorSystem.Create("Client");

        }
    }
}
