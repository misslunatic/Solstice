using Discord;
using Discord.Interactions;
using TechMod;
using TechMod.Modules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechMod.Actions.MuteUser;
using TechMod.Classes;

namespace TechMod.Actions.MuteUser
{
    public class ClearModule : InteractionModuleBase<SocketInteractionContext>
    {
        [Group("mute", "User muting stuff.")]
        public class MuteUser : InteractionModuleBase<SocketInteractionContext>
        {
            [EnabledInDm(false)]
            [SlashCommand("vote", "Create a vote for locking this specific channel for a set amount of time.")]
            public async Task LockVote(float minutes, IUser user, string reason)
            {
                if (MainModule.Database.UserHasRequiredRole(Context.Guild, Context.User))
                {
                    if (!Vote.Votes.ContainsKey((Context.Guild, Context.Channel)))
                    {
                        if (user == null)
                        {
                            await RespondAsync(":x: No one was supplied.", ephemeral: true);
                            return;
                        }
                        if (string.IsNullOrWhiteSpace(reason))
                        {
                            await RespondAsync(":x: A reason is required.", ephemeral: true);
                            return;
                        }
                        if (minutes >= Config.MuteUserRange.min & minutes <= Config.MuteUserRange.max)
                        {
                            var vote = new MuteUserVote(Context.Channel, Context.Guild, new MuteUserVote.MuteUserVoteArgs(minutes, user, reason));
                            Vote.Votes.Add((Context.Guild, Context.Channel), vote);
                            vote.BeginVote();
                            await RespondAsync(":white_check_mark: Began a vote!", ephemeral: true);
                        }
                        else
                        {
                            await RespondAsync($":x: Must be at least {Config.MuteUserRange.min} and at most {Config.MuteUserRange.max}.", ephemeral: true);
                        }
                    }
                    else
                    {
                        await RespondAsync(":x: A vote is already in progress or being enforced in this channel.");
                    }
                }
                else
                {
                    await RespondAsync($":x: You don't have the {MainModule.Database.UserRequiredRole(Context.Guild).Name} role!", ephemeral: true);
                }
            }
        }
    }
}
