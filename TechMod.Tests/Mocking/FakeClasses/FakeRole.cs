using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechMod.Tests.Mocking.FakeClasses
{
    internal class FakeRole : IRole
    {
        public IGuild Guild { get; set; }

        public Color Color { get; set; }

        public bool IsHoisted { get; set; }

        public bool IsManaged { get; set; }

        public bool IsMentionable { get; set; }

        public string Name { get; set; } = "Unnamed Role";

        public string Icon => throw new NotImplementedException();

        public Emoji Emoji => throw new NotImplementedException();

        public GuildPermissions Permissions { get; set; } = new();

        public int Position => throw new NotImplementedException();

        public RoleTags Tags => throw new NotImplementedException();

        public RoleFlags Flags { get; set; } = new();

        public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.Now;

        public ulong Id {get; set;}

        public FakeRole()
        {
            var random = new Random();
            Id = (ulong)random.NextInt64();
        }

        public string Mention => $"<@{Id}>";

        public int CompareTo(IRole? other)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(RequestOptions options = null)
        {
            var guild = Guild as FakeGuild;
            guild._roles.Remove(this);
            return Task.CompletedTask;
        }

        public string GetIconUrl()
        {
            throw new NotImplementedException();
        }

        public Task ModifyAsync(Action<RoleProperties> func, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }
    }
}
