﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechMod.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChannelMute",
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
                    table.PrimaryKey("PK_ChannelMute", x => x.ChannelMuteId);
                });

            migrationBuilder.CreateTable(
                name: "GuildSettings",
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
                    table.PrimaryKey("PK_GuildSettings", x => x.GuildSettingsId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChannelMute");

            migrationBuilder.DropTable(
                name: "GuildSettings");
        }
    }
}
