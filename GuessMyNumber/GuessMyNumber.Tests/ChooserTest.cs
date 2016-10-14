using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.NUnit;
using NUnit.Framework;
using GuessMyNumber.Actors;
using GuessMyNumber.Messages;
using System;

namespace GuessMyNumber.Tests
{
    [TestFixture]
    public class ChooserTest : TestKit
    {
        /// <summary>
        /// unser beeinflussbarer Zufallszahlengenerator
        /// </summary>
        private class FakeRandom : Random
        {
            public int FakeNumber { get; set; }

            public override int Next(int minValue, int maxValue)
            {
                return FakeNumber;
            }
        }

        private FakeRandom fakeRandom;
        private TestActorRef<Chooser> chooser;

        [SetUp]
        public void SetUp()
        {
            fakeRandom = new FakeRandom { FakeNumber = 42 };
            chooser = ActorOfAsTestActorRef<Chooser>(
                Props.Create<Chooser>(fakeRandom), 
                "chooser"
            );
        }

        [Test, TestCase(46), TestCase(97)]
        public void Chooser_OnStart_ChoosesNumber(int secret)
        {
            // Arrange
            fakeRandom.FakeNumber = secret;

            // Act
            chooser.Tell(new Start());

            // Assert
            Assert.AreEqual(secret, chooser.UnderlyingActor.mySecretNumber);
        }

        [Test]
        public void Chooser_OnStart_RepliesStarted()
        {
            // Act
            chooser.Tell(new Start());

            // Assert
            ExpectMsg<Started>();
        }

        // TODO: nach Implementierung von HandleTestTry
        // sind die 3 Testfälle abzudecken: zu klein, zu groß, erraten

        [Test]
        public void Chooser_OnTestTryTooSmall_RepliesTryTooSmall()
        {
            // Arrange
            // TODO: eine Zufallszahl setzen

            // Act
            // TODO: Nachricht "TestTry" mit kleinerer Zahl versenden

            // Assert
            // TODO: gab es die passende Antwort?
            Assert.Fail("Implement this!");

        }

        // TODO: zwei weitere Testfälle
    }
}
