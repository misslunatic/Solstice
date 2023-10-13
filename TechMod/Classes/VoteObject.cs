using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace TechMod.Classes
{
    public class VoteObject
    {
        public IGuild Guild;
        public IMessageChannel Channel;
        public VoteObject(IGuild guild, IMessageChannel channel)
        {
            Console.WriteLine($"New Vote object created in {guild}/{channel}.");
            VoteManager.Objects.Add(this);
            Guild = guild;
            Channel = channel;
        }
        public virtual void Finish() {
            Console.WriteLine($"Vote object removed in {Guild}/{Channel}.");

            VoteManager.Objects.Remove(this);
        }

        public virtual void Stop(bool cancelled = false)
        {
            Finish();

        }
    }
}
