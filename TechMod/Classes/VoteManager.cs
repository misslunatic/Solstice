using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechMod.Classes
{
    public static class VoteManager
    {
        public static List<VoteObject> Objects = new();

        public static VoteObject? RunningInServer(IGuild guild, IMessageChannel channel)
        {
            var obj = Objects.Where(p => p.Channel == channel && p.Guild == guild);
            if (obj.Any())
            {
                return obj.First();
            }
            return null;
        }
    }
}
