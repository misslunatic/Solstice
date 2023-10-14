using Discord;
using TechMod.Data;
using TechMod.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechMod.Classes.Util;
using TechMod.Classes.Triggers;

namespace TechMod.Classes.Behaviors
{
    public class VoteAction : BotSystem
    {
        public int Yes;
        public int No;

        public static InfoDB Database => MainModule.Database;

        public VoteAction(BotSystemManager manager, Vote vote) : base(manager, vote.Guild, vote.Channel)
        {
            No = vote.VotedNo;
            Yes = vote.VotedYes;
        }

        public VoteAction(BotSystemManager manager, IGuild guild, IMessageChannel channel) : base(manager, guild, channel)
        {

        }

        public virtual void BeginAction()
        {

        }


    }
}
