using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechMod.Classes.Triggers;
using TechMod.Classes.Util;
using TechMod.Functions.Votes.Clear;
using TechMod.Tests.Mocking.FakeClasses;
using TechMod.Tests.Tests.General;

namespace TechMod.Tests.Tests.Actions
{

    [Collection("Clearing Messages")]
    public class ClearTest
    {
        [Theory(DisplayName = "Clearing All Messages ", Timeout = 40000)]
        [InlineData(0)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(200)]
        public void ClearMessageTestAll(int amount) 
        {
            // Setup
            var manager = new BotSystemManager();
            var guild = new FakeGuild();
            var channel = guild.CreateTextChannelAsync("test").Result as FakeMessageChannel;

            for (var i = 0; i < amount+100; i++)
            {
                channel.SendMessageAsync("MESSAGE SPAM");
            }

            var Finished = false;
            var Result = false;
            var ex = new Exception();

            // Action
            var vote = new Vote(manager, channel, guild, new ClearVote.ClearVoteArgs(amount, null));
            vote.Message = channel.SendMessageAsync("Clearing!").Result;
            var action = new ClearAction(manager, vote);
            action.WaitTime = 0;
            vote.Finish();
            action.OnCatch += (Exception ex) =>
            {
                Finished = true;
                Result = false;
            };
            action.OnFinish += () =>
            {
                Result = true;
                Finished = true;
            };
            action.BeginAction();

            while(!Finished)
            {
                Thread.Sleep(5);
            }

            // Handling
            Assert.True(Result, ex.ToString());
            Assert.Equal(103, channel._messages.Count);
            

        }

        [Theory(DisplayName = "Clearing Messages From User", Timeout = 40000)]
        [InlineData(0)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(200)]
        public void ClearMessageTestSpecific(int amount)
        {
            // Setup
            var manager = new BotSystemManager();
            var guild = new FakeGuild();
            var channel = guild.CreateTextChannelAsync("test").Result as FakeMessageChannel;
            var user1 = new FakeUser();
            var user2 = new FakeUser();

            var random = new Random();

            var messages = new List<FakeMessage>();
            var user1Messages = new List<FakeMessage>();

            
            for (var i = 0; i < amount + 100; i++)
            {
                FakeMessage msg;
                if (random.Next(4) == 2)
                {
                    msg = new FakeMessage { Content = "Good Message.", Author = user1 };
                    messages.Add(msg);
                    user1Messages.Add(msg);
                    channel._messages.Add(msg);
                }
                msg = new FakeMessage { Content = "Bad Message!", Author = user2 };
                messages.Add(msg);

                channel._messages.Add(msg);
            }

            for (var i = 0; i < 100; i++)
            {
                
            }

            var Finished = false;
            var Result = false;
            var ex = new Exception();


            // Action
            var vote = new Vote(manager, channel, guild, new ClearVote.ClearVoteArgs(amount, user2));
            vote.Message = channel.SendMessageAsync("Clearing!").Result;
            var action = new ClearAction(manager, vote);
            action.WaitTime = 0;
            vote.Finish();
            action.OnCatch += (Exception ex) =>
            {
                Finished = true;
                Result = false;
            };
            action.OnFinish += () =>
            {
                Result = true;
                Finished = true;
            };
            action.BeginAction();

            while (!Finished)
            {
                Thread.Sleep(5);
            }

            // Handling
            Assert.True(Result, ex.ToString());

            // Verify conditions
            var containsAllUser1 = user1Messages.All(msg => channel._messages.Any(m => m.Id == msg.Id));
            var user2Count = channel._messages.Count(p => p.Author == user2);

            // Assertion
            Assert.True(containsAllUser1, "Not all messages from user1 are present.");


        }
    }
}
