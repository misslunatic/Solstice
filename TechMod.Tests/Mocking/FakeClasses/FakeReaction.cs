using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechMod.Tests.Mocking.FakeClasses
{
    public class FakeReaction : IReaction
    {
        public IEmote Emote {get; set;} = null;
        public IUser User { get; set; } = null;
    }
}
