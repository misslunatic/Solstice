using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace TechMod.Classes.Util
{
    public class BotSystem
    {
        public IGuild Guild;
        public IMessageChannel Channel;

        public Action<Exception> OnCatch;
        public Action OnFinish;

        public BotSystemManager Manager;
        public static BotSystemManager DefaultManager = new();

        public BotSystem(BotSystemManager manager, IGuild guild, IMessageChannel channel)
        {
            Console.WriteLine($"New BotSystem created in {guild}/{channel}.");
            Manager = manager;
            manager.Objects.Add(this);
            Guild = guild;
            Channel = channel;
        }
        public virtual void Finish()
        {
            Manager.Mutex.WaitOne();
            OnFinish?.Invoke();
            Manager.Objects.Remove(this);
            Console.WriteLine($"BotSystem removed in {Guild}/{Channel}.");
            Manager.Mutex.ReleaseMutex();
        }

        public virtual void Stop(bool cancelled = false)
        {
            Finish();
        }


    }
}
