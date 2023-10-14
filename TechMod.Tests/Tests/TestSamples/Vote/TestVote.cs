using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechMod.Classes.Util;

namespace TechMod.Tests.TestSamples.Vote
{
    public class TestVote : Classes.Triggers.Vote
    {
        
        public TestVote(BotSystemManager manager, IMessageChannel channel, IGuild guild, object? arg) : base(manager, channel, guild, arg)
        {
        }

        public override string GetTitle()
        {
            return "TITLE";
        }

        public override string GetDescription()
        {
            return "DESCRIPTION";
        }

        public override string GetNoMessage()
        {
            return "NO";
        }
        public override void CreateAction(Classes.Triggers.Vote vote)
        {
            base.CreateAction(vote);
        }
    }
}
