using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechMod.Classes;
using TechMod.Classes.Util;
using TechMod.Tests.Mocking.FakeClasses;

namespace TechMod.Tests.Tests.General
{
    public class BotSystemTests
    {
        public static Mutex SyncMutex = new Mutex();

        [Theory]
        [InlineData(1)]
        [InlineData(8)]
        [InlineData(80)]
        [InlineData(800)]
        [InlineData(8000)]
        public void ProperRemoval(int amount)
        {
            var manager = new BotSystemManager();
            SyncMutex.WaitOne();
            for (int i = 0; i < amount; i++)
            {
                var guild = new FakeGuild();
                var channel = new FakeMessageChannel();
                var system = new BotSystem(manager, guild, channel);
            }

            Assert.Equal(amount, manager.Objects.Count);


            foreach(var data in manager.Objects.ToList())
            {
                data.Finish();
            }

            Assert.Empty(manager.Objects);

            manager.Objects.Clear();
            SyncMutex.ReleaseMutex();
        }
    }
}
