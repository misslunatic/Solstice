using Discord;
using Discord.Interactions;
using Discord.Interactions.Builders;
using Discord.WebSocket;
using ff_cah.Data;
using InteractionFramework.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Timers;
using TechMod.Actions.Lock;
using TechMod.Classes;
using static ff_cah.Data.InfoDB;

namespace ff_cah.Modules
{
    // Interaction modules must be public and inherit from an IInteractionModuleBase
    public class MainModule : InteractionModuleBase<SocketInteractionContext>
    {
        public static InfoDB Database = new();


        

        // Dependencies can be accessed through Property injection, public properties with public setters will be set by the service provider
        public InteractionService Commands { get; set; }

        private InteractionHandler _handler;

        public static void Setup()
        {
            InteractionHandler._client.ButtonExecuted += OnButton;
            
        }

        public static MainModule Module;

        public MainModule()
        {
            Module = this;

        }

        public static async Task OnButton(SocketMessageComponent component)
        {
     
            if (Database.UserHasRequiredRole(InteractionHandler._client.GetGuild(component.GuildId.Value), component.User))
            {
                var votes = Vote.Votes.Where(v =>
                v.Value.Guild.Id == component.GuildId &&
                v.Value.Channel == component.Channel);

                if (votes.Any())
                {
                    var vote = votes.First();
                    vote.Value.OnButton(component);
                }
            }
            }


            public async void Recover()
        {
            LockAction.RecoverFromDatabase();
        }

        public static bool HasRequirements(IGuildUser user)
        {
            return user.GuildPermissions.ManageChannels;
        }

        [EnabledInDm(false)]
        [SlashCommand("cancel", "Cancels a vote or lock. (Manage Channels Required)")]
        public async Task CancelLock()
        {
            if (HasRequirements(Context.Guild.GetUser(Context.User.Id)))
            {
                var act = VoteManager.RunningInServer(Context.Guild, Context.Channel);

                if (act != null)
                {
                    act.Stop(true);
                    Vote.Votes.Remove((Context.Guild, Context.Channel));
                    await RespondAsync(":white_check_mark: Cancelled!");
                }
                else
                {
                    await RespondAsync(":x: There's nothing to cancel here.");
                }

            }
            else
            {
                await RespondAsync(":x: You are not authorized to use this command.");
            }
        }

        [Group("config", "For muting channels (Use to break drama)")]
        public class Config : InteractionModuleBase<SocketInteractionContext>
        {

       
            public enum TiePriority
            {
                Yes,
                No
            }
          
           

            [EnabledInDm(false)]
            [SlashCommand("priority", "Sets the tie priority to break ties. This will default to it on a tie. (Manage Channels Required)")]
            public async Task SetPriority(TiePriority prio)
            {
                if (HasRequirements(Context.Guild.GetUser(Context.User.Id)))
                {
                    Database.SetTiePriority(Context.Guild, prio == TiePriority.Yes);
                    await RespondAsync(":white_check_mark: Set priority to bias towards " + (prio == TiePriority.Yes ? "yes." : "no."));
                }
                else
                {
                    await RespondAsync(":x: You are not authorized to use this command.");
                }
            }

            

            [EnabledInDm(false)]
            [SlashCommand("minimum", "Sets the minimum required votes for something to happen. (Manage Channels Required)")]
            public async Task LockMinimum(int amount)
            {
                
                if (HasRequirements(Context.Guild.GetUser(Context.User.Id)))
                {
                    if (amount >= 0 && amount <= 255)
                    {
                        Database.SetMinimum(Context.Guild, (byte)amount);
                        await RespondAsync($":white_check_mark: The minimum votes was set to {amount}.");
                    }
                    else
                    {
                        await RespondAsync($":x: The minimum votes must be set to at least 0 and at most 255!");

                    }

                }
                else
                {
                    await RespondAsync(":x: You are not authorized to use this command.");
                }
            }

            [EnabledInDm(false)]
            [SlashCommand("setrole", "Sets a required role for users to open or use votes. (Manage Channels Required)")]
            public async Task SetRole(IRole role)
            {

                if (HasRequirements(Context.Guild.GetUser(Context.User.Id)))
                {
                   
                        Database.GetSettingsEnsure(Context.Guild).VoteRole = role.Id;
                        Database.SaveChanges();
                        await RespondAsync($":white_check_mark: The required role is now {role.Name}!");
                    
                 

                }
                else
                {
                    await RespondAsync(":x: You are not authorized to use this command.");
                }
            }

            /*
             *   
            */

            [EnabledInDm(false)]
            [SlashCommand("removerole", "Removes the required role for users to open or use votes. (Manage Channels Required)")]
            public async Task ClearRole()
            {

                if (HasRequirements(Context.Guild.GetUser(Context.User.Id)))
                {

                        Database.GetSettingsEnsure(Context.Guild).VoteRole = 0;
                        Database.SaveChanges();
                        await RespondAsync($":white_check_mark: The required role was removed!");
                }
                else
                {
                    await RespondAsync(":x: You are not authorized to use this command.");
                }
            }
        }

    }
}