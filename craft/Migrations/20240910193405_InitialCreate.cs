using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace craft.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "files",
                columns: table => new
                {
                    uuid = table.Column<string>(type: "text", nullable: false),
                    path = table.Column<string>(type: "text", nullable: false),
                    size = table.Column<long>(type: "bigint", nullable: true),
                    lastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    isDirectory = table.Column<bool>(type: "boolean", nullable: false),
                    isFileProviderRoot = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_files", x => x.uuid);
                });

            migrationBuilder.CreateTable(
                name: "permissions",
                columns: table => new
                {
                    permissionUuid = table.Column<string>(type: "text", nullable: false),
                    userUuid = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    path = table.Column<string>(type: "text", nullable: false),
                    isSharedFilePermission = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permissions", x => x.permissionUuid);
                });

            migrationBuilder.CreateTable(
                name: "sessions",
                columns: table => new
                {
                    sessionId = table.Column<string>(type: "text", nullable: false),
                    userUuid = table.Column<string>(type: "text", nullable: false),
                    lastAccess = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    origin = table.Column<string>(type: "text", nullable: false),
                    creationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    validUnti = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sessions", x => x.sessionId);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    uuid = table.Column<string>(type: "text", nullable: false),
                    username = table.Column<string>(type: "text", nullable: true),
                    password = table.Column<string>(type: "text", nullable: true),
                    isPublicLinkUser = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.uuid);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "files");

            migrationBuilder.DropTable(
                name: "permissions");

            migrationBuilder.DropTable(
                name: "sessions");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
