using Discord.Interactions;
using ff_cah.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechMod.Classes;

namespace TechMod.Actions.Lock
{
    public class LockModule : InteractionModuleBase<SocketInteractionContext>
    {
        [Group("lock", "Channel locking stuff.")]
        public class Lock : InteractionModuleBase<SocketInteractionContext>
        {
            [EnabledInDm(false)]
            [SlashCommand("vote", "Create a vote for locking this specific channel for a set amount of time.")]
            public async Task LockVote(float minutes)
            {
                if (MainModule.Database.UserHasRequiredRole(Context.Guild, Context.User))
                {
                    if (!Vote.Votes.ContainsKey((Context.Guild, Context.Channel)))
                    {
                        if (minutes >= 0.5 & minutes <= 30)
                        {
                            var vote = new LockVote(Context.Channel, Context.Guild, minutes);
                            Vote.Votes.Add((Context.Guild, Context.Channel), vote);
                            vote.BeginVote();
                            await RespondAsync(":white_check_mark: Began a vote!", ephemeral: true);
                        }
                        else
                        {
                            await RespondAsync(":x: Must be at least 0.5 and at most 30.", ephemeral: true);
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
