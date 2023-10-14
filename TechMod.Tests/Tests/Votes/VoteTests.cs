using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechMod.Classes.Triggers;
using TechMod.Classes.Util;
using TechMod.Tests.Mocking.FakeClasses;
using TechMod.Tests.TestSamples.Vote;

namespace TechMod.Tests.Tests.Votes
{
    [Collection("Voting System")]
    public class VoteTests
    {
        bool Result = false;
        bool Returned = false;
        bool TimedOut = false;

        public (bool, string) VoteMenuTest(float duration, byte amount, ushort yes, ushort no, bool defaultOnTie, bool expect)
        {

            // Setup
            Returned = false;
            Result = false;

            var channel = new FakeMessageChannel();
            var guild = new FakeGuild();
            var vote = new TestVote(BotSystem.DefaultManager, channel, guild, duration);
            vote.Minimum = amount;
            vote.DefaultOnTie = defaultOnTie;
            for (var i = 0; i < yes; i++)
            {
                vote.Users.Add(new FakeUser(), true);
            }
            for (var i = 0; i < no; i++)
            {
                vote.Users.Add(new FakeUser(), false);
            }
            vote.OnCreateAction = () =>
            {
                Result = true;
                Returned = true;
            };
            vote.OnVoteNo = () =>
            {
                Result = false;
                Returned = true;
            };

            Vote.Votes.Add((guild, channel), vote);
            vote.BeginVote();
            vote.SetVoteDuration(duration);

            System.Timers.Timer timeOutTimer = new((duration + 0.2f) * 60000);
            timeOutTimer.Elapsed += (caller, args) =>
            {
                timeOutTimer.Stop();
                Result = false;
                TimedOut = true;
                Returned = false;
            };

            timeOutTimer.Start();

            while (!Returned && !TimedOut)
            {
                Thread.Sleep(5);
            }
            timeOutTimer.Stop();
            if (TimedOut)
            {

                return (false, $"Timed out - Duration {duration}, Minimum {amount}, Yes {yes}, No {no}, Expect {expect}.");
            }
            if (Result == expect)
            {
                return (true, $"Ran successfully - duration {duration}, Minimum {amount}, Yes {yes}, No {no}, Expect {expect}.");
            }
            return (false, $"Did not get expected result - duration {duration}, Minimum {amount}, Yes {yes}, No {no}, Expect {expect}.");



        }

        [Theory]
        [InlineData(4, 1, 0, true)]
        [InlineData(4, 1, 1, true)]
        [InlineData(4, 0, 1, true)]
        [InlineData(4, 0, 0, true)]
        [InlineData(4, 1, 0, false)]
        [InlineData(4, 1, 1, false)]
        [InlineData(4, 0, 1, false)]
        [InlineData(4, 0, 0, false)]
        public void VoteMenuOverall(byte min, ushort yes, ushort no, bool bias)
        {
            var expected = false;
            if (yes >= min || no >= min)
            {
                if (no > yes) expected = false;
                if (yes > no) expected = true;
                if (yes == no) expected = bias;
            }
            var vote = VoteMenuTest(0.05f, min, yes, no, bias, expected);
            Assert.True(vote.Item1, vote.Item2);
        }



    }
}
