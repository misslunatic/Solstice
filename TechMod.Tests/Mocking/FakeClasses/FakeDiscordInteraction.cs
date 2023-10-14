using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechMod.Tests.Mocking.FakeClasses
{
    public class FakeDiscordInteraction : IDiscordInteraction
    {
        public ulong Id { get; set; }

        public FakeDiscordInteraction()
        {
            var random = new Random();
            Id = (ulong)random.NextInt64();

        }
        public InteractionType Type => throw new NotImplementedException();

        public IDiscordInteractionData Data => throw new NotImplementedException();

        public string Token => throw new NotImplementedException();

        public int Version => throw new NotImplementedException();

        public bool HasResponded { get; protected set; } = false;

        public IUser User => throw new NotImplementedException();

        public string UserLocale => throw new NotImplementedException();

        public string GuildLocale => throw new NotImplementedException();

        public bool IsDMInteraction => throw new NotImplementedException();

        public ulong? ChannelId {get; set; } = null;

        public ulong? GuildId { get; set; } = null;

        public ulong ApplicationId { get; set; } = 0;

        public DateTimeOffset CreatedAt => DateTimeOffset.Now;

        public virtual Task DeferAsync(bool ephemeral = false, RequestOptions options = null)
        {
            HasResponded = true;
            return Task.CompletedTask;
        }

        public virtual Task DeleteOriginalResponseAsync(RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IUserMessage> FollowupAsync(string text = null, Embed[] embeds = null, bool isTTS = false, bool ephemeral = false, AllowedMentions allowedMentions = null, MessageComponent components = null, Embed embed = null, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IUserMessage> FollowupWithFilesAsync(IEnumerable<FileAttachment> attachments, string text = null, Embed[] embeds = null, bool isTTS = false, bool ephemeral = false, AllowedMentions allowedMentions = null, MessageComponent components = null, Embed embed = null, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IUserMessage> GetOriginalResponseAsync(RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IUserMessage> ModifyOriginalResponseAsync(Action<MessageProperties> func, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public virtual Task RespondAsync(string text = null, Embed[] embeds = null, bool isTTS = false, bool ephemeral = false, AllowedMentions allowedMentions = null, MessageComponent components = null, Embed embed = null, RequestOptions options = null)
        {
            HasResponded = true;
            return Task.CompletedTask;
        }

        public virtual Task RespondWithFilesAsync(IEnumerable<FileAttachment> attachments, string text = null, Embed[] embeds = null, bool isTTS = false, bool ephemeral = false, AllowedMentions allowedMentions = null, MessageComponent components = null, Embed embed = null, RequestOptions options = null)
        {
            HasResponded = true;
            return Task.CompletedTask;
        }

        public virtual Task RespondWithModalAsync(Modal modal, RequestOptions options = null)
        {
            HasResponded = true;
            return Task.CompletedTask;
        }
    }
}
