using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechMod.Classes.Triggers;
using TechMod.Classes.Util;

namespace TechMod.Functions.Votes.Mute
{

    public class MuteUserVote : Vote
    {
        public int _Yes = 0;
        public int _No = 0;
        public float Duration;
        public IGuildUser User;
        public string Reason;
        public struct MuteUserVoteArgs
        {
            public IGuildUser guildUser;
            public float duration;
            public string reason;

            public MuteUserVoteArgs(float minutes, IUser user, string reason) : this()
            {
                duration = minutes;
                guildUser = user as IGuildUser;
                this.reason = reason;
            }
        }

        public MuteUserVote(BotSystemManager manager, IMessageChannel channel, IGuild guild, object? arg) : base(manager, channel, guild, arg)
        {
            Duration = ((MuteUserVoteArgs)arg).duration;
            User = ((MuteUserVoteArgs)arg).guildUser;
            Reason = ((MuteUserVoteArgs)arg).reason;
            UserData = arg;
        }

        public override void CreateAction(Vote vote)
        {
            var l = new MuteUserAction(Manager, vote);
            l.BeginAction();
        }

        public override string GetDescription()
        {
            return "Mutes this user temporarily.\nReason: " + Reason;
        }

        public override string GetTitle()
        {
            return $"Mute {User.Username} for {string.Format(@"{0:hh\:mm\:ss}", TimeSpan.FromTicks((DateTime.Now.AddMinutes(Duration) - DateTime.Now).Ticks))}?";
        }

        public override string GetNoMessage()
        {
            return $"Will not mute {User.Username}.";
        }
    }

}
