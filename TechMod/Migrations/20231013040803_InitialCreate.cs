using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechMod.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Guilds",
                columns: table => new
                {
                    GuildSettingsId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MinimumToConsider = table.Column<byte>(type: "INTEGER", nullable: false),
                    TiePriority = table.Column<bool>(type: "INTEGER", nullable: false),
                    Guild = table.Column<ulong>(type: "INTEGER", nullable: false),
                    VoteRole = table.Column<ulong>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guilds", x => x.GuildSettingsId);
                });

            migrationBuilder.CreateTable(
                name: "Mutes",
                columns: table => new
                {
                    ChannelMuteId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UnmuteTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    BeginTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Channel = table.Column<ulong>(type: "INTEGER", nullable: false),
                    Yes = table.Column<ushort>(type: "INTEGER", nullable: false),
                    No = table.Column<ushort>(type: "INTEGER", nullable: false),
                    Guild = table.Column<ulong>(type: "INTEGER", nullable: false),
                    LockMessage = table.Column<ulong>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mutes", x => x.ChannelMuteId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Guilds");

            migrationBuilder.DropTable(
                name: "Mutes");
        }
    }
}
