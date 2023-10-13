using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.FileSystemGlobbing;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Text.RegularExpressions;

namespace ff_cah.Data
{
    public class DBContextFactory : IDesignTimeDbContextFactory<InfoDB>
    {
        public InfoDB CreateDbContext(string[] args)
        {


            return new InfoDB();
        }
    }
    public class InfoDB : DbContext
    {
        public string DbPath = "";
      
        public class ChannelMute
        {
            public int ChannelMuteId { get; set; }
            public DateTime UnmuteTime { get; set; }
            public DateTime BeginTime { get; set; }
            public ulong Channel { get; set; }
            public ushort Yes { get; set; }
            public ushort No { get; set; }
            public ulong Guild { get; set; }
            public ulong LockMessage { get; set; }

        }

        public class GuildSettings
        {
            public int GuildSettingsId { get; set; }
            public byte MinimumToConsider { get; set; } = 4;
            public bool TiePriority { get; set;} = false;
            public ulong Guild { get; set; }
            public ulong VoteRole { get; set; } = 0;
        }

        public GuildSettings GetSettingsEnsure(IGuild guild)
        {
            var guilds = Guilds.Where(g => g.Guild == guild.Id);
            if (guilds.Any())
            {
                return guilds.First();
            }
            var newGuild = new GuildSettings { Guild = guild.Id };
            Guilds.Add(newGuild);
            return newGuild;
        }

        public GuildSettings? GetSettings(IGuild guild)
        {
            var guilds = Guilds.Where(g => g.Guild == guild.Id);
            if(guilds.Any())
            {
                return guilds.First();
            }
            return null;
        }

        public byte GetMinimum(IGuild guild)
        {
            var g = GetSettings(guild);
            if(g == null)
            {
                return 4;
            }
            return g.MinimumToConsider;
        }

        public bool GetTiePriority(IGuild guild)
        {
            var g = GetSettings(guild);
            if (g == null)
            {
                return false;
            }
            return true;
        }

        public IRole? UserRequiredRole(IGuild guild)
        {
            var db = Guilds.Where(r => r.Guild == guild.Id);
            var id = 0xDEADBEEFDEADBEEF;
            if (db.Any())
            {
                id = db.First().VoteRole;
            }
            if (id == 0) return null;

            var roles = guild.Roles.Where(r => r.Id == id);
            if (!roles.Any())
                return null;

            return roles.First();
        }

        public  bool UserHasRequiredRole(IGuild guild, IUser user)
        {
            var db = Guilds.Where(r => r.Guild == guild.Id);
            var id = 0xDEADBEEFDEADBEEF;
            if (db.Any())
                id = db.First().VoteRole;
            else
                return true;
            if (id == 0) return true;

            return guild.GetUserAsync(user.Id).Result.RoleIds.Where(r => r == id).Any();
        }

        public void SetTiePriority(IGuild guild, bool prio)
        {
            var g = GetSettingsEnsure(guild);
            g.TiePriority = prio;
            SaveChanges();
        }

        public void SetMinimum(IGuild guild, byte amount)
        {
            var g = GetSettingsEnsure(guild);
            g.MinimumToConsider = amount;
            SaveChanges();

        }


        public InfoDB()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = "C:\\Users\\super\\source\\repos\\TechMod\\TechMod\\data.db";
        }

        public DbSet<ChannelMute> Mutes { get; set; }
        public DbSet<GuildSettings> Guilds { get; set; }


        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}
