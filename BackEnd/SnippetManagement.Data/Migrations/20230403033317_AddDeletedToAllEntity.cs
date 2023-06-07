using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnippetManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDeletedToAllEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Tag",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "SnippetTag",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "SnippetTag");
        }
    }
}
