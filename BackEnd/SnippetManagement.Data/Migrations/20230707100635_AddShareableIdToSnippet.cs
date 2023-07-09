using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnippetManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddShareableIdToSnippet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Snippet",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValue: new Guid("3b094ed2-5eb3-4b6e-150c-08db05b7cf56"));

            migrationBuilder.AddColumn<Guid>(
                name: "ShareableId",
                table: "Snippet",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShareableId",
                table: "Snippet");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Snippet",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("3b094ed2-5eb3-4b6e-150c-08db05b7cf56"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }
    }
}
