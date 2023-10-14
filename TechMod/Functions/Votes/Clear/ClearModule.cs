using Discord;
using Discord.Interactions;
using TechMod.Modules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechMod.Classes.Util;
using TechMod.Classes.Triggers;

namespace TechMod.Functions.Votes.Clear
{
    public class ClearModule : InteractionModuleBase<SocketInteractionContext>
    {
        [Group("clear", "Channel clearing stuff.")]
        public class MuteUser : InteractionModuleBase<SocketInteractionContext>
        {
            [EnabledInDm(false)]
            [SlashCommand("vote", "Create a vote for clearing a specific amount of messages.")]
            public async Task LockVote(int amount, IUser? user = null)
            {
                if (MainModule.Database.UserHasRequiredRole(Context.Guild, Context.User))
                {
                    if (!Vote.Votes.ContainsKey((Context.Guild, Context.Channel)))
                    {

                        if (amount >= Config.ClearRange.min & amount <= Config.ClearRange.max)
                        {
                            var vote = new ClearVote(BotSystem.DefaultManager, Context.Channel, Context.Guild, new ClearVote.ClearVoteArgs(amount, user));
                            Vote.Votes.Add((Context.Guild, Context.Channel), vote);
                            vote.BeginVote();
                            await RespondAsync(":white_check_mark: Began a vote!", ephemeral: true);
                        }
                        else
                        {
                            await RespondAsync($":x: Must be at least {Config.ClearRange.min} and at most {Config.ClearRange.max}.", ephemeral: true);
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
