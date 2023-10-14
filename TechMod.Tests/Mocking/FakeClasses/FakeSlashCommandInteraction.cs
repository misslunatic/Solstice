using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechMod.Tests.Mocking.FakeClasses
{
    internal class FakeSlashCommandInteraction : FakeDiscordInteraction, ISlashCommandInteraction
    {
        IApplicationCommandInteractionData ISlashCommandInteraction.Data { get; }

        IApplicationCommandInteractionData IApplicationCommandInteraction.Data { get; }

        public override Task DeferAsync(bool ephemeral = false, RequestOptions options = null)
        {
            base.DeferAsync(ephemeral, options);

            return Task.CompletedTask;
        }
    }
}
