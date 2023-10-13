using Discord;
using TechMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using TechMod.Classes;
using static TechMod.Data.InfoDB;

namespace TechMod.Actions.Lock
{
    public class LockAction : VoteAction
    {
        public System.Timers.Timer StepTimer = new();
        public static Dictionary<(IGuild, IMessageChannel), LockAction> Locks = new();

        public DateTime EndDate;
        public DateTime StartDate;
        public IUserMessage Message;

        public LockAction(Vote vote) : base(vote)
        {
            Console.WriteLine("Lock action");
            var f = (float)vote.UserData;
            StartDate = DateTime.Now;
            EndDate = StartDate.AddMinutes(f);
            Locks.Add((Guild, Channel), this);
            BeginLock();
        }

        public LockAction(IGuild guild, IMessageChannel channel, IUserMessage msg, ChannelMute mute)  : base(guild, channel)
        {
            EndDate = mute.UnmuteTime;
            StartDate = mute.BeginTime;
            Message = msg;
            Locks.Add((Guild, Channel), this);
            ResumeLock();
        }

        public void ResumeLock()
        {
            UpdateLockedMessage();

            StepTimer = new((EndDate - StartDate).TotalMilliseconds / Config.BarLength); // Steps of ten for progress bar.
            StepTimer.Elapsed += (object? sender, System.Timers.ElapsedEventArgs args) =>
            {

                UpdateLockedMessage();
                if (DateTime.Now >= EndDate)
                {
                    Stop();
                }
                else
                {
                    StepTimer.Stop(); // Steps of ten for progress bar.
                    StepTimer.Start();
                }
            };

            StepTimer.Start();
        }

        public static async void RecoverFromDatabase()
        {
            foreach (var mute in Database.ChannelMutes)
            {
                var g = InteractionHandler._client.GetGuild(mute.Guild);
                var c = g.GetTextChannel(mute.Channel); // Using GetTextChannel instead of GetChannel for IMessageChannel

                try
                {
                    var messages = await c.GetMessagesAsync(1).FlattenAsync();
                    var msg = messages.FirstOrDefault() as IUserMessage;

                    if (msg != null)
                    {
                        Locks.Add((g, c),new LockAction(g, c, msg, mute));

                    }
                    else
                    {
                        await c.SendMessageAsync("# :x: No messages found in the channel.");
                    }
                }
                catch (Exception ex)
                {
                    await c.SendMessageAsync("# :x: Failed to restore from database!\nError: " + ex.Message);
                }
            }
        }

        public override void Finish()
        {
            base.Finish();
            Locks[(Guild, Channel)] = null;
        }

        public override void Stop(bool cancelled=false)
        {
            base.Stop(cancelled);

            StepTimer.Stop();
            var c = Guild.GetChannelAsync(Channel.Id).Result;
            var pQ = c.GetPermissionOverwrite(Guild.EveryoneRole);

            c.RemovePermissionOverwriteAsync(Guild.EveryoneRole).RunSynchronously();

            Message.UnpinAsync();
            Message.ModifyAsync(m => m.Content = "## :unlock: Lock has expired.");
            Database.ChannelMutes.RemoveRange(Database.ChannelMutes.Where(m => m.Channel == Channel.Id && m.Guild == Guild.Id));
            Database.SaveChanges();

          
                if (cancelled)
                {
                    Channel.SendMessageAsync("# :unlock: Cancelled!");
                }
                else
                {
                    Channel.SendMessageAsync("# :unlock: The lock has ended!");
                }
            
            StepTimer.Dispose();

            Locks.Remove((Guild, Channel));
        }

        public void UpdateLockedMessage()
        {
            var eb = new EmbedBuilder()
                .WithColor(Color.Green)
                .WithCurrentTimestamp()
                .WithTitle($":lock: Channel locked for {string.Format(@"{0:hh\:mm\:ss}", TimeSpan.FromTicks((EndDate - DateTime.Now).Ticks))}")
                .WithDescription(Utils.GetProgressBar(StartDate, EndDate, DateTime.Now));

            Message.ModifyAsync(m => m.Embed = eb.Build());
        }

        public async void BeginLock()
        {
            var c = await Guild.GetChannelAsync(Channel.Id);
            var pQ = c.GetPermissionOverwrite(Guild.EveryoneRole);

            await c.AddPermissionOverwriteAsync(Guild.EveryoneRole, new OverwritePermissions(sendMessages: PermValue.Deny));

            // Set up message
            var eb = new EmbedBuilder()
                .WithColor(Color.Green)
                .WithCurrentTimestamp()
                .WithDescription(Utils.GetProgressBar(StartDate, EndDate, DateTime.Now));

            Message = await Channel.SendMessageAsync("# :lock: This channel has been locked!", embed: eb.Build());
            _ = Message.PinAsync();


            // Write to database
            Database.ChannelMutes.Add(new ChannelMute { Channel = Channel.Id, Guild = Guild.Id, UnmuteTime = EndDate, BeginTime = StartDate, LockMessage = Message.Id, Yes = (ushort)Yes, No = (ushort)No });
            Database.SaveChanges();
            StepTimer.Stop();
            StepTimer = new((EndDate - StartDate).TotalMilliseconds / Config.BarLength); // Steps of ten for progress bar.
            StepTimer.Elapsed += (object? sender, System.Timers.ElapsedEventArgs args) =>
            {

                UpdateLockedMessage();
                if (DateTime.Now >= EndDate)
                {
                    Stop();
                }
                else
                {
                    StepTimer.Stop(); // Steps of ten for progress bar.
                    StepTimer.Start();
                }
            };
            StepTimer.Start();
        }

    }
}
