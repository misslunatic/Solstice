using Discord;
using ff_cah.Data;
using ff_cah.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechMod.Classes
{
    public class VoteAction : VoteObject
    {
        public int Yes;
        public int No;

        public static InfoDB Database => MainModule.Database;

        public VoteAction(Vote vote) : base(vote.Guild, vote.Channel) 
        {
            No = vote.VotedNo; 
            Yes = vote.VotedYes;
        }

        public VoteAction(IGuild guild, IMessageChannel channel) : base(guild, channel)
        {

        }


    }
}
