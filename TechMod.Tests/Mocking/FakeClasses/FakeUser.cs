using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechMod.Tests.Mocking.FakeClasses
{
    public class FakeUser : IUser
    {
        public static FakeUser BotUser = new FakeUser("Bot");
        protected string _username = "user-" + Guid.NewGuid().ToString();
        protected ulong _id = 0;
        public static List<FakeUser> Users { get; set; } = new List<FakeUser>();

        public static FakeUser CreateUser()
        {
            var user = new FakeUser();
            Users.Add(user);
            return user;
        }
        public FakeUser(string nick="")
        {
            if (!string.IsNullOrWhiteSpace(nick))
                _username = nick;
            var random = new Random();
            _id = (ulong)random.NextInt64();
        }

        public string AvatarId => "";

        public string Discriminator => "0000";

        public ushort DiscriminatorValue => 0;

        public bool IsBot => false;

        public bool IsWebhook => throw new NotImplementedException();

        public string Username => _username;

        public UserProperties? PublicFlags => throw new NotImplementedException();

        public string GlobalName => throw new NotImplementedException();

        public DateTimeOffset CreatedAt => DateTime.Now;

        public ulong Id => throw new NotImplementedException();

        public string Mention => $"<@{Id}>";

        public UserStatus Status => UserStatus.Online;

        public IReadOnlyCollection<ClientType> ActiveClients => throw new NotImplementedException();

        public IReadOnlyCollection<IActivity> Activities => throw new NotImplementedException();

        public Task<IDMChannel> CreateDMChannelAsync(RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public string GetAvatarUrl(ImageFormat format = ImageFormat.Auto, ushort size = 128)
        {
            throw new NotImplementedException();
        }

        public string GetDefaultAvatarUrl()
        {
            throw new NotImplementedException();
        }
    }
}
