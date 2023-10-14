using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechMod.Classes.Behaviors;
using TechMod.Classes.Triggers;
using TechMod.Classes.Util;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TechMod.Functions.Votes.Clear
{
    public class ClearAction : VoteAction
    {
        
        public int amount = 0;
        public ClearVote.ClearVoteArgs data;
        public Vote vote;
        public ushort WaitTime = 600;

        public ClearAction(BotSystemManager manager, Vote vote) : base(manager, vote)
        {
            data = (ClearVote.ClearVoteArgs)vote.UserData;
            amount = data.Number;
            this.vote = vote;
        }

        public override void BeginAction()
        {

            var eb = new EmbedBuilder()
                .WithColor(Color.Green)
            .WithCurrentTimestamp()
            .WithTitle($"# :white_check_mark: Removing the messages above" + (data.Specific == null ? "..." : $" from {data.Specific.Username}..."));

            var msgList = Channel.GetMessagesAsync(amount).ToListAsync().Result;
            var number = 0;
            foreach (var list in msgList)
            {
                number += list.Count;
            }

            var msg = vote.Message.ReplyAsync($":raised_hand: Deleting {number} messages...").Result;
            try
            {
                foreach (var x in msgList)
                {
                    foreach (var y in x)
                    {
                        if (data.Specific == null || y.Author == data.Specific)
                        {
                            y.DeleteAsync().GetAwaiter().GetResult();
                            Thread.Sleep(WaitTime);
                        }
                    }
                }

                msg.ReplyAsync($":white_check_mark: Removed messages!");

            }
            catch (Exception ex)
            {
                msg.ReplyAsync(":x: Failed to delete messages: " + ex.Message);
                OnCatch?.Invoke(ex);
            }
            Stop();
        }
    }
}
