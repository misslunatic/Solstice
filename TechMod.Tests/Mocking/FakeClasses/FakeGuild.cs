using Discord;
using Discord.Audio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechMod.Tests.Mocking.FakeClasses
{
    internal class FakeGuild : IGuild
    {
        public List<FakeRole> _roles = new();
        public static List<FakeGuild> Guilds = new();
        public List<IChannel> Channels = new();

        public string Name => "Test Guild";

        public int AFKTimeout => 600;

        public bool IsWidgetEnabled => throw new NotImplementedException();

        public DefaultMessageNotifications DefaultMessageNotifications => throw new NotImplementedException();

        public MfaLevel MfaLevel => throw new NotImplementedException();

        public VerificationLevel VerificationLevel => throw new NotImplementedException();

        public ExplicitContentFilterLevel ExplicitContentFilter => throw new NotImplementedException();

        public string IconId => throw new NotImplementedException();

        public string IconUrl => throw new NotImplementedException();

        public string SplashId => throw new NotImplementedException();

        public string SplashUrl => throw new NotImplementedException();

        public string DiscoverySplashId => throw new NotImplementedException();

        public string DiscoverySplashUrl => throw new NotImplementedException();

        public bool Available => throw new NotImplementedException();

        public ulong? AFKChannelId => throw new NotImplementedException();

        public ulong? WidgetChannelId => throw new NotImplementedException();

        public ulong? SafetyAlertsChannelId => throw new NotImplementedException();

        public ulong? SystemChannelId => throw new NotImplementedException();

        public ulong? RulesChannelId => throw new NotImplementedException();

        public ulong? PublicUpdatesChannelId => throw new NotImplementedException();

        public ulong OwnerId => throw new NotImplementedException();

        public ulong? ApplicationId => throw new NotImplementedException();

        public string VoiceRegionId => throw new NotImplementedException();

        public IAudioClient AudioClient => throw new NotImplementedException();

        public IRole EveryoneRole { get; set; } = new FakeRole { Name = "Everyone" };

        public IReadOnlyCollection<GuildEmote> Emotes => throw new NotImplementedException();

        public IReadOnlyCollection<ICustomSticker> Stickers => throw new NotImplementedException();

        public GuildFeatures Features => throw new NotImplementedException();

        public IReadOnlyCollection<IRole> Roles => _roles;

        public PremiumTier PremiumTier => throw new NotImplementedException();

        public string BannerId => throw new NotImplementedException();

        public string BannerUrl => throw new NotImplementedException();

        public string VanityURLCode => throw new NotImplementedException();

        public SystemChannelMessageDeny SystemChannelFlags => throw new NotImplementedException();

        public string Description { get; set; } = "No description.";

        public int PremiumSubscriptionCount => throw new NotImplementedException();

        public int? MaxPresences => throw new NotImplementedException();

        public int? MaxMembers => throw new NotImplementedException();

        public int? MaxVideoChannelUsers => throw new NotImplementedException();

        public int? MaxStageVideoChannelUsers => throw new NotImplementedException();

        public int? ApproximateMemberCount => throw new NotImplementedException();

        public int? ApproximatePresenceCount => throw new NotImplementedException();

        public int MaxBitrate => throw new NotImplementedException();

        public string PreferredLocale => throw new NotImplementedException();

        public NsfwLevel NsfwLevel => throw new NotImplementedException();

        public CultureInfo PreferredCulture => throw new NotImplementedException();

        public bool IsBoostProgressBarEnabled => throw new NotImplementedException();

        public ulong MaxUploadLimit => throw new NotImplementedException();

        public DateTimeOffset CreatedAt => throw new NotImplementedException();

        public ulong Id { get; set; }

        public FakeGuild()
        {
            var random = new Random();
            Id = (ulong)random.NextInt64();
            Guilds.Add(this);
        }

        public FakeGuild CreateGuild()
        {
            var guild = new FakeGuild();
            return guild;
        }

        public Task AddBanAsync(IUser user, int pruneDays = 0, string reason = null, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task AddBanAsync(ulong userId, int pruneDays = 0, string reason = null, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IGuildUser> AddGuildUserAsync(ulong userId, string accessToken, Action<AddGuildUserProperties> func = null, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<IApplicationCommand>> BulkOverwriteApplicationCommandsAsync(ApplicationCommandProperties[] properties, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IApplicationCommand> CreateApplicationCommandAsync(ApplicationCommandProperties properties, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IAutoModRule> CreateAutoModRuleAsync(Action<AutoModRuleProperties> props, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<ICategoryChannel> CreateCategoryAsync(string name, Action<GuildChannelProperties> func = null, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<GuildEmote> CreateEmoteAsync(string name, Image image, Optional<IEnumerable<IRole>> roles = default, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IGuildScheduledEvent> CreateEventAsync(string name, DateTimeOffset startTime, GuildScheduledEventType type, GuildScheduledEventPrivacyLevel privacyLevel = GuildScheduledEventPrivacyLevel.Private, string description = null, DateTimeOffset? endTime = null, ulong? channelId = null, string location = null, Image? coverImage = null, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IForumChannel> CreateForumChannelAsync(string name, Action<ForumChannelProperties> func = null, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IRole> CreateRoleAsync(string name, GuildPermissions? permissions = null, Color? color = null, bool isHoisted = false, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IRole> CreateRoleAsync(string name, GuildPermissions? permissions = null, Color? color = null, bool isHoisted = false, bool isMentionable = false, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IStageChannel> CreateStageChannelAsync(string name, Action<VoiceChannelProperties> func = null, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<ICustomSticker> CreateStickerAsync(string name, Image image, IEnumerable<string> tags, string description = null, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<ICustomSticker> CreateStickerAsync(string name, string path, IEnumerable<string> tags, string description = null, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<ICustomSticker> CreateStickerAsync(string name, Stream stream, string filename, IEnumerable<string> tags, string description = null, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<ITextChannel> CreateTextChannelAsync(string name, Action<TextChannelProperties> func = null, RequestOptions options = null)
        {
            if (!Channels.Any(p => p.Name == name))
            {
                var channel = new FakeTextChannel { Name = name };
                Channels.Add(channel);
                return Task.FromResult((ITextChannel)channel);
            }

            // If the channel already exists, return a completed task with null.
            return Task.FromResult<ITextChannel>(null);
        }

        public Task<IVoiceChannel> CreateVoiceChannelAsync(string name, Action<VoiceChannelProperties> func = null, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(RequestOptions options = null)
        {
            Guilds.Remove(this);
            return Task.CompletedTask;
        }

        public Task DeleteEmoteAsync(GuildEmote emote, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task DeleteIntegrationAsync(ulong id, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task DeleteStickerAsync(ICustomSticker sticker, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task DisconnectAsync(IGuildUser user)
        {
            throw new NotImplementedException();
        }

        public Task DownloadUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IVoiceChannel> GetAFKChannelAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IApplicationCommand> GetApplicationCommandAsync(ulong id, CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<IApplicationCommand>> GetApplicationCommandsAsync(bool withLocalizations = false, string locale = null, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<IAuditLogEntry>> GetAuditLogsAsync(int limit = 100, CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null, ulong? beforeId = null, ulong? userId = null, ActionType? actionType = null, ulong? afterId = null)
        {
            throw new NotImplementedException();
        }

        public Task<IAutoModRule> GetAutoModRuleAsync(ulong ruleId, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IAutoModRule[]> GetAutoModRulesAsync(RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IBan> GetBanAsync(IUser user, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IBan> GetBanAsync(ulong userId, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<IReadOnlyCollection<IBan>> GetBansAsync(int limit = 1000, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<IReadOnlyCollection<IBan>> GetBansAsync(ulong fromUserId, Direction dir, int limit = 1000, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<IReadOnlyCollection<IBan>> GetBansAsync(IUser fromUser, Direction dir, int limit = 1000, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<ICategoryChannel>> GetCategoriesAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IGuildChannel> GetChannelAsync(ulong id, CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<IGuildChannel>> GetChannelsAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IGuildUser> GetCurrentUserAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<ITextChannel> GetDefaultChannelAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<GuildEmote> GetEmoteAsync(ulong id, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<GuildEmote>> GetEmotesAsync(RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IGuildScheduledEvent> GetEventAsync(ulong id, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<IGuildScheduledEvent>> GetEventsAsync(RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<IIntegration>> GetIntegrationsAsync(RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<IInviteMetadata>> GetInvitesAsync(RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IGuildOnboarding> GetOnboardingAsync(RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IGuildUser> GetOwnerAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<ITextChannel> GetPublicUpdatesChannelAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public IRole GetRole(ulong id)
        {
            return _roles.First(r => r.Id == id);
        }

        public Task<ITextChannel> GetRulesChannelAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IStageChannel> GetStageChannelAsync(ulong id, CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<IStageChannel>> GetStageChannelsAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<ICustomSticker> GetStickerAsync(ulong id, CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<ICustomSticker>> GetStickersAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<ITextChannel> GetSystemChannelAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<ITextChannel> GetTextChannelAsync(ulong id, CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<ITextChannel>> GetTextChannelsAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IThreadChannel> GetThreadChannelAsync(ulong id, CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<IThreadChannel>> GetThreadChannelsAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IGuildUser> GetUserAsync(ulong id, CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<IGuildUser>> GetUsersAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IInviteMetadata> GetVanityInviteAsync(RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IVoiceChannel> GetVoiceChannelAsync(ulong id, CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<IVoiceChannel>> GetVoiceChannelsAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<IVoiceRegion>> GetVoiceRegionsAsync(RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IWebhook> GetWebhookAsync(ulong id, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<IWebhook>> GetWebhooksAsync(RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<WelcomeScreen> GetWelcomeScreenAsync(RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IGuildChannel> GetWidgetChannelAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task LeaveAsync(RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task ModifyAsync(Action<GuildProperties> func, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<GuildEmote> ModifyEmoteAsync(GuildEmote emote, Action<EmoteProperties> func, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IGuildOnboarding> ModifyOnboardingAsync(Action<GuildOnboardingProperties> props, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<WelcomeScreen> ModifyWelcomeScreenAsync(bool enabled, WelcomeScreenChannelProperties[] channels, string description = null, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task ModifyWidgetAsync(Action<GuildWidgetProperties> func, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task MoveAsync(IGuildUser user, IVoiceChannel targetChannel)
        {
            throw new NotImplementedException();
        }

        public Task<int> PruneUsersAsync(int days = 30, bool simulate = false, RequestOptions options = null, IEnumerable<ulong> includeRoleIds = null)
        {
            throw new NotImplementedException();
        }

        public Task RemoveBanAsync(IUser user, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task RemoveBanAsync(ulong userId, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task ReorderChannelsAsync(IEnumerable<ReorderChannelProperties> args, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task ReorderRolesAsync(IEnumerable<ReorderRoleProperties> args, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<IGuildUser>> SearchUsersAsync(string query, int limit = 1000, CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }
    }
}
