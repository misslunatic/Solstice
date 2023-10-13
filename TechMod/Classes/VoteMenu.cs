using Discord.WebSocket;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ff_cah.Data.InfoDB;
using ff_cah.Modules;
using ff_cah.Data;
using TechMod.Actions.Lock;

namespace TechMod.Classes
{
    public class Vote : VoteObject
    {
        public static Dictionary<(IGuild, IMessageChannel), Vote> Votes = new();

        public static InfoDB Database => MainModule.Database;
        public DateTime EndDate;
        public DateTime StartDate;
        public System.Timers.Timer StepTimer;
        public IUserMessage Message;
        public Dictionary<IUser, bool> Users = new();
        public bool LoadedFromDB = false;
        public object? UserData;
        public float VoteDuration = 0.5f;


        public int VotedYes => Users.Where(p => p.Value).Count();
        public int VotedNo => Users.Where(p => !p.Value).Count();
        public bool CurrentAction
        {
            get
            {
                var min = Database.GetMinimum(Guild);
                var yes = VotedYes;
                var no = VotedNo;
                if (no >= min || yes >= min)
                {
                    if (yes > no)
                    {
                        return true;
                    }
                    else if (yes < no)
                    {
                        return false;
                    }
                    // Can assume they are equal.
                    return Database.GetTiePriority(Guild);
                }
                return false;
            }
        }

        public void OnButton(SocketMessageComponent component)
        {
            switch (component.Data.CustomId)
            {
                case "no":
                    if (!Users.ContainsKey(component.User))
                    {
                        Users.Add(component.User, false);
                        UpdateVoteMessage();
                    }
                    else
                    {
                        if (Users[component.User])
                        {
                            Users[component.User] = false;
                            UpdateVoteMessage();
                        }
                    }
                    break;
                case "yes":
                    if (!Users.ContainsKey(component.User))
                    {
                        Users.Add(component.User, true);
                        UpdateVoteMessage();

                    }
                    else
                    {
                        
                        if (!Users[component.User])
                        {
                            Users[component.User] = true;
                            UpdateVoteMessage();
                        }
                    }
                    break;
                case "remove-vote":
                    if (Users.ContainsKey(component.User))
                    {
                        Users.Remove(component.User);
                        UpdateVoteMessage();

                    }
                    break;
            }
            component.DeferAsync(ephemeral: true);
        }

        public Vote(IMessageChannel channel, IGuild guild, object? arg) : base(guild, channel)
        {
            EndDate = DateTime.Now;
            EndDate.AddMinutes(VoteDuration);
            Channel = channel;
            Guild = guild;
        }



        public void UpdateVoteMessage()
        {

            var eb = new EmbedBuilder()
                .WithColor(Color.Orange)
                .WithCurrentTimestamp()
                                    .WithTitle(GetTitle())

                .WithDescription(GetDescription() + "\n\n" + GetVoterDesc() + "\n" + Utils.GetProgressBar(StartDate, EndDate, DateTime.Now, CurrentAction ? ":green_square:" : ":red_square:"));

            Message.ModifyAsync(m => m.Embed = eb.Build());
        }

        public virtual string GetTitle()
        {
            return "This isn't set up right...";
        }

        public virtual string GetNoMessage()
        {
            return "...";
        }

        public virtual string GetDescription()
        {
            return "No description.";
        }

        public async void BeginVote()
        {
            StartDate = DateTime.Now;
            EndDate = StartDate.AddMinutes(VoteDuration);
            var cb = new ComponentBuilder()
                .WithButton("Yes", "yes", style: ButtonStyle.Success)
                .WithButton("No", "no", ButtonStyle.Danger)
                .WithButton("Remove my Vote", "remove-vote");

            var eb = new EmbedBuilder()
                .WithColor(Color.Orange)
                .WithCurrentTimestamp()
                .WithTitle(GetTitle())

                .WithDescription(GetDescription()+"\n\n"+GetVoterDesc() + "\n\n" + Utils.GetProgressBar(StartDate, EndDate, DateTime.Now));

            Message = await Channel.SendMessageAsync($"A vote has begun! \n***At least {Database.GetMinimum(Guild)} must vote on something for it to happen!***", components: cb.Build(), embed: eb.Build());
            StepTimer = new System.Timers.Timer((EndDate - StartDate).TotalMilliseconds / Utils.BarLength);
            StepTimer.Elapsed += (object? sender, System.Timers.ElapsedEventArgs args) =>
            {

                UpdateVoteMessage();
                if (DateTime.Now >= EndDate)
                {
                    TakeAction();
                }
                else
                {
                    StepTimer.Stop(); // Steps of ten for progress bar.
                    StepTimer.Start();
                }
            };
            StepTimer.Start();
        }

        public string GetVoterDesc()
        {
            var min = Database.GetMinimum(Guild);
            var desc = $"## :white_check_mark: {VotedYes}/{min}\n" +
                $"## :x: {VotedNo}/{min}";


            return desc;
        }

        public virtual void CreateAction(Vote vote)
        {
            Console.WriteLine("Base action!");
        }

        public override void Finish()
        {
            base.Finish();
            Votes[(Guild, Channel)] = null;
        }

        public override void Stop(bool cancelled=false)
        {
            base.Stop(cancelled);
            StepTimer.Stop();

        }

        public async void TakeAction()
        {
            Stop();
            try
            {
                await Message.ModifyAsync(m => {
                    m.Content = "The voting has ended.";
                    m.Components = null;
                    });
                if (CurrentAction)
                {
                    // Lock
                    CreateAction(this);
                }
                else
                {
                    // No lock
                    var eb = new EmbedBuilder()
                        .WithCurrentTimestamp()
                        .WithColor(Color.Red)
                        .WithDescription($"# :x: {GetNoMessage()}")
                        .WithFooter("To break ties, servers can configure a priority via /lockpriority.\n" +
                        "Currently, it prioritizes " + (Database.GetTiePriority(Guild) ? "Yes." : "No."));

                    await Message.ReplyAsync(":x: Vote resulted in a no.", embed: eb.Build());
                    
                }
            }
            catch (Exception ex)
            {
                _ = Channel.SendMessageAsync(":x: Error handling action: " + ex.Message);
            }
            Votes.Remove((Guild,Channel));
            
        }
    }
}
