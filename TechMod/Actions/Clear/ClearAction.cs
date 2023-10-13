using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechMod.Classes;

namespace TechMod.Actions.Clear
{
    public class ClearAction : VoteAction
    {
        public ClearAction(Vote vote) : base(vote)
        {
            var data = ((ClearVote.ClearVoteArgs)vote.UserData);
            var amount = data.Number;

            var eb = new EmbedBuilder()
                .WithColor(Color.Green)
                .WithCurrentTimestamp()
                .WithTitle($"# :white_check_mark: Removing the messages above"+(data.Specific==null?"...":$" from {data.Specific.Username}..."));

            var msgList = Channel.GetMessagesAsync(amount).ToListAsync().Result;
            var number = msgList.Count;
            var msg = vote.Message.ReplyAsync(":raised_hand: Deleting messages...").Result;
            try
            {
                foreach (var x in msgList)
                {
                    foreach (var y in x)
                    {
                        if (data.Specific == null || y.Author == data.Specific)
                        {
                            y.DeleteAsync().GetAwaiter().GetResult();
                            Thread.Sleep(600);
                        }
                    }
                }

                msg.ReplyAsync($":white_check_mark: Removed {number} messages!");
            }
            catch(Exception ex)
            {
                msg.ReplyAsync(":x: Failed to delete messages: " + ex.Message);
            }
            Stop();
        }
    }
}
