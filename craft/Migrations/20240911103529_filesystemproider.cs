using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace craft.Migrations
{
    public partial class filesystemproider : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "filesSystemFileProviders",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    rootPath = table.Column<string>(type: "text", nullable: true),
                    mountPoint = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_filesSystemFileProviders", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "filesSystemFileProviders");
        }
    }
}
