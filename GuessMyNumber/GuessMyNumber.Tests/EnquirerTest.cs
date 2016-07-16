using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.NUnit;
using NUnit.Framework;
using GuessMyNumber.Actors;
using GuessMyNumber.Messages;

namespace GuessMyNumber.Tests
{
    class EnquirerTest : TestKit
    {
        private TestActorRef<Enquirer> enquirer;
        private TestProbe chooser;

        [SetUp]
        public void SetUp()
        {
            chooser = CreateTestProbe("chooser");
            enquirer = ActorOfAsTestActorRef<Enquirer>(Props.Create<Enquirer>(TestActor, chooser), "enquirer");
        }

        [Test]
        public void Enquirer_Initially_HasRange1To100()
        {
            // Assert
            Assert.AreEqual(1, enquirer.UnderlyingActor.rangeFrom, "from");
            Assert.AreEqual(100, enquirer.UnderlyingActor.rangeTo, "to");
        }

        [Test]
        public void Enquirer_OnStart_MakesTry()
        {
            // Act
            enquirer.Tell(new Start());

            // Assert
            chooser.ExpectMsg<TestTry>(t => t.Number == 50);
        }

        [Test, TestCase(50), TestCase(30)]
        public void Enquirer_OnTooSmall_IncreasesRangeFrom(int number)
        {
            // Act
            enquirer.Tell(new TryTooSmall(number));

            // Assert
            Assert.AreEqual(number+1, enquirer.UnderlyingActor.rangeFrom);
        }

        [Test, TestCase(50)]
        public void Enquirer_OnTooSmall_MakesTry(int number)
        {
            // Act
            enquirer.Tell(new TryTooSmall(number));

            // Assert
            chooser.ExpectMsg<TestTry>();
        }

        [Test, TestCase(50), TestCase(80)]
        public void Enquirer_OnTooBig_DecreasesRangeTo(int number)
        {
            // Act
            enquirer.Tell(new TryTooBig(number));

            // Assert
            Assert.AreEqual(number-1, enquirer.UnderlyingActor.rangeTo);
        }

        [Test, TestCase(50)]
        public void Enquirer_OnTooBig_MakesTry(int number)
        {
            // Act
            enquirer.Tell(new TryTooSmall(number));

            // Assert
            chooser.ExpectMsg<TestTry>();
        }

        [Test, TestCase(50)]
        public void Enquirer_OnGuessed_ReplyGuessed(int number)
        {
            // Act
            enquirer.Tell(new Guessed(number));

            // Assert
            ExpectMsg<Guessed>(); // fails.
        }
    }
}
