using Discord;
using Discord.Interactions;
using ff_cah;
using ff_cah.Modules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechMod.Actions.MuteUser;
using TechMod.Classes;

namespace TechMod.Actions.Clear
{
    public class ClearModule : InteractionModuleBase<SocketInteractionContext>
    {
        [Group("clear", "Channel clearing stuff.")]
        public class MuteUser : InteractionModuleBase<SocketInteractionContext>
        {
            [EnabledInDm(false)]
            [SlashCommand("vote", "Create a vote for clearing a specific amount of messages.")]
            public async Task LockVote(int amount, IUser? user=null)
            {
                if (MainModule.Database.UserHasRequiredRole(Context.Guild, Context.User))
                {
                    if (!Vote.Votes.ContainsKey((Context.Guild, Context.Channel)))
                    {
                      
                        if (amount >= 10 & amount <= 200)
                        {
                            var vote = new ClearVote(Context.Channel, Context.Guild, new ClearVote.ClearVoteArgs(amount, user));
                            Vote.Votes.Add((Context.Guild, Context.Channel), vote);
                            vote.BeginVote();
                            await RespondAsync(":white_check_mark: Began a vote!", ephemeral: true);
                        }
                        else
                        {
                            await RespondAsync(":x: Must be at least 1 and at most 200.", ephemeral: true);
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
