using Akka.TestKit;
using Akka.TestKit.NUnit;
using NUnit.Framework;
using Akka.Actor;
using GuessMyNumber.Actors;
using GuessMyNumber.Messages;

namespace GuessMyNumber.Tests
{
    [TestFixture]
    public class EmptyTest : TestKit
    {
        private IActorRef chooser;

        [SetUp]
        public void SetUp()
        {
            chooser = Sys.ActorOf(Props.Create<Chooser>(), "chooser");
        }

        [Test]
        public void Chooser_OnStart_RepliesStarted()
        {
            // Act
            chooser.Tell(new Start());

            // Assert
            ExpectMsg<Started>();
        }
    }
}
