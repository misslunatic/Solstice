using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using TechMod.Classes.Triggers;
using TechMod.Classes.Util;

namespace TechMod.Functions.Votes.Lock
{
    public class LockVote : Vote
    {
        public int _Yes = 0;
        public int _No = 0;
        public float Duration;

        public LockVote(BotSystemManager manager, IMessageChannel channel, IGuild guild, object? arg) : base(manager, channel, guild, arg)
        {
            Duration = (float)arg;
            UserData = arg;
        }

        public override void CreateAction(Vote vote)
        {
            var l = new LockAction(Manager, vote);
        }

        public override string GetDescription()
        {
            return "Prevents everyone (excepting admins) from using this channel.\n Use this to break up drama, an argument, etc.";
        }

        public override string GetTitle()
        {
            return $"Lock the channel for {string.Format(@"{0:hh\:mm\:ss}", TimeSpan.FromTicks((DateTime.Now.AddMinutes(Duration) - DateTime.Now).Ticks))}?";
        }

        public override string GetNoMessage()
        {
            return "Will not lock the channel.";
        }
    }
}
