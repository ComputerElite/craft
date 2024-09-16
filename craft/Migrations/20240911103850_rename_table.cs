using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace craft.Migrations
{
    public partial class rename_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_filesSystemFileProviders",
                table: "filesSystemFileProviders");

            migrationBuilder.RenameTable(
                name: "filesSystemFileProviders",
                newName: "fileSystemFileProviders");

            migrationBuilder.AddPrimaryKey(
                name: "PK_fileSystemFileProviders",
                table: "fileSystemFileProviders",
                column: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_fileSystemFileProviders",
                table: "fileSystemFileProviders");

            migrationBuilder.RenameTable(
                name: "fileSystemFileProviders",
                newName: "filesSystemFileProviders");

            migrationBuilder.AddPrimaryKey(
                name: "PK_filesSystemFileProviders",
                table: "filesSystemFileProviders",
                column: "id");
        }
    }
}
