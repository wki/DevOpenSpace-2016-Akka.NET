using System;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.NUnit;
using NUnit.Framework;
using GuessMyNumber.Actors;
using GuessMyNumber.Messages;

namespace GuessMyNumber.Tests
{
    [TestFixture]
    public class GuessGameTest : TestKit
    {
        private IActorRef game;
        private TestProbe chooser;
        private TestProbe enquirer;

        private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(0.5);


        [SetUp]
        public void SetUp()
        {
            chooser = CreateTestProbe("chooser");
            enquirer = CreateTestProbe("enquirer");

            game = Sys.ActorOf(Props.Create<GuessGame>(chooser, enquirer, TestActor), "game");
        }

        [Test]
        public void GuessGame_Initially_Creates2Children()
        {
            // Assert
            // TODO: nicht testbar
            
        }

        [Test]
        public void GuessGame_OnStart_StartsChooser()
        {
            // Act
            game.Tell(new Start());

            // Assert
            chooser.ExpectMsg<Start>();
            enquirer.ExpectNoMsg(Timeout);
        }

        [Test]
        public void GuessGame_OnStarted_StartsEnquirer()
        {
            // Act
            game.Tell(new Started());

            // Assert
            enquirer.ExpectMsg<Start>();
            chooser.ExpectNoMsg(Timeout);
        }

        [Test]
        public void GuessGame_WhenReceivingGuessed_EmitsGuessedToParent()
        {
            // Act
            game.Tell(new Guessed(42));

            // Assert
            var answer = ExpectMsg<Guessed>();
            Assert.AreEqual(42, answer.Number);

            enquirer.ExpectNoMsg(Timeout);
            chooser.ExpectNoMsg(Timeout);
        }
    }
}
