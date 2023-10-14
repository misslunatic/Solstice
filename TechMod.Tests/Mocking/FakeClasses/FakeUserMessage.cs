using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace TechMod.Tests.Mocking.FakeClasses
{
    public class FakeUserMessage : FakeMessage, IUserMessage
    {
        public IUserMessage ReferencedMessage { get; set; } = null;
        protected bool _pinned { get; private set; }

        public Task CrosspostAsync(RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public static FakeUserMessage CreateUserMessage()
        {
            var msg = new FakeUserMessage();
            Messages.Add(msg);
            return msg;
        }


        public Task ModifyAsync(Action<MessageProperties> func, RequestOptions options = null)
        {
            var msgData = new MessageProperties();
            // Downcast!
            if(_components != null && _components.Any())
                msgData.Components = Optional.Create((MessageComponent)_components.First());
            msgData.Flags = msgData.Flags;
            msgData.Embeds = msgData.Embeds;
            msgData.Content = msgData.Content;


            func?.Invoke(msgData);

            // Now that msgData has been updated... Do the above in reverse!
            _components.Clear();
            _components.Add(msgData.Components.Value);

            _embeds = msgData.Embeds.IsSpecified ? msgData.Embeds.Value.ToList() : null;
            Flags = msgData.Flags.IsSpecified ? msgData.Flags.Value : null;
            Content = msgData.Content.IsSpecified ? msgData.Content.Value : null;

            return Task.CompletedTask;
        }

        public async Task PinAsync(RequestOptions options = null)
        {
            _pinned = true;
        }

        public string Resolve(TagHandling userHandling = TagHandling.Name, TagHandling channelHandling = TagHandling.Name, TagHandling roleHandling = TagHandling.Name, TagHandling everyoneHandling = TagHandling.Ignore, TagHandling emojiHandling = TagHandling.Name)
        {
            throw new NotImplementedException();
        }

        public async Task UnpinAsync(RequestOptions options = null)
        {
            _pinned = false;
        }
    }
}
