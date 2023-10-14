using Discord;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TechMod.Tests.Mocking.FakeClasses
{
    public class FakeMessage : IMessage
    {
        public static List<FakeMessage> Messages = new List<FakeMessage>();

        public bool Deleted { get; private set; } = false;

        public List<FakeReaction> _reactions = new();
        public List<Embed> _embeds = new();

        public List<MessageComponent> _components = new();
        public MessageType Type { get; set; } = MessageType.Default;

        public MessageSource Source { get; set; } = MessageSource.User;

        public bool IsTTS { get; set; } = false;

        public bool IsPinned { get; set; } = false;

        public bool IsSuppressed { get; set; } = false;

        public bool MentionedEveryone { get; set; } = false;

        public string Content { get; set; } = null;

        public string CleanContent { get; set; } = null;

        public DateTimeOffset Timestamp { get; set; } = DateTime.Now;

        public DateTimeOffset? EditedTimestamp { get; set; } = null;

        public IMessageChannel Channel { get; set; } = null;

        public IUser Author { get; set; } = null;

        public IThreadChannel Thread { get; set; } = null;

        public IReadOnlyCollection<IAttachment> Attachments { get; set; } = null;

        public IReadOnlyCollection<IEmbed> Embeds => _embeds;

        public IReadOnlyCollection<ITag> Tags { get; set; } = null;

        public IReadOnlyCollection<ulong> MentionedChannelIds { get; set; } = null;

        public IReadOnlyCollection<ulong> MentionedRoleIds { get; set; } = null;

        public IReadOnlyCollection<ulong> MentionedUserIds { get; set; } = null;

        public MessageActivity Activity { get; set; } = null;

        public MessageApplication Application { get; set; } = null;

        public MessageReference Reference { get; set; } = null;
        public bool Ephemeral { get; set; } = false;

        public IReadOnlyDictionary<IEmote, ReactionMetadata> Reactions { get; }

        public IReadOnlyCollection<IMessageComponent> Components => (IReadOnlyCollection<IMessageComponent>)new ReadOnlyCollection<MessageComponent>(_components);

        public IReadOnlyCollection<IStickerItem> Stickers { get; set; } = null;

        public MessageFlags? Flags { get; set; } = null;

        public IMessageInteraction Interaction { get; set; } = null;

        public MessageRoleSubscriptionData RoleSubscriptionData { get; set; } = null;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

        public ulong Id { get; private set; }

        public FakeMessage()
        {
            var random = new Random();
            Id = (ulong)random.NextInt64();

        }

        

        public async Task AddReactionAsync(IEmote emote, RequestOptions options = null)
        {
            if (!_reactions.Any(p => p.User == Author && p.Emote == emote))
            {
                _reactions.Add(new FakeReaction { Emote = emote , User=Author});
            }
        }

        public Task DeleteAsync(RequestOptions options = null)
        {
            Messages.Remove(this);
            FakeGuild.Guilds.ForEach(m => (m).Channels.ForEach(
                M =>
                (M as FakeMessageChannel)._messages.RemoveAll(p => p.Id == Id)));
            Deleted = true;

            return Task.CompletedTask;
        }

        public static FakeMessage CreateMessage()
        {
            var msg = new FakeMessage();
            Messages.Add(msg);
            return msg;
        }

        public async IAsyncEnumerable<IReadOnlyCollection<IUser>> GetReactionUsersAsync(IEmote emoji, int limit, RequestOptions options = null)
        {
            // Assuming _reactions is a collection of reactions on the message
        var matchingReactions = _reactions.Where(reaction => reaction.Emote.Equals(emoji)).ToList();

        // Create a task with the result
        var task = Task.FromResult<IReadOnlyCollection<IUser>>(matchingReactions.Select(reaction => reaction.User).ToList());

        // Convert the task to an asynchronous enumerable
        yield return (IReadOnlyCollection<IUser>)task;
        }


        public async Task RemoveAllReactionsAsync(RequestOptions options = null)
        {
            _reactions.Clear();
        }

        public async Task RemoveAllReactionsForEmoteAsync(IEmote emote, RequestOptions options = null)
        {
            _reactions.RemoveAll(p => p.Emote == emote);
        }

        public async Task RemoveReactionAsync(IEmote emote, IUser user, RequestOptions options = null)
        {
            _reactions.RemoveAll(p => p.Emote == emote && p.User == user);
        }

        public async Task RemoveReactionAsync(IEmote emote, ulong userId, RequestOptions options = null)
        {
            _reactions.RemoveAll(p => p.Emote == emote && p.User.Id == userId);
        }

        public WeakReference<FakeMessage> GetReference()
        {
            return new WeakReference<FakeMessage>(this);
        }
    }
}
