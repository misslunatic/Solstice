using Discord;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Threading;

namespace TechMod.Data
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

        [Table("ChannelMute")]
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

        [Table("GuildSettings")]
        public class GuildSettings
        {
            public int GuildSettingsId { get; set; }
            public byte MinimumToConsider { get; set; } = 4;
            public bool TiePriority { get; set;} = false;
            public ulong Guild { get; set; }
            public ulong VoteRole { get; set; } = 0;
        }

        public static Mutex mutex = new();

        public GuildSettings GetSettingsEnsure(IGuild guild)
        {
            var guilds = ServerSettings.Where(g => g.Guild == guild.Id);
            if (guilds.Any())
            {
                return guilds.First();
            }
            var newGuild = new GuildSettings { Guild = guild.Id };
            ServerSettings.Add(newGuild);
            return newGuild;
        }

        public GuildSettings? GetSettings(IGuild guild)
        {
            mutex.WaitOne();
            var guilds = ServerSettings.Where(g => g.Guild == guild.Id);
            if(guilds.Any())
            {
                mutex.ReleaseMutex();
                return guilds.First();
            }
            mutex.ReleaseMutex();
            return null;
        }

        public byte GetMinimum(IGuild guild)
        {
            mutex.WaitOne();

            var g = GetSettings(guild);
            if(g == null)
            {
                mutex.ReleaseMutex();
                return 4;
            }
            mutex.ReleaseMutex();
            return g.MinimumToConsider;
        }

        public bool GetTiePriority(IGuild guild)
        {
            mutex.WaitOne();

            var g = GetSettings(guild);
            if (g == null)
            {
                mutex.ReleaseMutex();
                return false;
            }
            
            mutex.ReleaseMutex();
            return true;
        }

        public IRole? UserRequiredRole(IGuild guild)
        {
            var db = ServerSettings.Where(r => r.Guild == guild.Id);
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
            var db = ServerSettings.Where(r => r.Guild == guild.Id);
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

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            mutex.WaitOne();
            Database.Migrate();
            base.ConfigureConventions(configurationBuilder);
            mutex.ReleaseMutex();
        }

        public InfoDB()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            Console.WriteLine(Config.DatabasePath);
            DbPath = Config.DatabasePath;
               
        }

        public DbSet<ChannelMute> ChannelMutes { get; set; }
        public DbSet<GuildSettings> ServerSettings { get; set; }


        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}
