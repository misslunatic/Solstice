using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechMod.Tests.Mocking.FakeClasses
{
    public class FakeApplicationCommandInteractionData : IApplicationCommandInteractionData
    {
        public ulong Id { get; set; }

        public FakeApplicationCommandInteractionData() { 
            var random = new Random();
            Id = (ulong)random.NextInt64();
        }

        public string Name { get; set; } = "unnamed";

        public IReadOnlyCollection<IApplicationCommandInteractionDataOption> Options => throw new NotImplementedException();
    }
}
