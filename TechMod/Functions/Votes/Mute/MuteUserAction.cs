using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechMod.Classes.Behaviors;
using TechMod.Classes.Triggers;
using TechMod.Classes.Util;
using static TechMod.Functions.Votes.Mute.MuteUserVote;

namespace TechMod.Functions.Votes.Mute
{
    public class MuteUserAction : VoteAction
    {
        public MuteUserAction(BotSystemManager manager, Vote vote) : base(manager, vote)
        {
            var duration = ((MuteUserVoteArgs)vote.UserData).duration;
            var user = ((MuteUserVoteArgs)vote.UserData).guildUser;
            var ts = TimeSpan.FromMinutes(duration);
            user.SetTimeOutAsync(ts);
            var eb = new EmbedBuilder()
                .WithColor(Color.Green)
                .WithCurrentTimestamp()
                .WithTitle($":white_check_mark: {user.Username} has been muted for {string.Format(@"{0:hh\:mm\:ss}", ts)}.")
                .WithFooter("\nReason: " + ((MuteUserVoteArgs)vote.UserData).reason);

            vote.Message.ReplyAsync("Muted.", embed: eb.Build());

            Stop();
        }
    }
}
