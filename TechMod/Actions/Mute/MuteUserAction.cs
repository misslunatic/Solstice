using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechMod.Classes;
using static TechMod.Actions.MuteUser.MuteUserVote;

namespace TechMod.Actions.MuteUser
{
    public class MuteUserAction : VoteAction
    {
        public MuteUserAction(Vote vote) : base(vote)
        {
            var duration = ((MuteUserVoteArgs)vote.UserData).duration;
            var user = ((MuteUserVoteArgs)vote.UserData).guildUser;
            var ts = TimeSpan.FromMinutes(duration);
            user.SetTimeOutAsync(ts);
            var eb = new EmbedBuilder()
                .WithColor(Color.Green)
                .WithCurrentTimestamp()
                .WithTitle($"# :white_check_mark: {user.Username} has been muted for {ts}.")
                .WithFooter("\nReason: " + ((MuteUserVoteArgs)vote.UserData).reason);

            vote.Message.ReplyAsync("Muted.", embed: eb.Build());

            Stop();
        }
    }
}
