using Discord;
using Discord.WebSocket;
using TechMod.Classes.Util;
using TechMod.Data;
using TechMod.Modules;

namespace TechMod.Classes.Triggers
{
    /// <summary>
    /// This operates the Vote menu.
    /// You are intended to inherit from it to do custom ones.
    /// </summary>
    public class Vote : BotSystem
    {
        /// Global dictionary of the votes (All votes should be held here when running)
        public static Dictionary<(IGuild, IMessageChannel), Vote> Votes = new();

        #region Actions
        public Action OnCreateAction;
        public Action OnRefresh;
        public Action OnVoteNo;
        public Action OnVoteCancel;
        #endregion

        #region Vote Internal
        public static InfoDB Database => MainModule.Database;
        public DateTime EndDate;
        public DateTime StartDate;
        public System.Timers.Timer StepTimer;
        public IUserMessage Message;
        public Dictionary<IUser, bool> Users = new();
        public bool LoadedFromDB = false;
        #endregion




        public void SetVoteDuration(float duration)
        {
            VoteDuration = duration;
            EndDate = StartDate.AddMinutes(duration);
            UpdateVoteMessage();
        }

        #region Vote Information
        public float VoteDuration { get; private set; } = Config.MinutesToVote;
        public byte Minimum;
        /// If there is a tie, should we do yes or no?
        public bool DefaultOnTie = false;

        /// Used to store whatever data should be used after the vote (A parameter passthrough, basically)
        public object? UserData;
        public int VotedYes => Users.Where(p => p.Value).Count();
        public int VotedNo => Users.Where(p => !p.Value).Count();
        /// Whether or not, given how things are now, the answer would be yes or no.
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
                    return DefaultOnTie;
                }
                return false;
            }
        }
        #endregion

        #region Constructors
        public Vote(BotSystemManager manager, IMessageChannel channel, IGuild guild, object? arg) : base(manager, guild, channel)
        {
            EndDate = DateTime.Now;
            EndDate.AddMinutes(VoteDuration);
            Channel = channel;
            Guild = guild;
            Database.GetMinimum(Guild);
            DefaultOnTie = Database.GetTiePriority(guild);
            UserData = arg;
        }
        #endregion

        #region Event Handlers

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

        #endregion



        #region External Methods (API)

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

                .WithDescription(GetDescription() + "\n\n" + GetPollAsString() + "\n\n" + Utils.GetProgressBar(StartDate, EndDate, DateTime.Now));

            Message = await Channel.SendMessageAsync($"A vote has begun! \n***At least {Minimum} must vote on something for it to happen!***", components: cb.Build(), embed: eb.Build());
            StepTimer = new System.Timers.Timer((EndDate - StartDate).TotalMilliseconds / Config.BarLength);
            StepTimer.Elapsed += (sender, args) =>
            {

                UpdateVoteMessage();
                if (DateTime.Now >= EndDate)
                {
                    StepTimer.Stop(); // Steps of ten for progress bar.
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

        public string GetPollAsString()
        {
            var min = Minimum;
            var desc = $"## :white_check_mark: {VotedYes}/{min}\n" +
                $"## :x: {VotedNo}/{min}";


            return desc;
        }


        /// <summary>
        /// Updates the vote message, if it exists.
        /// </summary>
        public void UpdateVoteMessage()
        {
            OnRefresh?.Invoke();
            if (Message != null)
            {
                var eb = new EmbedBuilder()
                    .WithColor(Color.Orange)
                    .WithCurrentTimestamp()
                                        .WithTitle(GetTitle())

                    .WithDescription(GetDescription() + "\n\n" + GetPollAsString() + "\n" + Utils.GetProgressBar(StartDate, EndDate, DateTime.Now, CurrentAction ? ":green_square:" : ":red_square:"));

                Message.ModifyAsync(m => m.Embed = eb.Build());
            }
        }

        /// <summary>
        /// When the vote is up, this will be canlled to handle the results.
        /// </summary>
        public async void TakeAction()
        {
            Stop();
            try
            {
                await Message.ModifyAsync(m =>
                {
                    m.Content = "The voting has ended.";
                    m.Components = null;
                });
                if (CurrentAction)
                {
                    // Lock
                    OnCreateAction?.Invoke();
                    CreateAction(this);
                }
                else
                {
                    OnVoteNo?.Invoke();
                    // No lock
                    var eb = new EmbedBuilder()
                        .WithCurrentTimestamp()
                        .WithColor(Color.Red)
                        .WithDescription($"# :x: {GetNoMessage()}")
                        .WithFooter("To break ties, servers can configure a priority via /lockpriority.\n" +
                        "Currently, it prioritizes " + (DefaultOnTie ? "Yes." : "No."));

                    await Message.ReplyAsync(":x: Vote resulted in a no.", embed: eb.Build());

                }
            }
            catch (Exception ex)
            {
                _ = Channel.SendMessageAsync(":x: Error handling action: " + ex.Message);
            }
            Votes.Remove((Guild, Channel));

        }

        public override void Stop(bool cancelled = false)
        {
            if (cancelled)
                OnVoteCancel?.Invoke();
            base.Stop(cancelled);
            StepTimer.Stop();

        }

        #endregion
        #region Internal / Protected Methods

        public virtual void CreateAction(Vote vote)
        {

        }

        public override void Finish()
        {
            base.Finish();
            Votes[(Guild, Channel)] = null;
        }
        #endregion
    }
}
