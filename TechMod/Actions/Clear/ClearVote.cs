using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechMod.Actions.Lock;
using TechMod.Classes;

namespace TechMod.Actions.Clear
{
    public class ClearVote : Vote
    {
        public int _Yes = 0;
        public int _No = 0;
        public int Number;
        public IUser? User;

        public struct ClearVoteArgs
        {
            public int Number;
            public IUser? Specific;

            public ClearVoteArgs(int number, IUser? specific)
            {
                Specific = specific;
                Number = number;
            }
        }

        public ClearVote(IMessageChannel channel, IGuild guild, object? arg) : base(channel, guild, arg)
        {
            Number = ((ClearVoteArgs)arg).Number;
            User = ((ClearVoteArgs)arg).Specific;
            UserData = arg;
        }

        public override void CreateAction(Vote vote)
        {
            var l = new ClearAction(vote);
        }

        public override string GetDescription()
        {
            return "Clears messages. This will clear stuff starting at the bottom of the chat.";
        }

        public override string GetTitle()
        {
            return User==null?$"Clear {Number} messages?": $"Clear {Number} messages from {User.Username}?";
        }

        public override string GetNoMessage()
        {
            return $"Will not clear messages.";
        }
    }
    
    
}
