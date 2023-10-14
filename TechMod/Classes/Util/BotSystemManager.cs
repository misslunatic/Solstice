using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechMod.Classes.Util
{
    public class BotSystemManager
    {
        public List<BotSystem> Objects = new();
        public Mutex Mutex = new Mutex();

        public BotSystem? RunningInServer(IGuild guild, IMessageChannel channel)
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
